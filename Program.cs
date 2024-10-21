using System.Diagnostics;

namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\TouchLogs.txt";
        internal static string inputDevice = "/dev/input/event4"; // Ändere dies entsprechend deinem Gerät


        private static void Main()
        {
            // ADB-Server neu starten und mit dem Nox Player verbinden
            AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");
            var (width, height) = Display.GetScreenResolution(adbPath); // Bildschirmauflösung abfragen



            ClicksOperate.ClickAtPositionWithDecimal(adbPath, 420, 420);


            // Zeige die Liste aller Geräte an
            Console.WriteLine("Liste aller Eingabegeräte:");
            ListAllDevices(adbPath, logFilePath);

            // Erfassung der Touch-Positionen starten
            Console.WriteLine("Starte die Erfassung von Touch-Ereignissen...");
            TrackTouchEvents(adbPath, inputDevice, logFilePath);


            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            Checker.CheckTextInScreenshot(screenshotDirectory);
        }


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


        public static void TrackTouchEvents(string adbPath, string inputDevice, string logFilePath)
        {
            try
            {
                // Verwende getevent mit -lt, um mehr Touch-Ereignisse zu erfassen und detaillierter auszugeben
                string command = $"shell getevent -lt {inputDevice}";
                Process process = new Process();
                process.StartInfo.FileName = adbPath;
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                Console.WriteLine($"Überwache Touch-Ereignisse auf Gerät {inputDevice}...");

                using (StreamWriter writer = new StreamWriter(logFilePath))
                {
                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            // Schreibe die Touch-Ereignisse in die Log-Datei und Konsole
                            writer.WriteLine(args.Data);
                            Console.WriteLine(args.Data);
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



    }
}
