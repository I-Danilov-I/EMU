

namespace EMU
{
    internal class LagerOnlineBelohnung
    {

        internal static void Abholen(string adbPath, string screenshotDirectory)
        {
            WriteLogs.LogAndConsoleWirite("\n\nLager Online Belohnung wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameSteuerung.SeitenMenuOpen(adbPath);
            GameSteuerung.SeitenMenuScrolDown(adbPath);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Belohnung", "Belohnung"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "0000003b", "000003f8"); // Wähle Online Belohnungen

                DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce"); // Abholen

                DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce"); // Bestätigen

                DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "0000023f", "000002a6"); // Schliese das Seitenmenü

                Program.lagerBonusCounter++;
                WriteLogs.LogAndConsoleWirite($"Lager Online Belohnung erforgreich abgeholt!");
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
