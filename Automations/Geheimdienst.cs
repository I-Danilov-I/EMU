namespace EMU
{
    internal class Geheimdienst(WriteLogs writeLogs, DeviceControl deviceControl, StableControl stableControl)
    {

        public void GoToGeheimdienst()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            writeLogs.LogAndConsoleWirite("\n\nGeheimdiesnt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.GoWelt();
            deviceControl.ClickAtTouchPositionWithHexa("00000340", "00000437"); // Bonusübersicht
                                                                                // Setze die oberen und unteren Ränder in Pixel
            int topMargin = 400; // ~2 cm Abstand vom oberen Rand
            int bottomMargin = 400; // Optional: kein Abstand vom unteren Rand
            int leftMArgin = 200; // ~2 cm Abstand vom oberen Rand
            int rigtMargin = 200; // Optional: kein Abstand vom unteren Rand
            int step = 100; // Schrittweite in Pixeln zwischen den Klickpunkten

            // Aufruf der Klick-Methode mit diesen Margins
            deviceControl.ClickAcrossScreenWithMargins(topMargin, bottomMargin, leftMArgin, rigtMargin, step);
        }

    }
}
