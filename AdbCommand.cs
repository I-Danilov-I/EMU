using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace EMU
{
    internal static class AdbCommand
    {
        internal static string ExecuteAdbCommand(string adbPath, string command)
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

                // Log-Schreiben in eine separate Methode ausgelagert
                WriteLogs.Log(command, output, errorOutput);

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
