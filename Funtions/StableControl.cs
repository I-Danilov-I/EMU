using System.Diagnostics;
using System.Net.NetworkInformation;

namespace EMU
{
    internal class StableControl
    {
        private readonly Logging logging;
        private readonly DeviceControl deviceControl;


        public StableControl(Logging logging, DeviceControl deviceControl)
        {
            this.logging = logging;
            this.deviceControl = deviceControl;

        }


        internal void Control()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            logging.LogAndConsoleWirite("\n\n[Stabilitätskontrolle]");
            logging.LogAndConsoleWirite("-------------------------------------------------------------------------------------------");

            CheckNetworkStatus();
            CheckNoxStatus();
            CheckADBStatus();
            CheckNoxNetworkStatus();
            CheckAppStatus();
            CheckAccountUsage();

            logging.LogAndConsoleWirite("-------------------------------------------------------------------------------------------");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
        }


        private void LogStatus(string statusType, bool isOk, string details)
        {
            string status = isOk ? "OK" : "Not OK";
            int typeWidth = 20;
            int statusWidth = 10;
            int detailsWidth = 50;

            string formattedType = statusType.PadRight(typeWidth);
            string formattedStatus = status.PadRight(statusWidth);
            string formattedDetails = details.PadRight(detailsWidth);

            string logMessage = $"{formattedType} | {formattedStatus} | {formattedDetails}";
            logging.LogAndConsoleWirite(logMessage);
        }




        // [ VM ] ###################################################################################
        private void CheckNetworkStatus()
        {
            bool isNetworkAvailable = IsNetworkAvailable();
            LogStatus("NETWORK", isNetworkAvailable, isNetworkAvailable ? "Das VM-Netzwerk ist online und funktionsfähig." : "Das VM-Netzwerk ist offline, Verbindungsprüfung wird gestartet.");
            if (!isNetworkAvailable)
            {
                HandleNetworkError();
            }
        }

        internal bool IsNetworkAvailable() => NetworkInterface.GetIsNetworkAvailable();

        private void HandleNetworkError()
        {
            LogStatus("ERROR", false, $"Das VM-Netzwerk ist offline. Es wird ein erneuter Verbindungsversuch in {Program.reconnectSleepTime} Minuten gestartet.");
            Thread.Sleep(TimeSpan.FromMinutes(Program.reconnectSleepTime));
            throw new Exception("Netzwerk nicht verfügbar.");
        }




        // [ GAME ] ###################################################################################
        private void CheckAccountUsage()
        {
            deviceControl.TakeScreenshot();
            bool isAccountInUse = deviceControl.CheckTextInScreenshot("Tipps", "Konto");

            // Log-Nachricht wird nun in jedem Fall ausgegeben, unabhängig davon, ob das Konto verwendet wird oder nicht.
            LogStatus("ACCOUNT", !isAccountInUse, isAccountInUse ? $"Konto wird verwendet, erneuter Versuch in {Program.reconnectSleepTime} Minuten." : "Konto ist frei und nicht in Verwendung.");

            if (isAccountInUse)
            {
                CloseApp();
                Thread.Sleep(TimeSpan.FromMinutes(Program.reconnectSleepTime));
                StartApp();
                throw new Exception("Konto derzeit in Verwendung.");
            }
        }




        // [ ADB ] ###################################################################################
        internal void StartADBConnection()
        {
            // Startet den ADB-Server
            deviceControl.ExecuteAdbCommand("start-server");

            // Versucht, eine Verbindung zu einem spezifischen Gerät herzustellen
            string deviceIP = "127.0.0.1:5555"; // Beispiel-IP und Port für ein verbundenes Gerät
            string connectCommand = $"connect {deviceIP}";
            string output = deviceControl.ExecuteAdbCommand(connectCommand);

            // Überprüft, ob die Verbindung erfolgreich war
            if (output.Contains("connected to"))
            {
                LogStatus("ADB", true, deviceIP);

            }
            else
            {
                LogStatus("ADB", false, output);
            }
        }

        private void DisconnectADB(string deviceIP)
        {
            // ADB-Befehl zum gezielten Trennen des Geräts
            string adbCommand = $"disconnect {deviceIP}";
            string output = deviceControl.ExecuteAdbCommand(adbCommand);

            // Log-Nachricht, um den Status zu bestätigen
            if (output.Contains("disconnected"))
            {
                LogStatus("ADB", true, $"Erfolgreich getrennt von {deviceIP}");
            }
            else
            {
                LogStatus("ADB", false, $"Trennen fehlgeschlagen: {output}");
            }
        }

        internal bool IsADBConnected()
        {
            string output = deviceControl.ExecuteAdbCommand("devices");
            return output.Contains("device");
        }

        private void CheckADBStatus()
        {
            bool isADBConnected = IsADBConnected();
            LogStatus("ADB", isADBConnected, isADBConnected ? "ADB Verbindung ist etabliert." : "ADB Verbindung ist nicht etabliert, Verbindung wird hergestellt.");
            if (!isADBConnected)
            {
                StartADBConnection();
            }
        }




        // [ NOX ] ###################################################################################
        internal void StartNoxPlayer()
        {
            try
            {
                // Überprüfen, ob der Pfad existiert
                if (!File.Exists(Program.noxExePath))
                {
                    LogStatus("NOX", false, "Der angegebene Pfad zu Nox wurde nicht gefunden: " + Program.noxExePath);
                    Thread.Sleep(1000 * 60);
                }

                Process.Start(Program.noxExePath);

                // Warte, bis Nox vollständig gestartet ist
                while (!IsNoxReady())
                {
                    Thread.Sleep(5000); // Überprüfe alle 5 Sekunden
                }
                Thread.Sleep(10000);
            }
            catch (Exception ex)
            {
                LogStatus("NOX", false, ex.Message);
                Thread.Sleep(1000 * 60);
            }
        }

        internal void KillNoxPlayerProcess()
        {
            foreach (var process in Process.GetProcessesByName("Nox"))
            {
                process.Kill();
            }
            Thread.Sleep(10000);
        }

        internal void RestartNoxPlayer()
        {
            KillNoxPlayerProcess(); // Stoppt alle laufenden Nox-Prozesse
            StartNoxPlayer(); // Startet den Nox Player neu
            LogStatus("NOX", true, "Nox Player wird neu gestartet.");
        }

        internal bool IsNoxReady()
        {
            string output = deviceControl.ExecuteAdbCommand("shell getprop init.svc.bootanim");
            return output.Contains("stopped");
        }


        internal bool IsNetworkNoxConnected()
        {
            string output = deviceControl.ExecuteAdbCommand("shell ping -c 1 www.google.com");
            return output.Contains("1 received");
        }

        private void CheckNoxStatus()
        {
            bool isNoxRunning = IsNoxPlayerRunning();
            LogStatus("NOX", isNoxRunning, isNoxRunning ? "Nox Player ist aktiv und läuft." : "Nox Player läuft nicht, wird gestartet.");
            if (!isNoxRunning)
            {
                StartNoxPlayer();
            }
        }

        internal bool IsNoxPlayerRunning() => Process.GetProcessesByName("Nox").Any();

        private void CheckNoxNetworkStatus()
        {
            bool isNetworkNoxConnected = IsNetworkNoxConnected();
            LogStatus("NOX NETWORK", isNetworkNoxConnected, isNetworkNoxConnected ? "Nox Netzwerkverbindung ist aktiv." : "Nox Netzwerkverbindung ist inaktiv, Neustart von Nox.");
            if (!isNetworkNoxConnected)
            {
                RestartNoxPlayer();
            }
        }




        // [ APP ] ###################################################################################
        internal void StartApp()
        {
            deviceControl.ExecuteAdbCommand($"shell monkey -p {deviceControl.packageName} -c android.intent.category.LAUNCHER 1");
            // Warte, bis die App geladen ist
            while (!IsAppLoaded())
            {
                Thread.Sleep(5000); // Überprüfe alle Sekunden
            }
            Thread.Sleep(10000);
        }

        internal void CloseApp()
        {
            deviceControl.ExecuteAdbCommand($"shell am force-stop {deviceControl.packageName}");
            Thread.Sleep(10000);
        }

        internal void RestartApp()
        {
            deviceControl.ExecuteAdbCommand($"shell am force-stop {deviceControl.packageName}");
            Thread.Sleep(10000);
            StartApp();
        }

        internal bool IsAppRunning()
        {
            return !string.IsNullOrEmpty(deviceControl.ExecuteAdbCommand($"shell pidof {deviceControl.packageName}"));
        }

        internal bool IsAppResponsive()
        {
            return !deviceControl.ExecuteAdbCommand("shell dumpsys activity").Contains("ANR");
        }

        internal bool IsAppLoaded()
        {
            // Erste Überprüfung: Ist die App im Vordergrund?
            string focusOutput = deviceControl.ExecuteAdbCommand("shell dumpsys window windows | grep mCurrentFocus");
            bool isAppInFocus = focusOutput.Contains(deviceControl.packageName);

            // Zweite Überprüfung: Sind alle Ladeelemente verschwunden?
            string uiOutput = deviceControl.ExecuteAdbCommand("shell dumpsys window windows | grep -E 'LadeElementId'");
            bool isLoadingElementGone = !uiOutput.Contains("LadeElementId");

            // Nur als geladen betrachten, wenn beide Bedingungen erfüllt sind
            return isAppInFocus && isLoadingElementGone;
        }

        private void CheckAppStatus()
        {
            bool isAppRunning = IsAppRunning();
            LogStatus("APP", isAppRunning, isAppRunning ? "Die Anwendung ist online und funktionsfähig." : "Die Anwendung läuft nicht, wird gestartet.");
            if (!isAppRunning)
            {
                StartApp();
            }

            bool isAppResponsive = IsAppResponsive();
            LogStatus("APP RESPONSE", isAppResponsive, isAppResponsive ? "Die Anwendung reagiert ordnungsgemäß." : "Die Anwendung reagiert nicht, wird neu gestartet.");
            if (!isAppResponsive)
            {
                RestartApp();
            }
        }


    }
}
