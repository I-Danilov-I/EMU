namespace EMU
{
    internal class WriteLogs
    {
        private string logFileFolderPath = Program.logFileFolderPath;

        internal void LogAndConsoleWirite(string inputString)
        {
            try
            {
                // Überprüfen, ob das Verzeichnis existiert
                if (!Directory.Exists(logFileFolderPath))
                {
                    Directory.CreateDirectory(logFileFolderPath);
                }

                // Ändere den Namen der lokalen Variablen, um Konflikte zu vermeiden
                string logFilePath = Path.Combine(logFileFolderPath, "Logs.txt");

                // Falls die Datei nicht existiert, erstelle sie
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close();
                }

                // Schreiben in die Datei und Konsole
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // 'true' bedeutet, dass an die Datei angehängt wird
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
                Console.WriteLine($"Ein Fehler ist beim Schreiben in die Logdatei aufgetreten: {ex.Message}");
            }
        }
    }
}
