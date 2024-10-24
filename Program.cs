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
                    NoxControl.StartNoxPlayer();
                    NoxControl.StartADBConnection(adbPath);
                    //DeviceInfo.ListAllDevices(adbPath);
                    //DeviceInfo.ListRunningApps(adbPath);
                    //DeviceInfo.TrackTouchEvents(adbPath, inputDevice);                   
                    WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

                    while (true)
                    {
                        WriteLogs.LogAndConsoleWirite($"\n\n_________________________[GESAMTÜBERSICHT]__________________________________");
                        WriteLogs.LogAndConsoleWirite($"Lagerbonus abgeholt: {lagerBonusCounter}  Infaterie Einheiten traniert: {gesamtTruppenTraniert}");
                        WriteLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------\n");

                        if (DeviceRemote.IsAppRunning(adbPath, packeName) == true)
                        {
                            TruppenTraining.TrainiereInfaterie(adbPath, screenshotDirectory, 1);
                            LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                        }
                        else
                        {
                            DeviceRemote.StartApp(adbPath, packeName);
                            while (true) 
                            {
                                Thread.Sleep(5 * 1000);
                                Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
                               
                                if (Screenshot.CheckTextInScreenshot(screenshotDirectory, "Welt", "Allianz") == true)
                                {
                                    GameSteuerung.FirstStart(adbPath, screenshotDirectory);
                                    break;
                                }                              
                            }
                        }
                    }

                }
                catch (Exception){  }
            }
         

        }
    }
}
