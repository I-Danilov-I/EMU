namespace EMU
{
    internal class Geheimdienst(WriteLogs writeLogs, DeviceControl deviceControl)
    {

        public void GoToGeheimdienst()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            writeLogs.LogAndConsoleWirite("\n\nGeheimdiesnt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.BackUneversal();
            deviceControl.GoWelt();
            deviceControl.ClickAtTouchPositionWithHexa("00000340", "00000437"); // Geheimmission Icon 
                                                                                
            int topMargin = 400; // ~2 cm Abstand vom oberen Rand
            int bottomMargin = 400; // Optional: kein Abstand vom unteren Rand
            int leftMArgin = 200; // ~2 cm Abstand vom oberen Rand
            int rigtMargin = 200; // Optional: kein Abstand vom unteren Rand
            int clickCounter = 50; // 

            // Aufruf der Klick-Methode mit diesen Margins
            bool geheimMissionFind = deviceControl.ClickAcrossScreenRandomly(topMargin, bottomMargin, leftMArgin, rigtMargin,  "Belohnungen", "Ansehen", clickCounter);
            if (geheimMissionFind == true)
            {
                deviceControl.ClickAtTouchPositionWithHexa("000001cd", "00000435"); // Ansehen
                deviceControl.ClickAtTouchPositionWithHexa("000001bc", "00000311"); // Agreifen
                
                deviceControl.TakeScreenshot();
                if (deviceControl.CheckTextInScreenshot("nicht", "du") == true)
                {
                    writeLogs.LogAndConsoleWirite("Diese Mission hat kein Guten ausgang ich führe sie nicht aus ;)");
                    deviceControl.PressButtonBack();
                    return;
                }

                if (Program.truppenAusgleich == true)
                {
                    deviceControl.ClickAtTouchPositionWithHexa("000000fa", "000005ba"); // Ausgleichen?
                }

                deviceControl.ClickAtTouchPositionWithHexa("000002b6", "000005eb"); // Einsetzen
                CheckAusdauer();
                deviceControl.GoStadt();
                writeLogs.LogAndConsoleWirite("Operation: Geheimdienst Misson Succes!");

            }
            else { }
        }

        private bool CheckAusdauer()
        {
            writeLogs.LogAndConsoleWirite("Checke ob genug Ausdauer vorhaden ist...");
            deviceControl.TakeScreenshot();
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Ausdauer", "Ausdauer"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                writeLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(, Mission nicht gestartet.");
                return false;
            }
            writeLogs.LogAndConsoleWirite("Es ist genug Ausdauer da!");
            return true;      
        }

    }
}
