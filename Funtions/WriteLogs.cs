namespace EMU
{
    internal class WriteLogs : DeviceControl
    {

        internal void LogAndConsoleWirite(string inputString)
        {
            try
            {
                // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
                if (!Directory.Exists(Get_logFilerFolderPath()))
                {
                    Directory.CreateDirectory(Get_logFilerFolderPath());
                }

                // Pfad zur Log-Datei selbst
                string logFileFolderPath = Path.Combine(Get_logFilerFolderPath(), "Logs.txt");

                // Überprüfen, ob die Log-Datei existiert, und ggf. erstellen
                if (!File.Exists(logFileFolderPath))
                {
                    File.Create(logFileFolderPath).Close(); 
                }

                // In die Log-Datei schreiben
                using (StreamWriter writer = new StreamWriter(logFileFolderPath, true)) // 'true' hängt an die Datei an
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
