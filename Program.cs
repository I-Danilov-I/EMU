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
            WriteLogs writeLogs = new WriteLogs();
            DeviceControl deviceControl = new DeviceControl(writeLogs);
            Erkundung erkundung = new Erkundung(writeLogs, deviceControl);
            TruppenTraining truppenTraining = new TruppenTraining(writeLogs, deviceControl);
            LagerOnlineBelohnung lagerOnlineBelohnung = new LagerOnlineBelohnung(writeLogs, deviceControl);


            writeLogs.LogAndConsoleWirite("\n[PROGRAMM START]");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ShowSetting();
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            // deviceControl.TrackTouchEvents();


            while (true)
            {
                try
                {
                    deviceControl.StartNoxPlayer();
                    deviceControl.StartADBConnection();
                    if (deviceControl.IsAppRunning() == true)
                    {                     
                        writeLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]_________________________________");
                        writeLogs.LogAndConsoleWirite($"Lagerbonus Geschenk: {lagerBonusGeschenkCounter}");
                        writeLogs.LogAndConsoleWirite($"Lagerbounus Ausdauer: {Program.lagerBonusAausdauerCounter}");
                        writeLogs.LogAndConsoleWirite($"Infaterie Einheiten traniert: {infaterieTruppenTraniert}");
                        writeLogs.LogAndConsoleWirite($"Latenzträger Einheiten traniert: {latenztregerTruppenTraniert}");
                        writeLogs.LogAndConsoleWirite($"Sniper Einheiten traniert: {sniperTruppenTraniert}");
                        writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                        erkundung.Erkundungskampf();

                        deviceControl.OfflineErtregeAbholen();

                        truppenTraining.TrainiereInfaterie(10);
                        deviceControl.Wiederverbinden();

                        truppenTraining.TrainiereLatenzTreger(10);
                        deviceControl.Wiederverbinden();

                        truppenTraining.TrainiereSniper(10);
                        deviceControl.Wiederverbinden();

                        erkundung.ErkundungAbholen();
                        deviceControl.Wiederverbinden();

                        lagerOnlineBelohnung.GeschnekAbholen();
                        deviceControl.Wiederverbinden();

                        lagerOnlineBelohnung.AusdauerAbholen();
                        deviceControl.Wiederverbinden();



                    }
                    else
                    {
                        deviceControl.StartApp();
                    }
                }
                catch (Exception)
                {
                    deviceControl.KillNoxPlayerProcess();
                    Thread.Sleep(10000);
                }
            }
        }


    }
}
