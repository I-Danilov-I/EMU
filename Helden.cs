namespace EMU
{
    internal class Helden(WriteLogs writeLogs, DeviceControl deviceControl)
    {

        public void HeldenRecurtieren()
        {
            writeLogs.LogAndConsoleWirite("\n\nHeld Rekurtierung gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("000000e9", "000005f8"); // Helden
            deviceControl.ClickAtTouchPositionWithHexa("0000029d", "000005df"); // Held rekrutieren

            deviceControl.ClickAtTouchPositionWithHexa("00000335", "0000022d"); // Punkte Truhe   
            deviceControl.ClickAtTouchPositionWithHexa("00000335", "0000022d"); // Punkte Truhe 

            deviceControl.ClickAtTouchPositionWithHexa("000000f1", "00000411"); // Erweiterte rekrutierung         
            deviceControl.TakeScreenshot();
            if(deviceControl.CheckTextInScreenshot("Kaufen", "500") == true)
            {
                deviceControl.PressButtonBack();
                writeLogs.LogAndConsoleWirite("Aktuell keine ErweiterteRekurtierung.");
            }

            deviceControl.ClickAtTouchPositionWithHexa("000000f6", "000005e1"); // Normale rekurtierung
            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("Kaufen", "500") == true)
            {
                deviceControl.PressButtonBack();
                writeLogs.LogAndConsoleWirite("Aktuell keine normale Rekurtierung.");
            }

            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            Program.heldenRecurt += 1;
            writeLogs.LogAndConsoleWirite("Held Rekurtierung abgeshclsosen! :)");
        }

    }
}
