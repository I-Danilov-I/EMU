using EMU;

namespace EMU
{
    internal class Geheimdienst(Logging logging, DeviceControl deviceControl)
    {

        internal void StartProcess()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            logging.PrintFormatet("\n\n[ GEHEIMDIENST ]", "");
            logging.LogAndConsoleWirite("---------------------------------------------------------------------------");

            GoToMisson();
            if (FindMission())
            {
                StartMisson();
                if (CheckErkundungOrFight()) // True == Jagt
                {
                    if (Program.truppenAusgleich == true) { deviceControl.ClickAtTouchPositionWithHexa("000000fa", "000005ba"); } // Option: Augleich der Truppen vor dem Einsatz

                    if (CheckTruppenKraft() == true)
                    {
                        deviceControl.ClickAtTouchPositionWithHexa("000002b6", "000005eb"); // Einsetzen
                        if (CheckAusdauer() == true)
                        {                   
                            Program.geheimdienstCounter++;
                            logging.PrintFormatet("Jagt", "Completed");
                          
                        }
                    }
                }
                else 
                {
                    if (CheckAusdauer() == true)
                    {
                        Erkundungskampf();
                        Program.geheimdienstCounter++;
             
                    }
             
                }
            }
            deviceControl.BackUneversal();
            deviceControl.GoStadt();

        }


        public void Erkundungskampf()
        {
            logging.PrintFormatet("Kampf", "...");
            deviceControl.ClickAtTouchPositionWithHexa("000000d8", "000005e2"); // Schneller Einsatz
            deviceControl.ClickAtTouchPositionWithHexa("00000296", "000005da"); // Kampf
            deviceControl.ClickAtTouchPositionWithHexa("0000033c", "0000042a"); // Aktiviere  Speed 2x
            deviceControl.ClickAtTouchPositionWithHexa("00000335", "000004ab"); // Aktiviere Auto Attack
            Thread.Sleep(45 * 1000);
            logging.PrintFormatet("Kampf", "Completed");
        }


        private void GoToMisson()
        {
            logging.PrintFormatet("Go To Misson", "...");
            deviceControl.BackUneversal();
            deviceControl.GoWelt();
            deviceControl.ClickAtTouchPositionWithHexa("00000340", "00000437"); // Geheimmission Icon 
            logging.PrintFormatet("Go To Misson", "Completed");
            Thread.Sleep(1000);
        }                                                                               


        private bool FindMission()
        {
            logging.PrintFormatet("Find Mission", "...");
            if (deviceControl.ClickAcrossScreenRandomly(400, 400, 100, 100, "Belohnungen", "Mission", 100)) 
            {
                logging.PrintFormatet("Find Misson", "Completed");
                return true; 
            }
            logging.PrintFormatet("Find Misson", "Failed");
            return false;
        }


        internal void StartMisson()
        {
            logging.PrintFormatet("Start Mission", "...");
            deviceControl.ClickAtTouchPositionWithHexa("000001ca", "00000472"); // Ansehen
            Thread.Sleep(4000);
            deviceControl.ClickAtTouchPositionWithHexa("000001bc", "00000311"); // Agreifen / Erkunden
            Thread.Sleep(2000);
            logging.PrintFormatet("Start Mission", "Completed");
        }

        internal bool CheckErkundungOrFight()
        {
            logging.PrintFormatet("Missons Art", "...");
            deviceControl.TakeScreenshot();
            if(deviceControl.CheckTextInScreenshot("Oberste", "Oberster"))
            {
                logging.PrintFormatet("Missons Art", "Jagt");
              
                return true;
            }
            logging.PrintFormatet("Missons Art", "Kampf");
            return false;
        }

        internal bool CheckTruppenKraft()
        {
            logging.PrintFormatet("Truppen Kraft", "...");
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
            logging.PrintFormatet("Ausdauer", "...");
            deviceControl.TakeScreenshot();
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Ausdauer", "Gouverneur"); // Suche nach Text im Screenshot
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
