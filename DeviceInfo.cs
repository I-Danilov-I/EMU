
namespace EMU
{
    internal class DeviceInfo
    {
        // Beispiel, wie du jetzt TrackTouchEvents verwenden kannst
        public static void TrackTouchEvents(string adbPath, string inputDevice)
        {
            string command = $"shell getevent -lt {inputDevice}";
            string logFileFolder = Program.logFilePath;
            Console.WriteLine("Starte die Erfassung von Touch-Ereignissen...");

            // Führe den ADB-Befehl aus und protokolliere ihn
            string output = AdbCommand.ExecuteAdbCommand(adbPath, command);

            // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
                Console.WriteLine($"Verzeichnis '{logFileFolder}' wurde erstellt.");
            }

            // Pfad zur Log-Datei selbst
            string logFilePath = Path.Combine(logFileFolder, "touchLogs.txt");

            // Schreibe die Ausgabe in die Log-Datei
            using (StreamWriter writer = new StreamWriter(logFilePath))
            {
                writer.WriteLine(output);
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
