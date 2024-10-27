using System.Diagnostics;
using EMU.Automations;
using EMU.Funtions;

namespace EMU
{
    internal static class Program
    {
        internal static int commandDelay = 1000; // Pause in milliseconds between each command.
        internal static int reconnectSleepTime = 10; // Sleep time in milliseconds after reconnecting.
        internal static int roundCount = 0;

        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string inputDevice = "/dev/input/event4";
        internal static string packageName = "com.gof.global";
        internal static string screenshotDirectory = Path.Combine(Directory.GetCurrentDirectory().Replace("bin\\Debug\\net8.0\\win-x64", ""), "Screens");
        internal static string logFileFolderPath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";

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
            stableControl.GetResolution();
            
            //deviceControl.TrackTouchEvents();        
            stopwatch.Stop();
            writeLogs.LogAndConsoleWirite($"Startdauer: {stopwatch.Elapsed.TotalSeconds} sec");
            writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
            Console.ResetColor();

            while (true)
            {
                try
                {          
                    stopwatch.Restart();
                    printInfo.PrintSummary();    

                    // First 
                    deviceControl.OfflineErtregeAbholen();
                    deviceControl.BackUneversal();
                    deviceControl.CheckePositionAndGoStadt();
                    stableControl.Control();

                    // Arena Kampf
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    arena.GoToArena();


                    // Truppen Heilung
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    truppenHeilen.Heilen();
                    stableControl.Control();

                    // Allianz
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    allianz.Hilfe();
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    allianz.Technologie(3);
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    allianz.KistenAbholen();
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    allianz.AutobeitritAktivieren();
                    stableControl.Control();

                    // Jagt
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Jagt.BestienJagtStarten(26, false);
                    stableControl.Control();
                    //Jagt.PolarTerrorStarten(5, false);
                    //deviceControl.StableControl();


                    // Lager
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    helden.HeldenRekrutieren();
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    lagerOnlineBelohnung.GeschnekAbholen();
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    lagerOnlineBelohnung.AusdauerAbholen();
                    stableControl.Control();

                    // Erkundung
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    erkundung.Erkundungskampf();
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    erkundung.ErkundungAbholen();
                    stableControl.Control();

                    // Truppen Training
                    truppenTraining.TrainiereInfaterie(10);
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    truppenTraining.TrainiereLatenzTreger(10);
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    truppenTraining.TrainiereSniper(10);
                    stableControl.Control();

                    // Guvenour Befehle
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    guvenourBefehl.EilauftragAbholen();
                    guvenourBefehl.FestlichkeitenAbholen();
                    stableControl.Control();

                    // Lebens Baum
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    lebensBaum.BaumBelohnungAbholen();
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    lebensBaum.EssensVonFreundenAbholen();
                    stableControl.Control();

                    // VIP
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    vip.KistenAbholen();
                    stableControl.Control();

                    // Stoppen der Zeitmessung und Ausgabe der Dauer
                    roundCount++;
                    stopwatch.Stop();
                    Console.ResetColor();
                    writeLogs.LogAndConsoleWirite($"\nRundendauer: {stopwatch.Elapsed.TotalMinutes} Minuten, Runde {roundCount}");
                    stopwatch.Restart();
                    writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                    
                }
                catch
                {
                    Thread.Sleep(1000 * 60);
                }
            }
        }


    }
}
