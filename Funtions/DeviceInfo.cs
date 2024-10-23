
using System.Diagnostics;

namespace EMU
{
    internal class DeviceInfo
    {
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
    }
}
