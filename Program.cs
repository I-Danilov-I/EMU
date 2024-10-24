namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string inputDevice = "/dev/input/event4";
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
        internal static string packeName = "com.gof.global"; // Warte für erneuten Start fall anderes Gerät aktiv ist.
        internal static int timeSleepMin = 1; // Warte für erneuten Start fall anderes Gerät aktiv ist.
        
        internal static int lagerBonusCounter = 0;
        internal static int gesamtTruppenTraniert = 0;


        private static void Main()
        {
            while (true)
            {
                try
                {
                    WriteLogs.LogAndConsoleWirite("\n\n[PROGRAMM START]");
                    WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
                    NoxControl.StartNoxPlayer();
                    NoxControl.StartADBConnection(adbPath);
                    //DeviceInfo.ListAllDevices(adbPath);
                    //DeviceInfo.ListRunningApps(adbPath);
                    //DeviceInfo.TrackTouchEvents(adbPath, inputDevice);                   
                    WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

                    while (true)
                    {
                        if (DeviceRemote.IsAppRunning(adbPath, packeName) == true)
                        {
                            // DeviceRemote.RestartApp(adbPath, packeName);
                            DeviceRemote.MonitorISGameOnStartpointAndMakeReady(adbPath, screenshotDirectory);
                            WriteLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]_________________________________");
                            WriteLogs.LogAndConsoleWirite($"Lagerbonus abgeholt: {lagerBonusCounter}  Infaterie Einheiten traniert: {gesamtTruppenTraniert}");
                            WriteLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");

                            TruppenTraining.TrainiereInfaterie(adbPath, screenshotDirectory, 5);
                            LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                        }
                        else
                        {
                            DeviceRemote.StartApp(adbPath, packeName);
                            DeviceRemote.MonitorISGameOnStartpointAndMakeReady(adbPath, screenshotDirectory);
                            GameSteuerung.FirstStart(adbPath, screenshotDirectory);
                        }
                    }

                }
                catch (Exception){  }
            }
         

        }
    }
}
