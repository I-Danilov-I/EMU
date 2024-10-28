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

        // Bestimme das Basisverzeichnis des Programms.
        // Wenn das Programm in einem veröffentlichten Zustand ausgeführt wird, verwende das Verzeichnis des Executables.
        internal static string baseDirectory = AppContext.BaseDirectory;

        // Setze die Verzeichnisse relativ zu diesem Basisverzeichnis.
        internal static string trainedDataDirectory = Path.Combine(baseDirectory, "Resources", "Trained");
        internal static string screenshotDirectory = Path.Combine(baseDirectory, "Resources", "Screens");
        internal static string logFileFolderPath = Path.Combine(baseDirectory, "Resources", "Logs");
        internal static string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png");

        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string noxExePath = "C:\\Program Files\\Nox\\bin\\Nox.exe";
        internal static string inputDevice = "/dev/input/event4";
        internal static string packageName = "com.gof.global";


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


            ShowSetting();
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    deviceControl.OfflineErtregeAbholen();
                    deviceControl.BackUneversal();
                    deviceControl.GoStadt();
                    stableControl.Control();

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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Jagt.PolarTerrorStarten(5, false);
                    stableControl.Control();


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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    truppenTraining.TrainiereInfaterie(10);
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    truppenTraining.TrainiereLatenzTreger(10);
                    stableControl.Control();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    truppenTraining.TrainiereSniper(10);
                    stableControl.Control();

                    // Arena Kampf (Optimeirungbedarf)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    arena.GoToArena();
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

            void ShowSetting()
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                writeLogs.LogAndConsoleWirite("\n[PROGRAMM START]");
                writeLogs.LogAndConsoleWirite("------------------------------------------------------------------------------------------------");

                // Ausgabe der Einstellungen mit einheitlicher Ausrichtung.
                printInfo.PrintSetting("Program Directory: ", baseDirectory);
                printInfo.PrintSetting("ADB Path: ", adbPath);
                printInfo.PrintSetting("Input Device: ", inputDevice);
                printInfo.PrintSetting("Packege Name: ", packageName);
                printInfo.PrintSetting("Scrrenshot Directory: ", screenshotDirectory);
                printInfo.PrintSetting("Logfiles Directory: ", logFileFolderPath);
                printInfo.PrintSetting("Trained Data Dir: ", trainedDataDirectory);
                writeLogs.LogAndConsoleWirite("------------------------------------------------------------------------------------------------");
                Console.ResetColor();
            }
        }


    }
}
