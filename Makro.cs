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

    }
}
