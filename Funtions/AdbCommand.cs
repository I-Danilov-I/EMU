using System.Diagnostics;

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

                process.Start();

                // Ergebnis auslesen
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

                process.WaitForExit();
                return output;
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite(ex.Message);
                return "";
            }
        }


    }
}
