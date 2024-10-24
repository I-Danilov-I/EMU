namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string inputDevice = "/dev/input/event4";
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
        internal static string packeName = "com.gof.global"; // Warte für erneuten Start fall anderes Gerät aktiv ist.
        internal static int timeSleepMin = 1; // Warte für erneuten Start fall anderes Gerät aktiv ist.

        internal static int offlineErtrege = 0; 

        internal static int infaterieTruppenTraniert = 0;
        internal static int latenztregerTruppenTraniert = 0;
        internal static int sniperTruppenTraniert = 0;

        internal static int lagerBonusGeschenkCounter = 0;
        internal static int lagerBonusAausdauerCounter = 0;

        private static void Main()
        {
            WriteLogs.LogAndConsoleWirite("\n\n[PROGRAMM START]");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            //DeviceControl.TrackTouchEvents(adbPath, inputDevice);
            
            while (true)
            {
                try
                {
                    NoxControl.StartNoxPlayer();
                    NoxControl.StartADBConnection(adbPath);
                    if (DeviceControl.IsAppRunning(adbPath, packeName) == true)
                    {                     
                        WriteLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]_________________________________");
                        WriteLogs.LogAndConsoleWirite($"Lagerbonus Geschenk: {lagerBonusGeschenkCounter}");
                        WriteLogs.LogAndConsoleWirite($"Lagerbounus Ausdauer: {Program.lagerBonusAausdauerCounter}");
                        WriteLogs.LogAndConsoleWirite($"Infaterie Einheiten traniert: {infaterieTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"Latenzträger Einheiten traniert: {latenztregerTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"Sniper Einheiten traniert: {sniperTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");

                        //GameControl.OfflineErtregeAbholen(adbPath, screenshotDirectory);
                        TruppenTraining.TrainiereSniper(adbPath, screenshotDirectory, 9);
                        TruppenTraining.TrainiereLatenzTreger(adbPath, screenshotDirectory, 7);
                        TruppenTraining.TrainiereInfaterie(adbPath, screenshotDirectory, 3);
                        LagerOnlineBelohnung.GeschnekAbholen(adbPath, screenshotDirectory);
                        LagerOnlineBelohnung.AusdauerAbholen(adbPath, screenshotDirectory);
                    }
                    else
                    {
                        DeviceControl.StartApp(adbPath, packeName);
                        GameControl.OfflineErtregeAbholen(adbPath, screenshotDirectory);
                    }
                }
                catch (Exception)
                {
                    WriteLogs.LogAndConsoleWirite($"Neustart");
                    DeviceControl.StopApp(adbPath, packeName);
                    NoxControl.KillNoxPlayerProcess();
                    Thread.Sleep(10000);
                }
            }

        }

        internal static void Wiederverbinden(int timeSleep)
        {
            // Prüfe um Training erfoglreich gestartet wurde.
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool erfolg = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Kontankt", "Konto"); // Suche nach Text im Screenshot
            if (erfolg == true)
            {
                WriteLogs.LogAndConsoleWirite($"Akaunt wird von einem anderem Gerät verwendet. Verscuhe in {timeSleep} Min erneut.");
                DeviceControl.StopApp(adbPath, packeName);
                NoxControl.KillNoxPlayerProcess();
                Thread.Sleep(60 * 1000 * timeSleep);
            }
        }
    }
}
