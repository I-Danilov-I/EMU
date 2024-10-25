namespace EMU
{
    internal static class Program
    {  
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
            WriteLogs writeLogs = new WriteLogs();

            writeLogs.LogAndConsoleWirite("\n\n[PROGRAMM START]");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
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
                        writeLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]_________________________________");
                        writeLogs.LogAndConsoleWirite($"Lagerbonus Geschenk: {lagerBonusGeschenkCounter}");
                        writeLogs.LogAndConsoleWirite($"Lagerbounus Ausdauer: {Program.lagerBonusAausdauerCounter}");
                        writeLogs.LogAndConsoleWirite($"Infaterie Einheiten traniert: {infaterieTruppenTraniert}");
                        writeLogs.LogAndConsoleWirite($"Latenzträger Einheiten traniert: {latenztregerTruppenTraniert}");
                        writeLogs.LogAndConsoleWirite($"Sniper Einheiten traniert: {sniperTruppenTraniert}");
                        writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");

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
                    noxControl.KillNoxPlayerProcess();
                    Thread.Sleep(10000);
                }
            }

        }


    }
}
