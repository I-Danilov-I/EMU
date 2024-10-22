using System.Diagnostics;

namespace EMU
{
    internal static class Makro
    {

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


        public static void PlayMacro(string adbPath,string macroFilePath)
        {
            if (File.Exists(macroFilePath))
            {
                string[] lines = File.ReadAllLines(macroFilePath);
                string xValue = null;
                string yValue = null;

                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("ABS_MT_POSITION_X"))
                        {
                            // Extrahiere den Hex-Wert für X
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            xValue = parts[parts.Length - 1]; // Letzter Teil enthält den Hex-Wert
                        }
                        else if (line.Contains("ABS_MT_POSITION_Y"))
                        {
                            // Extrahiere den Hex-Wert für Y
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            yValue = parts[parts.Length - 1]; // Letzter Teil enthält den Hex-Wert
                        }

                        // Wenn sowohl X als auch Y gefunden wurden, führe den Klick aus
                        if (xValue != null && yValue != null)
                        {
                            Console.WriteLine($"Klick bei X: {xValue}, Y: {yValue}");
                            ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, xValue, yValue);

                            // Zurücksetzen, um den nächsten Klick zu erfassen
                            xValue = null;
                            yValue = null;
                        }
                    }
                }
                Console.WriteLine("Makro-Wiedergabe abgeschlossen.");
            }
            else
            {
                Console.WriteLine("Keine Makro-Datei für die Wiedergabe gefunden.");
            }
        }

    }
}
