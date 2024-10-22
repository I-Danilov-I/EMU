using System.Diagnostics;
using System.IO;

namespace EMU
{
    internal class AdbCommand
    {
        // Methode zur Ausführung von ADB-Befehlen und Protokollierung in einer Log-Datei
        public static string ExecuteAdbCommand(string adbPath, string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = adbPath; // ADB-Pfad verwenden
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true; // Fehlerausgabe auch protokollieren
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                Console.WriteLine($"Starting ADB Command: {command}");

                process.Start();

                // Ergebnis auslesen
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

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

                process.WaitForExit();

                Console.WriteLine($"Command exit code: {process.ExitCode}");

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while executing the ADB command: {ex.Message}");
                return "";
            }
        }

    }
}
