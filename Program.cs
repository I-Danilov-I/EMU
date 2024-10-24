namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string inputDevice = "/dev/input/event4";
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
        
        internal static int timeSleepMin = 1; // Warte für erneuten Start fall anderes GErät aktiv ist.

        private static void Main()
        {
            WriteLogs.LogAndConsoleWirite("[PROGRAMM START]___________________________________________________________");
            //NoxControl.StartNoxPlayer();
            //NoxControl.StartADBConnection(adbPath);
            //DeviceInfo.ListAllDevices(adbPath);
            //DeviceInfo.ListRunningApps(adbPath);
            //DeviceInfo.TrackTouchEvents(adbPath, inputDevice);
            // GameSteuerung.FirstStart(adbPath, screenshotDirectory);
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            
            while (true)
            {
                WriteLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]__________________________________");
                WriteLogs.LogAndConsoleWirite($"Lagerbonus abgeholt: {LagerOnlineBelohnung.counter}  Infaterie Einheiten traniert: {TruppenTraining.gesamtTruppenTraniert}");
                WriteLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------\n");

                if (DeviceRemote.IsAppRunning(adbPath, "com.gof.global") == true)
                { 
                    TruppenTraining.TrainiereInfaterie(adbPath, screenshotDirectory, 1); 
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                {                 
                    DeviceRemote.StartApp(adbPath, "com.gof.global");
                    Thread.Sleep(70*1000);
                }
            } 

         
        }
    }
}
