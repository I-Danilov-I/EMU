namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string inputDevice = "/dev/input/event4";
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
        
        internal static int timeSleepMin = 7;

        private static void Main()
        {
            WriteLogs.LogAndConsoleWirite("[PROGRAMM START]");
            NoxControl.StartNoxPlayer();
            NoxControl.StartADBConnection(adbPath);
            //DeviceInfo.ListAllDevices(adbPath);
            //DeviceInfo.ListRunningApps(adbPath);

            //DeviceInfo.TrackTouchEvents(adbPath, inputDevice);
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            
            while (true)
            {
                WriteLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]__________________________________");
                WriteLogs.LogAndConsoleWirite($"Lagerbonus abgeholt: {LagerOnlineBelohnung.counter}  Infaterie Einheiten traniert: {TruppenTraining.gesamtTruppenTraniert}");
                WriteLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------\n");
                bool isAppRunning = AppSteuerung.IsAppRunning(adbPath, "com.gof.global");
                if (isAppRunning == true)
                {
                    GameSteuerung.FirstStart(adbPath, screenshotDirectory);
                    AppSteuerung.Wiederverbinden(adbPath, screenshotDirectory, 7); 
                    TruppenTraining.TrainiereInfaterie(adbPath, screenshotDirectory, 1); 

                    AppSteuerung.Wiederverbinden(adbPath, screenshotDirectory, 7);
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                {
                    WriteLogs.LogAndConsoleWirite("App wird gestartet...");
                    AppSteuerung.StartApp(adbPath, "com.gof.global");
                    WriteLogs.LogAndConsoleWirite("App erfogreich gestartet!");
                    Thread.Sleep(70*1000);
                }
            } 

         
        }
    }
}
