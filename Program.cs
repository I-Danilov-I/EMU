using System.Diagnostics;

namespace EMU
{
    internal static class Program
    {
        // Wenn das Programm in einem veröffentlichten Zustand ausgeführt wird, verwende das Verzeichnis des Executables.
        internal static string baseDirectory = AppContext.BaseDirectory;

        internal static int commandDelay = 500; // Pause in milliseconds between each command.
        internal static int reconnectSleepTime = 20; // Sleep time in milliseconds after reconnecting.
        internal static int roundCount = 0;
        internal static bool truppenAusgleich = false; // Truppen ausgleichen
        
        internal static string allianceAutobeitrit = "ON";
        internal static int polarTerrorLevel = 6;
        internal static int bestienJagtLevel = 26;

        internal static string trainedDataDirectory = Path.Combine(baseDirectory);
        internal static string screenshotDirectory = Path.Combine(baseDirectory, "Screens");
        internal static string logFileFolderPath = Path.Combine(baseDirectory);
        internal static string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png");

        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string noxExePath = "C:\\Program Files\\Nox\\bin\\Nox.exe";
        internal static string inputDevice = "/dev/input/event4";
        internal static string packageName = "com.gof.global";

        internal static int geheimdienstCounter = 0;

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

            Logging writeLogs = new Logging();
            Logging logging = new Logging();
            DeviceControl deviceControl = new DeviceControl(logging);
            StableControl stableControl = new StableControl(logging, deviceControl);

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
            Geheimdienst geheimdienst = new Geheimdienst(writeLogs, deviceControl);
            //Begleiter begleiter = new Begleiter(writeLogs, deviceControl);
            //-----------------------------------------------------------------------------------------------------------
            //deviceControl.TrackTouchEvents();       
            // Geheimmission
            geheimdienst.StartProcess();
            stableControl.Control();
            logging.ShowSetting();
            stableControl.Control();
            stopwatch.Stop();
            writeLogs.LogAndConsoleWirite($"Startdauer: {stopwatch.Elapsed.TotalSeconds} sec");
            writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
            Console.ResetColor();
            
            while (true)
            {
                try
                {
                    stopwatch.Restart();
                    logging.PrintSummary();
                    if(roundCount < 1)
                    {
                        // First 
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        deviceControl.OfflineErtregeAbholen();
                        deviceControl.BackUneversal();
                        deviceControl.GoStadt();
                        stableControl.Control();
                    }

                    // Truppen Heilung
                    truppenHeilen.Heilen();
                    stableControl.Control();

                    // Geheimmission
                    geheimdienst.StartProcess();
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
                    Jagt.PolarTerrorStarten(polarTerrorLevel);
                    stableControl.Control();
                    Jagt.BestienJagtStarten(bestienJagtLevel);
                    stableControl.Control();

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

                    // Arena Kampf (Optimeirungbedarf)
                    arena.GoToArena();
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
                    Console.ResetColor();
                    writeLogs.LogAndConsoleWirite($"\nRundendauer: {stopwatch.Elapsed.TotalMinutes} Minuten, Runde {roundCount}");
                    stopwatch.Restart();
                    writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
                       
                }
                catch( Exception ex )
                {
                    writeLogs.LogAndConsoleWirite($"{ex.Message}");
                    Thread.Sleep(1000 * 60);
                }
            }



        }
    }
}
