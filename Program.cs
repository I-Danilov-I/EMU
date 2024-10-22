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
            Console.WriteLine("___________________________________________________________________________");
            // ADB-Server neu starten und mit dem Nox Player verbinden
            // AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            // AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");
            // Info.ListAllDevices(adbPath, logFilePath);
            var (width, height) = GetInfoFromDevice.GetScreenResolution(adbPath); // Bildschirmauflösung abfragen
            Console.WriteLine("___________________________________________________________________________\n");

            // Makro.TrackTouchEvents(adbPath, inputDevice, "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\TouchLogs.txt");

            LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);

         
        }
    }
}
