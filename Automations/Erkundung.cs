namespace EMU
{
    internal class Erkundung : DeviceControl
    {
        WriteLogs WriteLogs = new WriteLogs();

        // Methode zum Abholen der Belohnung
        public void ErkundungAbholen()
        {
            WriteLogs.LogAndConsoleWirite("\n\nErkundung wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            ClickAtTouchPositionWithHexa("00000054", "000005f3"); // Erkundung

            ClickAtTouchPositionWithHexa("000002cd", "00000479"); // Nehmen1
            ClickAtTouchPositionWithHexa("000002cd", "00000479"); // Nehmen1  

            ClickAtTouchPositionWithHexa("000001cd", "00000481"); // Nehmen Bestätigen1  
            ClickAtTouchPositionWithHexa("000001cd", "00000481"); // Nehmen Bestätigen1  

            PressButtonBack();
            WriteLogs.LogAndConsoleWirite("Erkundung erfogreich abgeholt! ;)");
        }

        // Methode zum Abholen der Belohnung
        public void Erkundungskampf()
        {
            WriteLogs.LogAndConsoleWirite("\n\nErkundungs kampf wird vorbereitet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            ClickAtTouchPositionWithHexa("00000054", "000005f3"); // Erkundung
            ClickAtTouchPositionWithHexa("000001c1", "000005b7"); // Erkunden (Kampf)
            ClickAtTouchPositionWithHexa("000000d8", "000005e2"); // Schneller Einsatz
            ClickAtTouchPositionWithHexa("00000296", "000005da"); // Kampf
            Thread.Sleep(45 * 1000);
            PressButtonBack();
            PressButtonBack();
            WriteLogs.LogAndConsoleWirite("Erkundungskapmf erfogreich durchgeführt! ;)");
        }
    }
}
