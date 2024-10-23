namespace EMU
{
    internal class WriteLogs
    {

        internal static void LogAndConsoleWirite(string inputString)
        {
            try
            {
                // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
                if (!Directory.Exists(Program.logFilePath))
                {
                    Directory.CreateDirectory(Program.logFilePath);
                }

                // Pfad zur Log-Datei selbst
                string logFilePath = Path.Combine(Program.logFilePath, "Logs.txt");

                // Überprüfen, ob die Log-Datei existiert, und ggf. erstellen
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close(); 
                }

                // In die Log-Datei schreiben
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // 'true' hängt an die Datei an
                {
                    if (!string.IsNullOrEmpty(inputString))
                    {
                        Console.WriteLine($"{inputString}");
                        writer.WriteLine($"{inputString}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
            }
        }


    }
}
