

namespace EMU
{
    internal class LagerOnlineBelohnung
    {
        internal static int counter = 0; 

        internal static void Abholen(string adbPath, string screenshotDirectory)
        {
            WriteLogs.LogAndConsoleWirite("\n\nLager Online Belohnung wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameSteuerung.SeitenMenuOpen(adbPath);
            GameSteuerung.SeitenMenuScrolDown(adbPath);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Belohnung"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "0000003b", "000003f8"); // Wähle Online Belohnungen
                Thread.Sleep(5000);

                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce"); // Abholen
                Thread.Sleep(5000);

                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce"); // Bestätigen
                Thread.Sleep(5000);

                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "0000023f", "000002a6"); // Schliese das Seitenmenü
                Thread.Sleep(5000);

                counter++;
                WriteLogs.LogAndConsoleWirite($"Lager Online Belohnung erforgreich abgeholt!");
                WriteLogs.LogAndConsoleWirite($"Erfogreiche Abholungen: {counter}");

            }
            else
            {
                GameSteuerung.SeitenMenuClose(adbPath);
                WriteLogs.LogAndConsoleWirite("Keine Online Belohnung verfügbar, versuche später erneut.");
                Thread.Sleep(5000);
            }
        }


    }
}
