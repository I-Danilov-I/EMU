using System.Diagnostics;

namespace EMU
{
    internal class AdbCommand
    {
        // Methode zur Ausführung von ADB-Befehlen
        public static string ExecuteAdbCommand(string adbPath, string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = adbPath; // ADB-Client von Nox verwenden
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true; // Fehlerausgabe hinzufügen
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                Console.WriteLine($"\nStarting ADB Command: {command}");

                process.Start();

                // Ausgabe des Ergebnisses (Standardausgabe)
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(errorOutput))
                {
                    Console.WriteLine("Error Output:");
                    Console.WriteLine(errorOutput);
                }

                process.WaitForExit();

                Console.WriteLine($"Command exit code: [{process.ExitCode}]");

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
