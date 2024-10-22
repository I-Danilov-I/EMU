using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EMU
{
    internal static class Info
    {

        // Methode zur Auflistung aller Geräte
        public static void ListAllDevices(string adbPath, string logFilePath)
        {
            try
            {
                string command = "shell getevent -lp"; // Befehl zum Auflisten aller Geräte
                Process process = new Process();
                process.StartInfo.FileName = adbPath;
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                using (StreamWriter writer = new StreamWriter(logFilePath))
                {
                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            writer.WriteLine(args.Data);
                            Console.WriteLine(args.Data);  // Ausgabe der Geräte-Liste auf die Konsole
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                Console.WriteLine($"Geräteliste wurde in {logFilePath} gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Auflisten der Geräte: {ex.Message}");
            }
        }



        // Methode zur Ermittlung der Bildschirmauflösung
        internal static (int, int) GetScreenResolution(string adbPath)
        {
            // Aufruf der Methode ExecuteAdbCommand aus der Klasse AdbUtils
            string resolutionOutput = AdbCommand.ExecuteAdbCommand(adbPath, "shell wm size");
            Regex regex = new Regex(@"Physical size: (\d+)x(\d+)");
            Match match = regex.Match(resolutionOutput);

            if (match.Success)
            {
                int width = int.Parse(match.Groups[1].Value);
                int height = int.Parse(match.Groups[2].Value);
                return (width, height);
            }
            else
            {
                Console.WriteLine("Unable to determine screen resolution.");
                return (1080, 1920); // Standardwerte, falls die Auflösung nicht ermittelt werden kann
            }
        }
    }
}
