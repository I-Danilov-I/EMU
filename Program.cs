using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Pfad zur ADB von Nox Player festlegen
        string adbPath = @"C:\Program Files (x86)\Nox\bin\adb.exe";

        // ADB-Server neu starten und mit dem Nox Player verbinden
        ExecuteAdbCommand(adbPath, "kill-server");
        ExecuteAdbCommand(adbPath, "start-server");
        ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");

        // Bildschirmauflösung abfragen
        (int screenWidth, int screenHeight) = GetScreenResolution(adbPath);
        Console.WriteLine($"Screen resolution: {screenWidth}x{screenHeight}");

        // Liste von relativen Koordinaten für die Klicks (z.B. 50% der Breite und 50% der Höhe)
        double[,] relativeCoordinates = new double[,]
        {
            {0.5, 0.5},  // Mitte des Bildschirms
            {0.3, 0.4},  // Links oben
            {0.7, 0.8},  // Rechts unten
            {0.4, 0.6},  // Mittig leicht versetzt
            {0.6, 0.3}   // Oben rechts
        };

        // ADB-Befehl für jeden Satz von Koordinaten ausführen
        for (int i = 0; i < relativeCoordinates.GetLength(0); i++)
        {
            // Berechne absolute Koordinaten basierend auf relativen Werten und Bildschirmauflösung
            int x = (int)(relativeCoordinates[i, 0] * screenWidth);
            int y = (int)(relativeCoordinates[i, 1] * screenHeight);

            // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
            string adbCommand = $"shell input tap {x} {y}";

            Console.WriteLine($"ADB Command to be executed: {adbPath} {adbCommand}");

            // ADB-Befehl ausführen
            ExecuteAdbCommand(adbPath, adbCommand);

            // Verzögerung von 500 Millisekunden zwischen den Klicks
            Thread.Sleep(500);
        }
    }

    // Methode zur Ermittlung der Bildschirmauflösung
    static (int, int) GetScreenResolution(string adbPath)
    {
        string resolutionOutput = ExecuteAdbCommand(adbPath, "shell wm size");
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

    // Methode zur Ausführung von ADB-Befehlen
    static string ExecuteAdbCommand(string adbPath, string command)
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

            Console.WriteLine($"Starting ADB Command Execution: {adbPath} {command}");

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

            Console.WriteLine($"Command executed with exit code: {process.ExitCode}");

            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while executing the ADB command: {ex.Message}");
            return "";
        }
    }
}
