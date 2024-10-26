using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

namespace EMU
{
    internal class StableControl
    {
        private readonly WriteLogs writeLogs;
        private readonly PrintInfo printInfo;
        private readonly DeviceControl deviceControl;

        public StableControl(WriteLogs writeLogs, PrintInfo printInfo, DeviceControl deviceControl)
        {
            this.writeLogs = writeLogs;
            this.printInfo = printInfo;
            this.deviceControl = deviceControl;
        }

        internal void Control()
        {
            writeLogs.LogAndConsoleWirite("[Stabilitätskontrolle]");
            writeLogs.LogAndConsoleWirite("----------------------------------------------------------------");

            CheckNetworkStatus();
            CheckNoxStatus();
            CheckADBStatus();
            CheckNoxNetworkStatus();
            CheckAppStatus();
            CheckAccountUsage();

            writeLogs.LogAndConsoleWirite("----------------------------------------------------------------");
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
            writeLogs.LogAndConsoleWirite(logMessage);
        }

        private void CheckNetworkStatus()
        {
            bool isNetworkAvailable = IsNetworkAvailable();
            LogStatus("NETWORK", isNetworkAvailable, isNetworkAvailable ? "Das VM-Netzwerk ist online und funktionsfähig." : "Das VM-Netzwerk ist offline, Verbindungsprüfung wird gestartet.");
            if (!isNetworkAvailable)
            {
                HandleNetworkError();
            }
        }

        private void HandleNetworkError()
        {
            LogStatus("ERROR", false, $"Das VM-Netzwerk ist offline. Es wird ein erneuter Verbindungsversuch in {Program.reconnectSleepTime} Minuten gestartet.");
            Thread.Sleep(TimeSpan.FromMinutes(Program.reconnectSleepTime));
            throw new Exception("Netzwerk nicht verfügbar.");
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

        private void CheckADBStatus()
        {
            bool isADBConnected = IsADBConnected();
            LogStatus("ADB", isADBConnected, isADBConnected ? "ADB Verbindung ist etabliert." : "ADB Verbindung ist nicht etabliert, Verbindung wird hergestellt.");
            if (!isADBConnected)
            {
                StartADBConnection();
            }
        }

        private void CheckNoxNetworkStatus()
        {
            bool isNetworkNoxConnected = IsNetworkNoxConnected();
            LogStatus("NOX NETWORK", isNetworkNoxConnected, isNetworkNoxConnected ? "Nox Netzwerkverbindung ist aktiv." : "Nox Netzwerkverbindung ist inaktiv, Neustart von Nox.");
            if (!isNetworkNoxConnected)
            {
                RestartNoxPlayer();
            }
        }

        private void RestartNoxPlayer()
        {
            KillNoxPlayerProcess(); // Stoppt alle laufenden Nox-Prozesse
            StartNoxPlayer(); // Startet den Nox Player neu
            LogStatus("NOX", true, "Nox Player wird neu gestartet.");
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

        private void CheckAccountUsage()
        {
            deviceControl.TakeScreenshot();
            bool isAccountInUse = deviceControl.CheckTextInScreenshot("Tipps", "Konto");
            LogStatus("ACCOUNT", isAccountInUse, isAccountInUse ? $"Konto wird verwendet, erneuter Versuch in {Program.reconnectSleepTime} Minuten." : "Konto ist frei und nicht in Verwendung.");
            if (isAccountInUse)
            {
                CloseApp();
                Thread.Sleep(TimeSpan.FromMinutes(Program.reconnectSleepTime));
                StartApp();
                throw new Exception("Konto derzeit in Verwendung.");
            }
        }

        // Helper methods to perform various checks and actions
        internal bool IsNetworkAvailable() => NetworkInterface.GetIsNetworkAvailable();

        internal bool IsNoxPlayerRunning() => Process.GetProcessesByName("Nox").Any();

        internal void StartNoxPlayer()
        {
            Process.Start(@"C:\Program Files\Nox\bin\Nox.exe", "-clone:Nox_0");
        }

        internal bool IsADBConnected()
        {
            string output = deviceControl.ExecuteAdbCommand("devices");
            return output.Contains("device");
        }

        internal bool IsNetworkNoxConnected()
        {
            string output = deviceControl.ExecuteAdbCommand("shell ping -c 1 www.google.com");
            return output.Contains("1 received");
        }

        internal void StartADBConnection()
        {
            deviceControl.ExecuteAdbCommand("start-server");
        }

        internal bool IsAppRunning()
        {
            return !string.IsNullOrEmpty(deviceControl.ExecuteAdbCommand($"shell pidof {deviceControl.packageName}"));
        }

        internal bool IsAppResponsive()
        {
            return !deviceControl.ExecuteAdbCommand("shell dumpsys activity").Contains("ANR");
        }

        internal void StartApp()
        {
            deviceControl.ExecuteAdbCommand($"shell monkey -p {deviceControl.packageName} -c android.intent.category.LAUNCHER 1");
        }

        internal void RestartApp()
        {
            deviceControl.ExecuteAdbCommand($"shell am force-stop {deviceControl.packageName}");
            StartApp();
        }

        internal void CloseApp()
        {
            deviceControl.ExecuteAdbCommand($"shell am force-stop {deviceControl.packageName}");
        }

        internal void KillNoxPlayerProcess()
        {
            foreach (var process in Process.GetProcessesByName("Nox"))
            {
                process.Kill();
            }
        }
    }
}
