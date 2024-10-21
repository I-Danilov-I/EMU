namespace EMU
{
    internal static class Screenshot
    {
        internal static void TakeScreenshot(string adbPath, string screenshotDirectory)
        {
            try
            {
                if (!Directory.Exists(screenshotDirectory))
                {
                    Directory.CreateDirectory(screenshotDirectory);
                }

                // Screenshot auf dem Emulator erstellen und auf den PC übertragen
                string localScreenshotPath = Path.Combine(screenshotDirectory, "img.png");
                // Screenshot auf dem Emulator erstellen und speichern
                string screenshotCommand = "shell screencap -p /sdcard/screenshot.png";
                AdbCommand.ExecuteAdbCommand(adbPath, screenshotCommand);

                // Screenshot vom Emulator auf den PC übertragen
                string pullCommand = $"pull /sdcard/screenshot.png {screenshotDirectory}";
                AdbCommand.ExecuteAdbCommand(adbPath, pullCommand);

                Console.WriteLine($"Screenshot erfolgreich erstellt und gespeichert unter: {screenshotDirectory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }
    }
}
