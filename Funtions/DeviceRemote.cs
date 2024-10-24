using System.Diagnostics;

namespace EMU
{
    internal class DeviceRemote
    {

        internal static void Wiederverbinden(string adbPath, string screenshotDirectory, string packageName,  int timeSleepMin)
        {
            // Wiederverbinden wenn von anderem Gerät beigetreten.
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool OnOff = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Tipps", "Dieses Konto");
            if (OnOff == true)
            {
                WriteLogs.LogAndConsoleWirite($"Ein anderes Gerät ist gereade Aktiv. Ich warte {timeSleepMin} Min...");
                // App beenden
                string forceStopCommand = $"shell am force-stop {packageName}";
                AdbCommand.ExecuteAdbCommand(adbPath, forceStopCommand);
                WriteLogs.LogAndConsoleWirite($"{packageName} wurde beendet.");

                // Warten
                //Thread.Sleep(60 * 1000 * timeSleepMin);

                // Ausnahme werfen
                throw new Exception("Ein anderes Gerät hat sich verbunden. Programm wird beendet.");             
            }
        }


        public static bool IsAppRunning(string adbPath, string packageName)
        {
            // Befehl, um zu überprüfen, ob die App läuft
            string adbCommand = $"shell pidof {packageName}";
            string result = AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
            return !string.IsNullOrEmpty(result); // Wenn ein Ergebnis vorliegt, läuft die App
        }


        public static void StartApp(string adbPath, string packageName)
        {
            if(IsAppRunning(adbPath, packageName) == true)
            {
                return;
            }
            string adbCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
            WriteLogs.LogAndConsoleWirite($"App {packageName} wird gestartet.");
        }


        public static void DrueckeZurueckTaste(string adbPath)
        {
            string adbCommand = "shell input keyevent 4";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
        }


        public static void ClickAndHoldAndScroll(string adbPath, string startXHex, string startYHex, string endXHex, string endYHex, int holdDuration, int scrollDuration)
        {
            Wiederverbinden(Program.adbPath, Program.screenshotDirectory, Program.packeName, Program.timeSleepMin);
            // Hex-Werte in Dezimalwerte umwandeln
            int startX = int.Parse(startXHex, System.Globalization.NumberStyles.HexNumber);
            int startY = int.Parse(startYHex, System.Globalization.NumberStyles.HexNumber);
            int endX = int.Parse(endXHex, System.Globalization.NumberStyles.HexNumber);
            int endY = int.Parse(endYHex, System.Globalization.NumberStyles.HexNumber);

            // Schritt 1: Klicken und Halten (Finger auf dem Bildschirm gedrückt halten)
            string adbCommandHold = $"shell input swipe {startX} {startY} {startX} {startY} {holdDuration}";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommandHold);

            // Schritt 2: Ziehen (Finger auf dem Bildschirm bewegen)
            string adbCommandScroll = $"shell input swipe {startX} {startY} {endX} {endY} {scrollDuration}";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommandScroll);
        }


        public static void ClickAtTouchPositionWithHexa(string adbPath, string hexX, string hexY)
        {
            Wiederverbinden(Program.adbPath, Program.screenshotDirectory, Program.packeName, Program.timeSleepMin);
            int x = int.Parse(hexX, System.Globalization.NumberStyles.HexNumber);
            int y = int.Parse(hexY, System.Globalization.NumberStyles.HexNumber);

            string adbCommand = $"shell input tap {x} {y}";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);

        }


        public static void TrackTouchEvents(string adbPath, string inputDevice)
        {
            string command = $"shell getevent -lt {inputDevice}"; // Verwende getevent ohne -lp für Live-Daten
            string logFileFolder = Program.logFilePath;
            WriteLogs.LogAndConsoleWirite("Starte die Erfassung von Touch-Ereignissen...");

            // Erstelle das Verzeichnis, falls es nicht existiert
            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
            }

            // Pfad zur Log-Datei selbst
            string logFilePath = Path.Combine(logFileFolder, "touchEventsLog.txt");

            // Führe den ADB-Befehl aus und speichere die Ausgabe in einer Datei
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
                {
                    Process process = new Process();
                    process.StartInfo.FileName = adbPath;
                    process.StartInfo.Arguments = command;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            // Schreibe die Touch-Ereignisse in die Logdatei und auf die Konsole
                            writer.WriteLine(args.Data);
                            WriteLogs.LogAndConsoleWirite(args.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                WriteLogs.LogAndConsoleWirite($"Touch-Ereignisse wurden in {logFilePath} gespeichert.");
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite($"Fehler bei der Erfassung der Touch-Ereignisse: {ex.Message}");
            }
        }


        public static void ListRunningApps(string adbPath)
        {
            string command = "shell ps | grep u0_a";
            string logFileFolder = Program.logFilePath;
            WriteLogs.LogAndConsoleWirite("Liste der laufenden Apps...");
            string output = AdbCommand.ExecuteAdbCommand(adbPath, command);

            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
            }

            string logFilePath = Path.Combine(logFileFolder, "runningAppsLogs.txt");

            // Schreibe die Ausgabe in die Log-Datei
            using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
            {
                if (!string.IsNullOrEmpty(output))
                {
                    writer.WriteLine(output);
                }
                else
                {
                    writer.WriteLine("Es laufen derzeit keine Apps.");
                }
            }
        }


        //############################################################################
        internal static void ClickAtPositionWithDecimal(string adbPath, int x, int y)
        {
            string adbCommand = $"shell input tap {x} {y}";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
            DeviceRemote.Wiederverbinden(adbPath, Program.screenshotDirectory, Program.packeName, Program.timeSleepMin);
        }

        // Methode zum Durchklicken eines quadratischen Bereichs um die Mitte des Bildschirms
        internal static void ClickInQuadraticArea(string adbPath, int width, int height, int offset, int step)
        {
            // Berechne die Mitte des Bildschirms
            int centerX = width / 2;
            int centerY = height / 2;

            // Berechne die Grenzen des quadratischen Bereichs
            int leftX = centerX - offset;   // Links von der Mitte
            int rightX = centerX + offset;  // Rechts von der Mitte
            int topY = centerY - offset;    // Oben von der Mitte
            int bottomY = centerY + offset; // Unten von der Mitte

            // Schleifen, um innerhalb des quadratischen Bereichs zu klicken
            for (int x = leftX; x <= rightX; x += step)  // Schleife über die X-Koordinate
            {
                for (int y = topY; y <= bottomY; y += step) // Schleife über die Y-Koordinate
                {
                    ClickAt(adbPath, x, y);
                    Thread.Sleep(100);
                }
            }
        }

        // Hilfsmethode, um an einer bestimmten Position zu klicken
        private static void ClickAt(string adbPath, int x, int y)
        {
            // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
            string adbCommand = $"shell input tap {x} {y}";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
        }


    }
}
