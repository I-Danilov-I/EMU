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


            //Makro.TrackTouchEvents(adbPath, inputDevice, "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\TouchLogs.txt");
            Makro.PlayMacro(adbPath, macroPathLagerhausBelohnung);
            ClicksOperate.ClickAndHoldAndScroll(adbPath, "000001a6", "000003b5", "000001c2", "00000094", 300, 500);             

            // ClicksOperate.ScrollOnScreen(adbPath, 500, 1200, 500, 300, 500);  // Scroll von unten nach oben         
            //Console.WriteLine("Liste aller Eingabegeräte:");
            //Info.ListAllDevices(adbPath, logFilePath);
            //Console.WriteLine("Starte die Erfassung von Touch-Ereignissen...");
        }
    }
}
