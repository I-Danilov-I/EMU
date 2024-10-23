
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
            //AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            //AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");
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
                    // Offline Erträge sammeln, bestätigen drücken.
                    Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
                    bool offlineErtrege = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Willkommen");
                    if (offlineErtrege == true)
                    {
                        ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, hexX: "000001bf", hexY: "000004d3"); // Bestätigen Button klicken
                        Thread.Sleep(10000);
                    }


                    AppSteuerung.Wiederverbinden(adbPath, screenshotDirectory, 7);
                    TruppenTraining.TrainiereInfaterie(adbPath, screenshotDirectory);

                    AppSteuerung.Wiederverbinden(adbPath, screenshotDirectory, 7);
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                {
                    WriteLogs.LogAndConsoleWirite("App wird gestartet...");
                    AppSteuerung.StartApp(adbPath, "com.gof.global");
                    WriteLogs.LogAndConsoleWirite("App erfogreich gestartet!");
                    Thread.Sleep(60*1000);
                }
            } 

         
        }
    }
}
