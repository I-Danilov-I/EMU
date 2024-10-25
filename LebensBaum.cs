namespace EMU
{
    internal class LebensBaum(WriteLogs writeLogs, DeviceControl deviceControl)
    {

        public void BaumBelohnungAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nLebensbaum Essens wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.SeitenMenuOpen();
            deviceControl.SeitenMenuScrolDown();
            deviceControl.ClickAtTouchPositionWithHexa("00000125", "00000379"); // Baum des Lebens im Menü
            Thread.Sleep(3000);
            deviceControl.ClickAtTouchPositionWithHexa("000001c3", "00000331"); // Baum anwählen
            deviceControl.ClickAtTouchPositionWithHexa("000001c3", "00000331"); // Baum (Vorsichthalber)
            deviceControl.ClickAtTouchPositionWithHexa("000002ac", "000003a6"); // Sammeln
            Thread.Sleep(2000);
            deviceControl.ClickAtTouchPositionWithHexa("0000032f", "000005e4"); // Stadt

            Program.lebensbaumEssens += 1;
            writeLogs.LogAndConsoleWirite("Lebensbaum Essens erfogreich abgeholt! ;)");
        }


        public void EssensVonFreundenAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nEssens von Freunden wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.SeitenMenuOpen();
            deviceControl.SeitenMenuScrolDown();
            deviceControl.ClickAtTouchPositionWithHexa("00000125", "00000379"); // Baum des Lebens im Menü
            Thread.Sleep(5000);

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
            Program.lebensbaumEssens += 1;
            writeLogs.LogAndConsoleWirite("Essens von Freunden erfogreich abgeholt! ;)");
        }


    }
}
