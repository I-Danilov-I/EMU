namespace EMU
{
    internal class LagerOnlineBelohnung : DeviceControl
    {
        GameControl GameControl = new GameControl();

        internal void GeschnekAbholen()
        {
            WriteLogs.LogAndConsoleWirite("\n\nLager Online Belohnung wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameControl.SeitenMenuOpen();
            GameControl.SeitenMenuScrolDown();

            TakeScreenshot(GetScreenDir()); // Mache ein Screenshot
            bool findOrNot = CheckTextInScreenshot("Belohnung", "Belohnung"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                ClickAtTouchPositionWithHexa("0000003b", "000003f8"); // Wähle Online Belohnungen

                ClickAtTouchPositionWithHexa("000001c3", "000002ce"); // Abholen

                ClickAtTouchPositionWithHexa("000001c3", "000002ce"); // Bestätigen

                ClickAtTouchPositionWithHexa("0000023f", "000002a6"); // Schliese das Seitenmenü

                Program.lagerBonusGeschenkCounter++;
                WriteLogs.LogAndConsoleWirite($"Lager Online Belohnung erforgreich abgeholt!");
            }
            else
            {
                GameControl.SeitenMenuClose();
                WriteLogs.LogAndConsoleWirite("Keine Online Belohnung verfügbar, versuche später erneut.");
                Thread.Sleep(5000);
            }
        }

        internal void AusdauerAbholen()
        {
            WriteLogs.LogAndConsoleWirite("\n\nLager Online Ausdauer wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

            ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
            ClickAtTouchPositionWithHexa("000002fe", "0000024b"); // Gebäudekraft klick
            ClickAtTouchPositionWithHexa("000001c8", "000002bfe"); // Gebäude anwählen
            ClickAtTouchPositionWithHexa("000001c1", "000002c6"); // Abholen 

            TakeScreenshot(GetScreenDir()); // Mache ein Screenshot
            bool findOrNot = CheckTextInScreenshot("Nehmen", "herzliches"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                ClickAtTouchPositionWithHexa("000001cb", "0000049a"); // Bestätigen
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
