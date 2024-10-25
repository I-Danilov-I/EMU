namespace EMU
{
    internal static class Program
    {
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
        
        internal static int timeSleepMin = 1; 

        internal static int offlineErtrege = 0; 
        internal static int infaterieTruppenTraniert = 0;
        internal static int latenztregerTruppenTraniert = 0;
        internal static int sniperTruppenTraniert = 0;
        internal static int lagerBonusGeschenkCounter = 0;
        internal static int lagerBonusAausdauerCounter = 0;

        private static void Main()
        {
            DeviceControl deviceControl = new DeviceControl();
            NoxControl noxControl = new NoxControl();
            GameControl gameControl = new GameControl();

            WriteLogs.LogAndConsoleWirite("\n\n[PROGRAMM START]");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ShowSetting();
            //deviceControl.TrackTouchEvents();
            
            while (true)
            {
                try
                {
                    noxControl.StartNoxPlayer();
                    noxControl.StartADBConnection();
                    if (deviceControl.IsAppRunning() == true)
                    {                     
                        WriteLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]_________________________________");
                        WriteLogs.LogAndConsoleWirite($"Lagerbonus Geschenk: {lagerBonusGeschenkCounter}");
                        WriteLogs.LogAndConsoleWirite($"Lagerbounus Ausdauer: {Program.lagerBonusAausdauerCounter}");
                        WriteLogs.LogAndConsoleWirite($"Infaterie Einheiten traniert: {infaterieTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"Latenzträger Einheiten traniert: {latenztregerTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"Sniper Einheiten traniert: {sniperTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");

                        Erkundung erkundung = new Erkundung();
                        erkundung.ErkundungAbholen();
                        erkundung.Erkundungskampf();
                        
                        GameControl gameControl1 = new GameControl();
                        gameControl.OfflineErtregeAbholen();

                        TruppenTraining truppenTraining = new TruppenTraining();
                        truppenTraining.TrainiereInfaterie(10);
                        truppenTraining.TrainiereLatenzTreger(10);
                        truppenTraining.TrainiereSniper(10);

                        LagerOnlineBelohnung lagerOnlineBelohnung = new LagerOnlineBelohnung();
                        lagerOnlineBelohnung.GeschnekAbholen();
                        lagerOnlineBelohnung.AusdauerAbholen();
                        
                    }
                    else
                    {
                        deviceControl.StartApp();
                        gameControl.OfflineErtregeAbholen();
                    }
                }
                catch (Exception)
                {
                    WriteLogs.LogAndConsoleWirite($"Neustart");
                    deviceControl.StopApp();
                    noxControl.KillNoxPlayerProcess();
                    Thread.Sleep(10000);
                }
            }

        }


    }
}
