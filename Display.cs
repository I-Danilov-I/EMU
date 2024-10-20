using System.Text.RegularExpressions;

namespace EMU
{
    internal class Display
    {
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
