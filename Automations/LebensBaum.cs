namespace EMU
{
    internal class LebensBaum(Logging writeLogs, DeviceControl deviceControl)
    {
        public void GoToBaum()
        {
            deviceControl.ClickAtTouchPositionWithHexa("00000084", "0000004d"); // Bonusübersicht
            deviceControl.ClickAtTouchPositionWithHexa("000001bc", "000003a8"); // Kraft
            deviceControl.ClickAtTouchPositionWithHexa("000002f4", "000002ce"); // Truppenstäerke
            deviceControl.ClickAtTouchPositionWithHexa("000002e6", "000004ba"); // Latenzträger
            deviceControl.ClickAndHoldAndScroll("000002bd", "000004bc", " 00000027", "000000da", 300, 500);
            deviceControl.ClickAtTouchPositionWithHexa("0000027f", "0000018b"); // Dock
            Thread.Sleep(4000);
        }


        public void BaumBelohnungAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nLebensbaum Essens wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

            GoToBaum();

            deviceControl.ClickAtTouchPositionWithHexa("000001c3", "00000331"); // Baum anwählen
            deviceControl.ClickAtTouchPositionWithHexa("000001c3", "00000331"); // Baum (Vorsichthalber)
            deviceControl.ClickAtTouchPositionWithHexa("000002ac", "000003a6"); // Sammeln
            Thread.Sleep(2000);
            deviceControl.ClickAtTouchPositionWithHexa("0000032f", "000005e4"); // Stadt

            Program.lifeTreeEssenceCounter += 1;
            writeLogs.LogAndConsoleWirite("Lebensbaum Essens erfogreich abgeholt! ;)");
            Thread.Sleep(3000);
        }


        public void EssensVonFreundenAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nEssens von Freunden wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GoToBaum();

            // Klicke 1 von unten an
            deviceControl.ClickAtTouchPositionWithHexa("00000346", "00000083"); // Freunde wählen
            deviceControl.ScrollDown(15);
            deviceControl.ClickAtTouchPositionWithHexa("00000328", "00000522"); // Klicke letzen an
            Thread.Sleep(3000);
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000001dd"); // Abholen
            Thread.Sleep(2000);

            // Klicke 2 von unten an
            deviceControl.ClickAtTouchPositionWithHexa("00000346", "00000083"); // Freunde wählen
            deviceControl.ScrollDown(15);
            deviceControl.ClickAtTouchPositionWithHexa("0000032b", "0000047c"); // Klicke 2 von unten an
            Thread.Sleep(3000);
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000001dd"); // Abholen
            Thread.Sleep(2000);

            // Klicke 3 von unten an
            deviceControl.ClickAtTouchPositionWithHexa("00000346", "00000083"); // Freunde wählen
            deviceControl.ScrollDown(15);
            deviceControl.ClickAtTouchPositionWithHexa("00000323", "000003cb"); // Klicke 3 von unten an
            Thread.Sleep(3000);
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000001dd"); // Abholen
            Thread.Sleep(2000);

            // Klicke 4 von unten an
            deviceControl.ClickAtTouchPositionWithHexa("00000346", "00000083"); // Freunde wählen
            deviceControl.ScrollDown(15);
            deviceControl.ClickAtTouchPositionWithHexa("00000326", "0000032c"); // Klicke 4 von unten an
            Thread.Sleep(3000);
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000001dd"); // Abholen
            Thread.Sleep(2000);

            deviceControl.ClickAtTouchPositionWithHexa("00000032", "00000020"); // Zurück
            Thread.Sleep(3000);

            deviceControl.ClickAtTouchPositionWithHexa("0000032f", "000005e4"); // Stadt
            Program.lifeTreeEssenceCounter += 1;

            writeLogs.LogAndConsoleWirite("Essens von Freunden erfogreich abgeholt! ;)");
            Thread.Sleep(3000);
        }


    }
}
