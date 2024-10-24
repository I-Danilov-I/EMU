using System.Diagnostics;

namespace EMU
{
    internal static class NoxControl
    {
        internal static void StartNoxPlayer()
        {
            try
            {
                // Überprüfen, ob NoxPlayer bereits läuft
                Process[] processes = Process.GetProcessesByName("Nox"); // Name des Prozesses für NoxPlayer

                if (processes.Length > 0)
                {
                    WriteLogs.LogAndConsoleWirite("NoxPlayer läuft bereits.");
                }
                else
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
                    WriteLogs.LogAndConsoleWirite("NoxPlayer wurde gestartet.");
                    Thread.Sleep(30000); // Warten, bis Nox vollständig gestartet ist
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite("Fehler beim Starten des NoxPlayers: " + ex.Message);
            }
        }


        internal static void StartADBConnection(string adbPath)
        {
            try
            {
                // Überprüfen, ob bereits eine ADB-Verbindung besteht
                string adbDevicesOutput = AdbCommand.ExecuteAdbCommand(adbPath, "devices");

                if (adbDevicesOutput.Contains("127.0.0.1:62001"))
                {
                    WriteLogs.LogAndConsoleWirite("ADB ist bereits mit Nox verbunden.");
                }
                else
                {
                    // ADB Server neu starten und mit Nox verbinden, wenn keine Verbindung besteht
                    AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
                    AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
                    AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001"); // Standard-ADB-Port von Nox
                    WriteLogs.LogAndConsoleWirite("ADB-Verbindung neu hergestellt.");
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite("Fehler bei der ADB-Verbindung: " + ex.Message);
            }
        }

    }
}
