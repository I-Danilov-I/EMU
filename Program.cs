namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
        internal static string screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        internal static string inputDevice = "/dev/input/event4"; // Ändere dies entsprechend deinem Gerät


        internal static string macroPathLagerhausBelohnung = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\LagerhausBelohnung.txt";

        private static void Main()
        {
            // ADB-Server neu starten und mit dem Nox Player verbinden
            AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");
            var (width, height) = Info.GetScreenResolution(adbPath); // Bildschirmauflösung abfragen

            //Console.WriteLine("Liste aller Eingabegeräte:");
            //Info.ListAllDevices(adbPath, logFilePath);
            //Console.WriteLine("Starte die Erfassung von Touch-Ereignissen...");
            //Makro.TrackTouchEvents(adbPath, inputDevice, "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\TouchLogs.txt");


            ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "00000017", "000002b0"); // Öffne Seitenmenü
            Thread.Sleep(5000);
            ClicksOperate.ClickAndHoldAndScroll(adbPath, "0000005b", "000003ab", "00000025", "000000b5", 300, 500); // Switsche runter
            Thread.Sleep(5000);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Belohnung");
            Thread.Sleep(5000);
            if (findOrNot)
            {
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "0000003b", "000003f8"); // Wähle BEgleiter Abenteuer
                Thread.Sleep(5000);

                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce");
                Thread.Sleep(5000);
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce");
                Thread.Sleep(5000);
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce");
                Thread.Sleep(5000);
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001b5", "0000049c");
                Thread.Sleep(5000);
            }
            else 
            {
                Console.WriteLine("Keine Online Belohnung verfügbar, veersuche später erneut.");
                Thread.Sleep(5000);
            }
        }
    }
}
