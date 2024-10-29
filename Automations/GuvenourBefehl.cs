namespace EMU
{ 
    internal class GuvenourBefehl(Logging writeLogs, DeviceControl deviceControl)
    {

        public void EilauftragAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nGuvenour Befehl wird veröffetlicht...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.GoStadt();
            deviceControl.ClickAtTouchPositionWithHexa("00000346", "000004a9"); // Open
            deviceControl.ClickAtTouchPositionWithHexa("0000026e", "000001a2"); // Eilauftrag
            deviceControl.ClickAtTouchPositionWithHexa("000001e3", "0000047a"); // Erlassen
            deviceControl.ClickAtTouchPositionWithHexa("000001e3", "0000047a"); // Abholen
            deviceControl.BackUneversal();
            writeLogs.LogAndConsoleWirite("Guvenour Befehl Eilauftrag erfolgreich veröffetlicht! ;)");
        }


        public void FestlichkeitenAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nGuvenour Befehl wird veröffetlicht...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.GoStadt();
            deviceControl.ClickAtTouchPositionWithHexa("00000346", "000004a9"); // Open
            deviceControl.ClickAtTouchPositionWithHexa("00000276", "000004c4"); // Fetlichkeiten
            deviceControl.ClickAtTouchPositionWithHexa("000001e3", "0000047a"); // Erlassen
            deviceControl.BackUneversal();
            writeLogs.LogAndConsoleWirite("Guvenour Befehl Festlichkeiten erfolgreich veröffetlicht! ;)");
        }

    }
}
