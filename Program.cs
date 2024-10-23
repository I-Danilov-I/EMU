﻿namespace EMU
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
            //AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");

            //GetInfoFromDevice.ListAllDevices(adbPath, logFilePath);
            //GetInfoFromDevice.ListRunningApps(adbPath, logFilePath);

            //var (width, height) = GetInfoFromDevice.GetScreenResolution(adbPath); // Bildschirmauflösung abfragen

            //GetInfoFromDevice.TrackTouchEvents(adbPath, inputDevice);
            Console.WriteLine("------------------------------------------------------------------------");
            


            while (true)
            {
                bool isAppRunning = Steuerung.IsAppRunning(adbPath, "com.gof.global");
                if (isAppRunning == true)
                {
                    Screenshot.TakeScreenshot(adbPath, screenshotDirectory);                 
                    bool OnOff = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Tipps");
                    if (OnOff == true) 
                    {
                        ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "0000027d", "000003c7"); // Wiederverbinden Klicken
                        Thread.Sleep(10000);
                    }

                    // ABlauf
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                {
                    Console.WriteLine("App wird gestartet...");
                    Steuerung.StartApp(adbPath, "com.gof.global");
                    Console.WriteLine("App erfogreich gestartet!");
                    Thread.Sleep(40*1000);
                }
            } 

         
        }
    }
}
