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
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("[PROGRAMM START]");
            // ADB-Server neu starten und mit dem Nox Player verbinden
            // AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            // AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");
            // GetInfoFromDevice.ListAllDevices(adbPath, logFilePath);
            var (width, height) = GetInfoFromDevice.GetScreenResolution(adbPath); // Bildschirmauflösung abfragen
            Console.WriteLine("------------------------------------------------------------------------");
            // GetInfoFromDevice.TrackTouchEvents(adbPath, inputDevice, "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\TouchLogs.txt");


            while (true)
            {
                GetInfoFromDevice.ListRunningApps(adbPath);
                bool isAppRunning = IsAppRunning(adbPath, "com.gof.global");
                if (isAppRunning == true)
                {
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                { 

                }
            } 

         
        }

        public static bool IsAppRunning(string adbPath, string packageName)
        {
            // Befehl, um zu überprüfen, ob die App läuft
            string adbCommand = $"shell pidof {packageName}";
            string result = AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
            return !string.IsNullOrEmpty(result); // Wenn ein Ergebnis vorliegt, läuft die App
        }

    }
}
