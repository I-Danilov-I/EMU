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

            AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");

            GetInfoFromDevice.ListAllDevices(adbPath, logFilePath);
            GetInfoFromDevice.ListRunningApps(adbPath);

            var (width, height) = GetInfoFromDevice.GetScreenResolution(adbPath); // Bildschirmauflösung abfragen

            Console.WriteLine("------------------------------------------------------------------------");
            
            // GetInfoFromDevice.TrackTouchEvents(adbPath, inputDevice, "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\TouchLogs.txt");


            while (true)
            {
                bool isAppRunning = IsAppRunning(adbPath, "com.gof.global");
                if (isAppRunning == true)
                {
                    LagerOnlineBelohnung.Abholen(adbPath, screenshotDirectory);
                }
                else 
                {
                    Console.WriteLine("App wird gestartet...");
                    StartApp(adbPath, "com.gof.global");
                    Console.WriteLine("App erfogreich gestartet!");
                    Thread.Sleep(40*1000);
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

        public static void StartApp(string adbPath, string packageName)
        {
            // Befehl zum Starten der App
            string adbCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
            Console.WriteLine($"App {packageName} wird gestartet.");
        }


    }
}
