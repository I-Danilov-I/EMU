using EMU;

namespace EMU
{
    internal class Geheimdienst(Logging logging, DeviceControl deviceControl)
    {

        internal void StartProcess()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            logging.PrintFormatet("[ GEHEIMDIENST ]", "");
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
                        if (CheckAusdauer() == false)
                        {
                            deviceControl.BackUneversal();
                            deviceControl.GoStadt();
                            logging.PrintFormatet("Jagt", "Completed");
                            return;
                        }
                    }

                    deviceControl.BackUneversal();
                    deviceControl.GoStadt();
                }
                else 
                {
                    Erkundungskampf();
                }
            }

            deviceControl.BackUneversal();

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
            logging.PrintFormatetInSameLine("Find Mission", "...");
            if (deviceControl.ClickAcrossScreenRandomly(400, 400, 100, 100, "Belohnungen", "Mission", 100)) 
            {
                logging.PrintFormatetInSameLine("Find Misson", "Completed");
                return true; 
            }
            logging.PrintFormatetInSameLine("Find Misson", "Failed");
            return false;
        }


        internal void StartMisson()
        {
            logging.PrintFormatetInSameLine("Start Mission", "...");
            deviceControl.ClickAtTouchPositionWithHexa("000001ca", "00000472"); // Ansehen
            Thread.Sleep(4000);
            deviceControl.ClickAtTouchPositionWithHexa("000001bc", "00000311"); // Agreifen / Erkunden
            Thread.Sleep(1000);
            logging.PrintFormatetInSameLine("Start Misson", "Completed");
        }

        internal bool CheckErkundungOrFight()
        {
            logging.PrintFormatetInSameLine("Missons Art", "...");
            deviceControl.TakeScreenshot();
            if(deviceControl.CheckTextInScreenshot("Oberste", "Oberster"))
            {
                logging.PrintFormatetInSameLine("Missons Art", "Jagt");
              
                return true;
            }
            logging.PrintFormatetInSameLine("Missons Art", "Kampf");
            return false;
        }

        internal bool CheckTruppenKraft()
        {
            logging.PrintFormatetInSameLine("Truppen Kraft", "...");
            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("nicht", "du") == true)
            {
                logging.PrintFormatetInSameLine("Truppen Kraft", "Nicht ausreichen");
                deviceControl.PressButtonBack();
                return false;
            }
            logging.PrintFormatetInSameLine("Truppen Kraft", "OK");
            return true;
        }

        private bool CheckAusdauer()
        {
            logging.PrintFormatetInSameLine("Ausdaueer", "...");
            deviceControl.TakeScreenshot();
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Ausdauer", "Ausdauer"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                logging.PrintFormatetInSameLine("Ausdauer", "Nicht ausreichend");
                return false;
            }
            logging.PrintFormatetInSameLine("Ausdauer", "OK");
            return true;      
        }

        public void Erkundungskampf()
        {
            logging.PrintFormatetInSameLine("Kampf", "...");
            deviceControl.ClickAtTouchPositionWithHexa("00000054", "000005f3"); // Erkundung
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "000005b7"); // Erkunden (Kampf)
            deviceControl.ClickAtTouchPositionWithHexa("000000d8", "000005e2"); // Schneller Einsatz
            deviceControl.ClickAtTouchPositionWithHexa("00000296", "000005da"); // Kampf
            deviceControl.ClickAtTouchPositionWithHexa("0000033c", "0000042a"); // Aktiviere  Speed 2x
            deviceControl.ClickAtTouchPositionWithHexa("00000335", "000004ab"); // Aktiviere Auto Attack
            Thread.Sleep(45 * 1000);
            logging.PrintFormatet("Kampf", "Completed");
        }


    }
}
