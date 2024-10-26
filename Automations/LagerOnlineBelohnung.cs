namespace EMU
{
    internal class LagerOnlineBelohnung(WriteLogs writeLogs, DeviceControl deviceControl)
    {

        internal void GeschnekAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nLager Online Belohnung wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.SeitenMenuOpen();
            deviceControl.SeitenMenuScrolDown();

            deviceControl.TakeScreenshot(); // Mache ein Screenshot
            bool findOrNot = deviceControl.CheckTextInScreenshot("Belohnung", "Belohnung"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                deviceControl.ClickAtTouchPositionWithHexa("0000003b", "000003f8"); // Wähle Online Belohnungen

                deviceControl.ClickAtTouchPositionWithHexa("000001c3", "000002ce"); // Abholen

                deviceControl.ClickAtTouchPositionWithHexa("000001c3", "000002ce"); // Bestätigen

                deviceControl.ClickAtTouchPositionWithHexa("0000023f", "000002a6"); // Schliese das Seitenmenü

                Program.storageBonusGiftCounter++;
                writeLogs.LogAndConsoleWirite($"Lager Online Belohnung erforgreich abgeholt!");
            }
            else
            {
                deviceControl.SeitenMenuClose();
                writeLogs.LogAndConsoleWirite("Keine Online Belohnung verfügbar, versuche später erneut.");
                Thread.Sleep(5000);
            }
        }


        internal void AusdauerAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nLager Online Ausdauer wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

            deviceControl.ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            deviceControl.ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick

            deviceControl.ClickAtTouchPositionWithHexa("000002f1", "00000540"); // Technologieforschung wälen
            deviceControl.ClickAtTouchPositionWithHexa("00000086", "000002ad"); // Lagerhausgebäude wählen

            deviceControl.ClickAtTouchPositionWithHexa("000001c8", "000002bfe"); // Gebäude anwählen
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "000002c6"); // Abholen 

            deviceControl.TakeScreenshot(); // Mache ein Screenshot
            bool findOrNot = deviceControl.CheckTextInScreenshot("Nehmen", "herzliches"); // Suche nach Text im Screenshot
            if (findOrNot)
            {
                deviceControl.ClickAtTouchPositionWithHexa("000001cb", "0000049a"); // Bestätigen
                Program.storageBonusStaminaCounter += 120;
                writeLogs.LogAndConsoleWirite($"Lager Online Ausdauer erforgreich abgeholt!");
            }
            else
            {
                writeLogs.LogAndConsoleWirite($"Aktuel keine Ausdauer zu verschenken.");
            }
        }


    }
}
