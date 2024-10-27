using System.Diagnostics;

namespace EMU
{
    internal static class Program
    {
        internal static int commandDelay = 1000; // Pause in milliseconds between each command.
        internal static int reconnectSleepTime = 10; // Sleep time in milliseconds after reconnecting.
        internal static int roundCount = 0;


        internal static string allianceAutobeitrit = "ON";

        internal static int offlineEarningsCounter = 0;

        internal static int storageBonusGiftCounter = 0;
        internal static int storageBonusStaminaCounter = 0;

        internal static int infantryUnitsTrainedCounter = 0;
        internal static int latencyCarrierUnitsTrainedCounter = 0;
        internal static int sniperUnitsTrainedCounter = 0;
        internal static int explorationBonusCounter = 0;
        internal static int explorationBattleCounter = 0;

        internal static int allianceHelpCounter = 0;
        internal static int allianceChestsCounter = 0;
        internal static int allianceTechnologyCounter = 0;

        internal static int healingCounter = 0;

        internal static int advancedHeroRecruitmentCounter = 0;
        internal static int epicHeroRecruitmentCounter = 0;

        internal static int beastHuntCounter = 0;

        internal static int lifeTreeEssenceCounter = 0;

        internal static int vipStatusCounter = 0;

        internal static int arenaFightsCounter = 0;

        private static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            // System Klassen
            WriteLogs writeLogs = new WriteLogs();
            PrintInfo printInfo = new PrintInfo(writeLogs);
            DeviceControl deviceControl = new DeviceControl(writeLogs, printInfo);
            StableControl stableControl = new StableControl(writeLogs, printInfo, deviceControl);


            // Automations in Game Klassen
            Erkundung erkundung = new Erkundung(writeLogs, deviceControl);
            TruppenTraining truppenTraining = new TruppenTraining(writeLogs, deviceControl);
            LagerOnlineBelohnung lagerOnlineBelohnung = new LagerOnlineBelohnung(writeLogs, deviceControl);
            Allianz allianz = new Allianz(writeLogs, deviceControl);
            Jagt Jagt = new Jagt(writeLogs, deviceControl);
            TruppenHeilen truppenHeilen = new TruppenHeilen(writeLogs, deviceControl);
            Helden helden = new Helden(writeLogs, deviceControl);
            LebensBaum lebensBaum = new LebensBaum(writeLogs, deviceControl);
            GuvenourBefehl guvenourBefehl = new GuvenourBefehl(writeLogs, deviceControl);
            VIP vip = new VIP(writeLogs, deviceControl);
            Arena arena = new Arena(writeLogs, deviceControl);

            deviceControl.ShowSetting();
            stableControl.Control();

            deviceControl.GetResolution();
            deviceControl.ResetResolution();
            Thread.Sleep(10000);
            deviceControl.GetResolution();

            //deviceControl.TrackTouchEvents();        

            while (true)
            {
                try
                {
                    stableControl.Control();
                    printInfo.PrintSummary();                    
                    stopwatch.Stop();
                    writeLogs.LogAndConsoleWirite($"Startdauer: {stopwatch.Elapsed.TotalSeconds} sec");
                    writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                    Console.ResetColor();
                    stopwatch.Restart();

                    // Nur beim ersten Start auszuführen.
                    deviceControl.OfflineErtregeAbholen();

                    // Arena Kampf
                    arena.GoToArena();
                    stableControl.Control();

                    // Stabilität Check
                    deviceControl.BackUneversal();
                    deviceControl.CheckePositionAndGoStadt();
                    stableControl.Control();

                    // Truppen Heilung
                    truppenHeilen.Heilen();
                    stableControl.Control();

                    // Allianz
                    allianz.Hilfe();
                    stableControl.Control();
                    allianz.Technologie(3);
                    stableControl.Control();
                    allianz.KistenAbholen();
                    stableControl.Control();
                    allianz.AutobeitritAktivieren();
                    stableControl.Control();

                    // Jagt
                    Jagt.BestienJagtStarten(26, false);
                    stableControl.Control();
                    //Jagt.PolarTerrorStarten(5, false);
                    //deviceControl.StableControl();


                    // Lager
                    helden.HeldenRekrutieren();
                    stableControl.Control();
                    lagerOnlineBelohnung.GeschnekAbholen();
                    stableControl.Control();
                    lagerOnlineBelohnung.AusdauerAbholen();
                    stableControl.Control();

                    // Erkundung
                    erkundung.Erkundungskampf();
                    stableControl.Control();
                    erkundung.ErkundungAbholen();
                    stableControl.Control();

                    // Truppen Training
                    truppenTraining.TrainiereInfaterie(10);
                    stableControl.Control();
                    truppenTraining.TrainiereLatenzTreger(10);
                    stableControl.Control();                   
                    truppenTraining.TrainiereSniper(10);
                    stableControl.Control();

                    // Guvenour Befehle
                    guvenourBefehl.EilauftragAbholen();
                    guvenourBefehl.FestlichkeitenAbholen();
                    stableControl.Control();

                    // Lebens Baum
                    lebensBaum.BaumBelohnungAbholen();
                    stableControl.Control();
                    lebensBaum.EssensVonFreundenAbholen();
                    stableControl.Control();

                    // VIP
                    vip.KistenAbholen();
                    stableControl.Control();

                    // Stoppen der Zeitmessung und Ausgabe der Dauer
                    roundCount++;
                    stopwatch.Stop();
                    writeLogs.LogAndConsoleWirite($"\nRundendauer: {stopwatch.Elapsed.TotalMinutes} Minuten, Runde {roundCount}");
                    stopwatch.Restart();
                    writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                    
                }
                catch
                {
                    
                }
            }
        }


    }
}
