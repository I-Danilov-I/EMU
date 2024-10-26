namespace EMU
{
    internal class Helden(WriteLogs writeLogs, DeviceControl deviceControl)
    {

        public void HeldenRekrutieren()
        {
            writeLogs.LogAndConsoleWirite("\n\nHeld Rekrutierung: Start...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("000000e9", "000005f8"); // Helden
            deviceControl.ClickAtTouchPositionWithHexa("0000029d", "000005df"); // Held rekrutieren

            deviceControl.ClickAtTouchPositionWithHexa("00000335", "0000022d"); // Punkte Truhe   
            deviceControl.ClickAtTouchPositionWithHexa("00000335", "0000022d"); // Punkte Truhe 

            deviceControl.ClickAtTouchPositionWithHexa("000000f1", "00000411"); // Erweiterte rekrutierung         
            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("Kaufen", "500") == true)
            {
                deviceControl.PressButtonBack();
                writeLogs.LogAndConsoleWirite("Erweiterte Rekrutierung: Nicht aktiv!");
            }
            else
            {
                deviceControl.PressButtonBack();
                writeLogs.LogAndConsoleWirite("Erweiterte Rekrutierung: Erfolgreich!");
                Program.heldenRekrurtErweitertCount += 1;
            }

            deviceControl.ClickAtTouchPositionWithHexa("000000f6", "000005e1"); // Normale rekurtierung
            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("Kaufen", "500") == true)
            {
                writeLogs.LogAndConsoleWirite("Epische Rekrutierung: Nicht aktiv.");
            }
            else
            {
                writeLogs.LogAndConsoleWirite("Epische Rekrutierung: Erfolgreich");
                Program.heldenRekrurtEpischCount += 1;
            }
            deviceControl.BackUneversal();
        }


    }
}
