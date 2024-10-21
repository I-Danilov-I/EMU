using EMU;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Pfad zur ADB von Nox Player festlegen
        string adbPath = @"C:\Program Files (x86)\Nox\bin\adb.exe";

        // ADB-Server neu starten und mit dem Nox Player verbinden
        AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
        AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
        AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");

        // Bildschirmauflösung abfragen
        var (width, height) = Display.GetScreenResolution(adbPath);

        // Klick auf Position durchführen (vorerst auskommentiert)
        // ClicksOperate.ClickToPosition(adbPath, width, height);
        // ClicksOperate.ClickInQuadraticArea(adbPath, width, height, 150, 10);

        // Verzeichnis für Screenshots überprüfen/erstellen
        string screenshotDirectory = @"C:\Users\Anatolius\source\repos\EMU\Screens";
        if (!Directory.Exists(screenshotDirectory))
        {
            Directory.CreateDirectory(screenshotDirectory);
        }

        // Screenshot auf dem Emulator erstellen und auf den PC übertragen
        string localScreenshotPath = Path.Combine(screenshotDirectory, "img.png");
        TakeScreenshot(adbPath, localScreenshotPath);

        // Prüfen, ob Text auf dem Screenshot vorhanden ist
        Checker.CheckTextInScreenshot(localScreenshotPath);
    }

    static void TakeScreenshot(string adbPath, string localPath)
    {
        try
        {
            // Verzeichnis für Screenshots überprüfen/erstellen
            string screenshotDirectory = @"C:\Users\Anatolius\source\repos\EMU\Screens";
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
            string pullCommand = $"pull /sdcard/screenshot.png {localPath}";
            AdbCommand.ExecuteAdbCommand(adbPath, pullCommand);

            Console.WriteLine($"Screenshot erfolgreich erstellt und gespeichert unter: {localPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Erstellen des Screenshots: " + ex.Message);
        }
    }
}
