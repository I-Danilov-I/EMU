

namespace EMU
{
    internal class LagerOnlineBelohnung
    {

        internal static void GeschnekAbholen(string adbPath, string screenshotDirectory)
        {
            WriteLogs.LogAndConsoleWirite("\n\nLager Online Belohnung wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameControl.SeitenMenuOpen(adbPath);
            GameControl.SeitenMenuScrolDown(adbPath);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Belohnung", "Belohnung"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000003b", "000003f8"); // Wähle Online Belohnungen

                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce"); // Abholen

                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce"); // Bestätigen

                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000023f", "000002a6"); // Schliese das Seitenmenü

                Program.lagerBonusGeschenkCounter++;
                WriteLogs.LogAndConsoleWirite($"Lager Online Belohnung erforgreich abgeholt!");
            }
            else
            {
                GameControl.SeitenMenuClose(adbPath);
                WriteLogs.LogAndConsoleWirite("Keine Online Belohnung verfügbar, versuche später erneut.");
                Thread.Sleep(5000);
            }
        }

        internal static void AusdauerAbholen(string adbPath, string screenshotDirectory)
        {
            WriteLogs.LogAndConsoleWirite("\n\nLager Online Ausdauer wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000000f1", "0000004d"); // Bonusübersicht klick
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001cf", "000003a6"); // Kraft klick
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000002fe", "0000024b"); // Gebäudekraft klick
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001c8", "000002bfe"); // Gebäude anwählen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001c1", "000002c6"); // Abholen 

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Nehmen", "herzliches"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001cb", "0000049a"); // Bestätigen
                Program.lagerBonusAausdauerCounter += 120;
                WriteLogs.LogAndConsoleWirite($"Lager Online Ausdauer erforgreich abgeholt!");
            }
            else
            {
                WriteLogs.LogAndConsoleWirite($"Aktuel keine Ausdauer zu verschenken.");
            }
        }


    }
}
