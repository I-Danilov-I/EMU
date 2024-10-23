
using System.Diagnostics;

namespace EMU.Funtions
{
    internal class DeviceInfo
    {
        public static void TrackTouchEvents(string adbPath, string inputDevice)
        {
            string command = $"shell getevent -lt {inputDevice}"; // Verwende getevent ohne -lp für Live-Daten
            string logFileFolder = Program.logFilePath;
            Console.WriteLine("Starte die Erfassung von Touch-Ereignissen...");

            // Erstelle das Verzeichnis, falls es nicht existiert
            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
                Console.WriteLine($"Verzeichnis '{logFileFolder}' wurde erstellt.");
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
                            Console.WriteLine(args.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                Console.WriteLine($"Touch-Ereignisse wurden in {logFilePath} gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Erfassung der Touch-Ereignisse: {ex.Message}");
            }
        }


        public static void ListRunningApps(string adbPath)
        {
            string command = "shell ps | grep u0_a";
            string logFileFolder = Program.logFilePath;
            Console.WriteLine("Liste der laufenden Apps...");

            // Führe den ADB-Befehl aus und protokolliere ihn
            string output = AdbCommand.ExecuteAdbCommand(adbPath, command);

            // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
                Console.WriteLine($"Verzeichnis '{logFileFolder}' wurde erstellt.");
            }

            // Pfad zur Log-Datei selbst
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
