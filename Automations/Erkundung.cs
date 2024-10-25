namespace EMU
{
    internal class Erkundung(WriteLogs writeLogs ,DeviceControl deviceControl)
    {

        public void ErkundungAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nErkundung wird abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("00000054", "000005f3"); // Erkundung

            deviceControl.ClickAtTouchPositionWithHexa("000002cd", "00000479"); // Nehmen1
            deviceControl.ClickAtTouchPositionWithHexa("000002cd", "00000479"); // Nehmen1  

            deviceControl.ClickAtTouchPositionWithHexa("000001cd", "00000481"); // Nehmen Bestätigen1  
            deviceControl.ClickAtTouchPositionWithHexa("000001cd", "00000481"); // Nehmen Bestätigen2  
            deviceControl.ClickAtTouchPositionWithHexa("000001cd", "00000481"); // Bestätigen3

            deviceControl.PressButtonBack();
            writeLogs.LogAndConsoleWirite("Erkundung erfogreich abgeholt! ;)");
        }


        public void Erkundungskampf()
        {
            writeLogs.LogAndConsoleWirite("\n\nErkundungs kampf wird durchgeführt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("00000054", "000005f3"); // Erkundung
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "000005b7"); // Erkunden (Kampf)
            deviceControl.ClickAtTouchPositionWithHexa("000000d8", "000005e2"); // Schneller Einsatz
            deviceControl.ClickAtTouchPositionWithHexa("00000296", "000005da"); // Kampf

            deviceControl.ClickAtTouchPositionWithHexa("0000033c", "0000042a"); // Aktiviere  Speed 2x
            deviceControl.ClickAtTouchPositionWithHexa("00000335", "000004ab"); // Aktiviere Auto Attack

            Thread.Sleep(45 * 1000);
            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            writeLogs.LogAndConsoleWirite("Erkundungskapmf erfogreich durchgeführt! ;)");
        }


    }
}
