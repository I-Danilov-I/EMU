namespace EMU
{
    internal static class Program
    {
        internal static string adbPath = @"C:\Program Files (x86)\Nox\bin\adb.exe";
        internal static string screenshotDirectory = @"C:\Users\Anatolius\source\repos\EMU\Screens\img.png";

        private static void Main()
        {

            // ADB-Server neu starten und mit dem Nox Player verbinden
            AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
            AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");

            // Bildschirmauflösung abfragen
            var (width, height) = Display.GetScreenResolution(adbPath);

            ClicksOperate.ClickToPosition(adbPath, width, height);
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            Checker.CheckTextInScreenshot(screenshotDirectory);
        }

    }
}
