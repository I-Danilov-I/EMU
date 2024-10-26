using System.Diagnostics;
using System.Reflection.Emit;

namespace EMU
{
    internal static class Program
    {
        internal static int commandDelay = 1000; // Pause in milliseconds between each command.
        internal static int reconnectSleepTime = 10; // Sleep time in milliseconds after reconnecting.
        internal static int roundCount = 0;

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
        internal static int allianceHealingCounter = 0;


        private static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            // System Klassen
            WriteLogs writeLogs = new WriteLogs();
            DeviceControl deviceControl = new DeviceControl(writeLogs);
            PrintInfo printInfo = new PrintInfo(writeLogs);

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

            deviceControl.ShowSetting();
            //deviceControl.TrackTouchEvents();        
            deviceControl.OfflineErtregeAbholen(); // Nur beim ersten Start auszuführen.

            while (true)
            {
                try
                {
                    // Connection
                    deviceControl.StartNoxPlayer();
                    deviceControl.StartADBConnection();
                    deviceControl.StartApp();
                   
                    // Timer
                    printInfo.PrintSummary();                    
                    stopwatch.Stop();
                    writeLogs.LogAndConsoleWirite($"Startdauer: {stopwatch.Elapsed.TotalSeconds} sec");
                    writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                    Console.ResetColor();
                    stopwatch.Restart();

                    // Stabilität Check
                    deviceControl.BackUneversal();
                    deviceControl.CheckePositionAndGoStadt();
                    deviceControl.StableControl();

                    // Truppen Heilung
                    truppenHeilen.Heilen();
                    deviceControl.StableControl();

                    // Allianz
                    allianz.Hilfe();
                    deviceControl.StableControl();
                    allianz.Technologie(3);
                    deviceControl.StableControl();
                    allianz.KistenAbholen();
                    deviceControl.StableControl();

                    // Jagt
                    Jagt.BestienJagtStarten(25, false);
                    deviceControl.StableControl();
                    Jagt.PolarTerrorStarten(5, false);
                    deviceControl.StableControl();

                    // VIP
                    vip.KistenAbholen();
                    deviceControl.StableControl();

                    // Lager
                    helden.HeldenRekrutieren();
                    deviceControl.StableControl();
                    lagerOnlineBelohnung.GeschnekAbholen();
                    deviceControl.StableControl();
                    lagerOnlineBelohnung.AusdauerAbholen();
                    deviceControl.StableControl();

                    // Erkundung
                    erkundung.Erkundungskampf();
                    deviceControl.StableControl();
                    erkundung.ErkundungAbholen();
                    deviceControl.StableControl();

                    // Truppen Training
                    truppenTraining.TrainiereInfaterie(10);
                    deviceControl.StableControl();
                    truppenTraining.TrainiereLatenzTreger(10);
                    deviceControl.StableControl();                   
                    truppenTraining.TrainiereSniper(10);
                    deviceControl.StableControl();

                    // Guvenour Befehle
                    guvenourBefehl.EilauftragAbholen();
                    guvenourBefehl.FestlichkeitenAbholen();
                    deviceControl.StableControl();

                    // Lebens Baum
                    lebensBaum.BaumBelohnungAbholen();
                    deviceControl.StableControl();
                    lebensBaum.EssensVonFreundenAbholen();
                    deviceControl.StableControl();

                    // Stoppen der Zeitmessung und Ausgabe der Dauer
                    roundCount++;
                    stopwatch.Stop();
                    writeLogs.LogAndConsoleWirite($"\nRundendauer: {stopwatch.Elapsed.TotalSeconds} Sekunden, Runde {roundCount}");
                    writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                    
                }
                catch
                {
                    
                }
            }
        }


    }
}
