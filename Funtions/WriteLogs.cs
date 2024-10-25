namespace EMU
{
    internal class WriteLogs : DeviceControl
    {

        internal void LogAndConsoleWirite(string inputString)
        {
            try
            {
                if (!Directory.Exists(Get_logFilerFolderPath()))
                {
                    Directory.CreateDirectory(Get_logFilerFolderPath());
                }

                string logFileFolderPath = Path.Combine(Get_logFilerFolderPath(), "Logs.txt");
                if (!File.Exists(logFileFolderPath))
                {
                    File.Create(logFileFolderPath).Close(); 
                }

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
