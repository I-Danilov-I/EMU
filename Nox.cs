using System.Diagnostics;

namespace EMU
{
    internal static class NoxControl
    {
        internal static void StartNoxPlayer()
        {
            try
            {
                string noxPath = @"C:\Program Files\Nox\bin\Nox.exe"; // Pfad zu Nox
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = noxPath,
                    Arguments = "-clone:Nox_0", // Verwende dies, um eine spezifische Instanz zu starten (z.B. Nox_0)
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(startInfo);
                Console.WriteLine("NoxPlayer wurde gestartet.");
                Thread.Sleep(30000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Starten des NoxPlayers: " + ex.Message);
            }
        }

        internal static void StartADBConnection(string adbPath)
        {
            try
            {
                // ADB Server neu starten und mit Nox verbinden
                AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
                AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
                AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001"); // Standard-ADB-Port von Nox
                Console.WriteLine("ADB-Verbindung hergestellt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler bei der ADB-Verbindung: " + ex.Message);
            }
        }
    }
}
