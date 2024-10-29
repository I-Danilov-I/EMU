﻿using System.Diagnostics;
using EMU.Automations;

namespace EMU
{
    internal static class Program
    {
        // Wenn das Programm in einem veröffentlichten Zustand ausgeführt wird, verwende das Verzeichnis des Executables.
        internal static string baseDirectory = AppContext.BaseDirectory;

        internal static int commandDelay = 100; // Pause in milliseconds between each command.
        internal static int reconnectSleepTime = 10; // Sleep time in milliseconds after reconnecting.
        internal static int roundCount = 0;
        internal static bool truppenAusgleich = false; // Truppen ausgleichen
        internal static string allianceAutobeitrit = "ON";

        // Setze die Verzeichnisse relativ zu diesem Basisverzeichnis.
        internal static string trainedDataDirectory = Path.Combine(baseDirectory);
        internal static string screenshotDirectory = Path.Combine(baseDirectory, "Screens");
        internal static string logFileFolderPath = Path.Combine(baseDirectory);
        internal static string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png");

        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string noxExePath = "C:\\Program Files\\Nox\\bin\\Nox.exe";
        internal static string inputDevice = "/dev/input/event4";
        internal static string packageName = "com.gof.global";


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
            Logging writeLogs = new Logging();
            Logging logging = new Logging();
            DeviceControl deviceControl = new DeviceControl(logging);
            StableControl stableControl = new StableControl(logging, deviceControl);

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
            Geheimdienst geheimdienst = new Geheimdienst(writeLogs, deviceControl);
            Begleiter begleiter = new Begleiter(writeLogs, deviceControl);
            //-----------------------------------------------------------------------------------------------------------
            // Aufruf der Klick-Methode mit diesen Margins

            //deviceControl.TrackTouchEvents();

            //begleiter.GoToBegleiter(); IN ARBET!!!
           

            /*
            logging.ShowSetting();
            stableControl.Control();
            stopwatch.Stop();
            writeLogs.LogAndConsoleWirite($"Startdauer: {stopwatch.Elapsed.TotalSeconds} sec");
            writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
            Console.ResetColor();
            */
            while (true)
            {
                try
                {
                    geheimdienst.StartProcess();
                    /*
                     * 
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

                       // Geheimmission
                       geheimdienst.GoToGeheimdienst();
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
                       Jagt.BestienJagtStarten(26);
                       stableControl.Control();
                       Console.ForegroundColor = ConsoleColor.Cyan;
                       Jagt.PolarTerrorStarten(5);
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
                       */
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
