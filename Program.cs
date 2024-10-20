using EMU;

class Program
{
    static void Main(string[] args)
    {

        Checker.CheckTextInScreenshot();
     

        // Pfad zur ADB von Nox Player festlegen
        string adbPath = @"C:\Program Files (x86)\Nox\bin\adb.exe";

        // ADB-Server neu starten und mit dem Nox Player verbinden
        AdbCommand.ExecuteAdbCommand(adbPath, "kill-server");
        AdbCommand.ExecuteAdbCommand(adbPath, "start-server");
        AdbCommand.ExecuteAdbCommand(adbPath, "connect 127.0.0.1:62001");

        // Bildschirmauflösung abfragen
        var (with, hight) = Display.GetScreenResolution(adbPath);

        // Klick auf Position durchführen
        // ClicksOperate.ClickToPosition(adbPath ,with, hight);
        ClicksOperate.ClickInQuadraticArea(adbPath, with, hight, 150, 10);
    }
}
