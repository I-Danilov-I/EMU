namespace EMU
{
    internal class Geheimdienst(Logging logging, DeviceControl deviceControl)
    {
        internal void StartProcess()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            logging.PrintFormatet("| GEHEIMDIENST", "   |");
            logging.LogAndConsoleWirite("---------------------------------------------------------------------------");

            GoToMisson();
            FindMission();

        }

        private void GoToMisson()
        {
            logging.PrintFormatetInSameLine("Go To Misson", "...");
            deviceControl.BackUneversal();
            deviceControl.GoWelt();
            deviceControl.ClickAtTouchPositionWithHexa("00000340", "00000437"); // Geheimmission Icon 
            logging.PrintFormatetInSameLine("Go To Misson", "Completed");
            Thread.Sleep(1000);
        }                                                                               

        private bool FindMission()
        {
            logging.PrintFormatet("Find Mission", "...");
            if (deviceControl.ClickAcrossScreenRandomly(400, 400, 200, 200, "Belohnungen", "Mission", 50)) 
            {
                logging.PrintFormatetInSameLine("Find Misson", "Completed");
                return true; 
            }
            logging.PrintFormatetInSameLine("Find Misson", "Failed");
            return false;
        }

        private void StartMisson()
        {
            logging.PrintFormatet("Start Mission", "...");
            deviceControl.ClickAtTouchPositionWithHexa("000001cd", "00000435"); // Ansehen
            Thread.Sleep(1000);
            deviceControl.ClickAtTouchPositionWithHexa("000001bc", "00000311"); // Agreifen
            if (Program.truppenAusgleich == true) { deviceControl.ClickAtTouchPositionWithHexa("000000fa", "000005ba"); } // Option: Augleich der Truppen vor dem Einsatz

            if (CheckTruppenKraft() == true)
            {
                deviceControl.ClickAtTouchPositionWithHexa("000002b6", "000005eb"); // Einsetzen
                if (CheckAusdauer() == false)
                {
                    deviceControl.BackUneversal();
                    deviceControl.GoStadt();
                    logging.PrintFormatetInSameLine("Ausdauer", "Keine Ausdauer");
                    return;
                }
                else 
                {
                    logging.PrintFormatetInSameLine("Ausdauer", "OK");
                }
            }
            deviceControl.BackUneversal();
            deviceControl.GoStadt();
        }

        internal bool CheckTruppenKraft()
        {
            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("nicht", "du") == true)
            {
                logging.PrintFormatet("Truppen Kraft", "Nicht ausreichen");
                deviceControl.PressButtonBack();
                return false;
            }
            logging.PrintFormatet("Truppen Kraft", "OK");
            return true;
        }

        private bool CheckAusdauer()
        {
            deviceControl.TakeScreenshot();
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Ausdauer", "Ausdauer"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                logging.PrintFormatet("Ausdauer", "Nicht ausreichend");
                return false;
            }
            logging.PrintFormatet("Ausdauer", "OK");
            return true;      
        }


    }
}
