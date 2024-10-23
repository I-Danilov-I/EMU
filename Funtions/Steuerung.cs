namespace EMU.Funtions
{
    internal class Steuerung
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
    }
}
