namespace EMU.Automations
{
    internal class Erkundung
    {
        // Private Eigenschaft für adbPath
        private string adbPath;
        private string screenshotDirectory;

        // Konstruktor mit adbPath und screenshotDirectory als Parameter
        public Erkundung(string adbPath, string screenshotDirectory)
        {
            // Initialisiere die Eigenschaft adbPath und screenshotDirectory
            this.adbPath = adbPath;
            this.screenshotDirectory = screenshotDirectory;
        }

        // Methode zum Abholen der Belohnung
        public void ErkundungAbholen()
        {
            WriteLogs.LogAndConsoleWirite("\n\nErkundung wird abgeholt...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "00000054", "000005f3"); // Erkundung

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000002cd", "00000479"); // Nehmen1
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000002cd", "00000479"); // Nehmen1  

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001cd", "00000481"); // Nehmen Bestätigen1  
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001cd", "00000481"); // Nehmen Bestätigen1  

            DeviceControl.DrueckeZurueckTaste(adbPath);
            WriteLogs.LogAndConsoleWirite("Erkundung erfogreich abgeholt! ;)");
        }

        // Methode zum Abholen der Belohnung
        public void Erkundungskampf()
        {
            WriteLogs.LogAndConsoleWirite("\n\nErkundungs kampf wird vorbereitet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "00000054", "000005f3"); // Erkundung
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001c1", "000005b7"); // Erkunden (Kampf)
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000000d8", "000005e2"); // Schneller Einsatz
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "00000296", "000005da"); // Kampf
            Thread.Sleep(45 * 1000);
            DeviceControl.DrueckeZurueckTaste(adbPath);
            DeviceControl.DrueckeZurueckTaste(adbPath);
            WriteLogs.LogAndConsoleWirite("Erkundungskapmf erfogreich durchgeführt! ;)");
        }
    }
}
