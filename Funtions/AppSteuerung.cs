namespace EMU
{
    internal class AppSteuerung
    {

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
            WriteLogs.LogAndConsoleWirite($"App {packageName} wird gestartet.");
        }

        public static void DrueckeZurueckTaste(string adbPath)
        {
            string adbCommand = "shell input keyevent 4";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
        }

        internal static void Wiederverbinden(string adbPath, string screenshotDirectory, int timeSleepMin)
        {
            // Wiederverbinden wenn von anderem Gerät beigetreten.
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool OnOff = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Tipps");
            if (OnOff == true)
            {
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, hexX: "0000027d", hexY: "000003c7"); // Wiederverbinden Klicken.
                Thread.Sleep(60 * 1000 * timeSleepMin);
            }
        }

    }
}
