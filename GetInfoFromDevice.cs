using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EMU
{
    internal static class GetInfoFromDevice
    {

        public static void TrackTouchEvents(string adbPath, string inputDevice)
        {
            try
            {
                string logFileFolder = Program.logFilePath;
                Console.WriteLine("Starte die Erfassung von Touch-Ereignissen...");
                // Verwende getevent mit -lt, um mehr Touch-Ereignisse zu erfassen und detaillierter auszugeben
                string command = $"shell getevent -lt {inputDevice}";
                Process process = new Process();
                process.StartInfo.FileName = adbPath;
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;


                if (!Directory.Exists(logFileFolder))
                {
                    Directory.CreateDirectory(logFileFolder);
                    Console.WriteLine($"Verzeichnis '{logFileFolder}' wurde erstellt.");
                }

                // Pfad zur Log-Datei selbst
                string logFilePath = Path.Combine(logFileFolder, "touchLogs.txt");
                using (StreamWriter writer = new StreamWriter(logFilePath))
                {
                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            // ADB-Befehl ausführen
                            if (args.Data.Contains("ABS_MT_POSITION_X") || args.Data.Contains("ABS_MT_POSITION_Y"))
                            {
                                writer.WriteLine(args.Data);
                                Console.WriteLine(args.Data);
                            }

                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Überwachung der Touch-Ereignisse: {ex.Message}");
            }
        }


        public static void ListRunningApps(string adbPath, string logFileFolder)
        {
            try
            {
                // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
                if (!Directory.Exists(logFileFolder))
                {
                    Directory.CreateDirectory(logFileFolder);
                    Console.WriteLine($"Verzeichnis '{logFileFolder}' wurde erstellt.");
                }

                // Pfad zur Log-Datei selbst
                string logFilePath = Path.Combine(logFileFolder, "runningAppsLogs.txt");

                // ADB-Befehl, um alle laufenden Prozesse zu erfassen
                string adbCommand = "shell ps | grep u0_a";
                string output = AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);

                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
                {
                    if (!string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine("Liste der laufenden Apps:");
                        writer.WriteLine("Liste der laufenden Apps:");

                        // Ausgabe formatieren, um nur die relevanten Prozessnamen und Paketnamen anzuzeigen
                        var lines = output.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in lines)
                        {
                            if (line.Contains("u0_a"))
                            {
                                // Extrahiere den Paketnamen
                                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                string packageName = parts[parts.Length - 1]; // Der letzte Teil ist normalerweise der Paketname

                                // Ausgabe auf der Konsole und Schreiben in die Log-Datei
                                Console.WriteLine($"App: {packageName}");
                                writer.WriteLine($"App: {packageName}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Es laufen derzeit keine Apps.");
                        writer.WriteLine("Es laufen derzeit keine Apps.");
                    }
                }

                Console.WriteLine($"Liste der laufenden Apps wurde in {logFilePath} gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der laufenden Apps: {ex.Message}");
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
                Console.WriteLine($"Auflösung: {width} x {height}");
                return (width, height);
            }
            else
            {
                Console.WriteLine("Unable to determine screen resolution.");
                return (1080, 1920); // Standardwerte, falls die Auflösung nicht ermittelt werden kann
            }
        }


        public static void ListAllDevices(string adbPath, string logFileFolder)
        {
            try
            {
                Console.WriteLine("Liste aller Eingabegeräte:");

                // Überprüfen, ob das Verzeichnis für die Log-Datei existiert, und ggf. erstellen
                if (!Directory.Exists(logFileFolder))
                {
                    Directory.CreateDirectory(logFileFolder);
                    Console.WriteLine($"Verzeichnis '{logFileFolder}' wurde erstellt.");
                }

                // Pfad zur Log-Datei selbst
                string logFilePath = Path.Combine(logFileFolder, "deviceLogs.txt");

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


    }
}
