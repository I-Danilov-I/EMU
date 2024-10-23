using EMU.Funtions;

namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string inputDevice = "/dev/input/event4";
        internal static string logFilePath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";

        private static void Main()
        {
            Console.WriteLine("[PROGRAMM START]---------------------------------------------------------");
            //AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            //AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");
            //DeviceInfo.ListAllDevices(adbPath);
            //DeviceInfo.ListRunningApps(adbPath);

            DeviceInfo.TrackTouchEvents(adbPath, inputDevice);
            Console.WriteLine("------------------------------------------------------------------------");
            

            while (true)
            {
                bool isAppRunning = Steuerung.IsAppRunning(adbPath, "com.gof.global");
                if (isAppRunning == true)
                {

                    // Wiederverbinden wenn vo nanderem Gerät beigetreten.
                    Screenshot.TakeScreenshot(adbPath, screenshotDirectory);                 
                    bool OnOff = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Tipps");
                    if (OnOff == true) 
                    {
                        ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, hexX: "0000027d", hexY: "000003c7"); // Wiederverbinden Klicken.
                        Thread.Sleep(10000);
                    }

                    // Offline Erträge, Besättigen drücken.
                    Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
                    bool offlineErtrege = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Willkommen");
                    if (offlineErtrege == true)
                    {
                        ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, hexX: "000001bf", hexY: "000004d3"); // Bestätigen Button klicken
                        Thread.Sleep(10000);
                    }

                    // Ablauf__________________________________________________
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                {
                    Console.WriteLine("App wird gestartet...");
                    Steuerung.StartApp(adbPath, "com.gof.global");
                    Console.WriteLine("App erfogreich gestartet!");
                    Thread.Sleep(60*1000);
                }
            } 

         
        }
    }
}
