namespace EMU
{
    internal class WriteLogs
    {
        // Separate Methode für das Schreiben in die Log-Datei
        internal static void Log(string command, string output, string errorOutput)
        {
            try
            {
                // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
                if (!Directory.Exists(Program.logFilePath))
                {
                    Directory.CreateDirectory(Program.logFilePath);
                    Console.WriteLine($"Verzeichnis '{Program.logFilePath}' wurde erstellt.");
                }

                // Pfad zur Log-Datei selbst
                string logFilePath = Path.Combine(Program.logFilePath, "adbCommandLogs.txt");

                // Überprüfen, ob die Log-Datei existiert, und ggf. erstellen
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close(); // Erstellen der Datei und sofortiges Schließen, um sie zum Schreiben zu öffnen
                    Console.WriteLine($"Log-Datei '{logFilePath}' wurde erstellt.");
                }

                // In die Log-Datei schreiben
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // 'true' hängt an die Datei an
                {
                    writer.WriteLine($"ADB Command: {command}");
                    writer.WriteLine("Output:");
                    writer.WriteLine(output);

                    if (!string.IsNullOrEmpty(errorOutput))
                    {
                        writer.WriteLine("Error Output:");
                        writer.WriteLine(errorOutput);
                    }
                    writer.WriteLine("------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
            }
        }
    }
}
