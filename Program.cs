using EMU.Automations;

namespace EMU
{
    internal static class Program
    {
        internal static int geschwindigkeit = 4000; // Pause in ms zwischen jedem Befehl.

        internal static int offlineErtregeCounter = 0; 

        internal static int lagerBonusGeschenkCounter = 0;
        internal static int lagerBonusAausdauerCounter = 0;

        internal static int infaterieTruppenTraniertCounter = 0;
        internal static int latenztregerTruppenTraniertCounter = 0;
        internal static int sniperTruppenTraniertCounter = 0;

        internal static int erkungBonusCounter = 0;
        internal static int erkundungKampfCounter = 0;

        internal static int alianzHilfeCounter = 0;
        internal static int alianzKistenCounter = 0;
        internal static int alianzTechnologie = 0;

        internal static int heilenCounter = 0;
        internal static int heldenRecurt = 0;

        private static void Main()
        {
            WriteLogs writeLogs = new WriteLogs();
            DeviceControl deviceControl = new DeviceControl(writeLogs);
            
            Erkundung erkundung = new Erkundung(writeLogs, deviceControl);
            TruppenTraining truppenTraining = new TruppenTraining(writeLogs, deviceControl);
            LagerOnlineBelohnung lagerOnlineBelohnung = new LagerOnlineBelohnung(writeLogs, deviceControl);
            Allianz allianz = new Allianz(writeLogs, deviceControl);
            Jagt bestienJagt = new Jagt(writeLogs, deviceControl);
            TruppenHeilen truppenHeilen = new TruppenHeilen(writeLogs, deviceControl);
            Helden helden = new Helden(writeLogs, deviceControl);

            writeLogs.LogAndConsoleWirite("\n[PROGRAMM START]");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ShowSetting();
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            //deviceControl.TrackTouchEvents();
            

            while (true)
            {
                try
                {
                    deviceControl.StartNoxPlayer();
                    deviceControl.StartADBConnection();
                    deviceControl.StartApp();
                    if (deviceControl.IsAppRunning() == true)
                    {                     
                        writeLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]_________________________________");
                        writeLogs.LogAndConsoleWirite($"Lagerbonus Geschenk: {lagerBonusGeschenkCounter}");
                        writeLogs.LogAndConsoleWirite($"Lagerbounus Ausdauer: {Program.lagerBonusAausdauerCounter}");

                        writeLogs.LogAndConsoleWirite($"Infaterie Einheiten traniert: {infaterieTruppenTraniertCounter}");
                        writeLogs.LogAndConsoleWirite($"Latenzträger Einheiten traniert: {latenztregerTruppenTraniertCounter}");
                        writeLogs.LogAndConsoleWirite($"Sniper Einheiten traniert: {sniperTruppenTraniertCounter}");

                        writeLogs.LogAndConsoleWirite($"Erkundungsbonus: {erkungBonusCounter}");
                        writeLogs.LogAndConsoleWirite($"Erkundungskämpfe: {erkundungKampfCounter}");

                        writeLogs.LogAndConsoleWirite($"Allianz Kisten: {alianzKistenCounter}");
                        writeLogs.LogAndConsoleWirite($"Allianz Hilfe: {alianzHilfeCounter}");
                        writeLogs.LogAndConsoleWirite($"Allinaz Technolgie: {alianzTechnologie}");
                        writeLogs.LogAndConsoleWirite($"Heilung: {heilenCounter}");
                        writeLogs.LogAndConsoleWirite($"Helden rekuritiert: {heldenRecurt}");
                        writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                        //deviceControl.BackUneversal();

                        helden.HeldenRecurtieren();
                        deviceControl.StableControl();
                        
                        bestienJagt.BestienJagtStarten(25, false);
                        deviceControl.StableControl();
                        
                        truppenHeilen.Heilen();
                        deviceControl.StableControl();
                    
                        deviceControl.OfflineErtregeAbholen();
                        deviceControl.StableControl();

                        lagerOnlineBelohnung.AusdauerAbholen();
                        deviceControl.StableControl();

                        lagerOnlineBelohnung.GeschnekAbholen();
                        deviceControl.StableControl();

                        erkundung.Erkundungskampf();
                        deviceControl.StableControl();
                        erkundung.ErkundungAbholen();
                        deviceControl.StableControl();

                        allianz.KistenAbholen();
                        deviceControl.StableControl();
                        allianz.Hilfe();
                        deviceControl.StableControl();
                        allianz.Technologie(3);
                        deviceControl.StableControl();

                        truppenTraining.TrainiereInfaterie(10);
                        deviceControl.StableControl();

                        truppenTraining.TrainiereLatenzTreger(10);
                        deviceControl.StableControl();
                        
                        truppenTraining.TrainiereSniper(10);
                        deviceControl.StableControl();
                        

                    }
                }
                catch (Exception)
                {
                    deviceControl.CloseApp();
                    deviceControl.KillNoxPlayerProcess();
                    Thread.Sleep(10000);
                }
            }
        }


    }
}
