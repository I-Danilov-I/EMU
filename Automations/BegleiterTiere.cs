namespace EMU.Automations
{
    internal class Begleiter(Logging writeLogs, DeviceControl deviceControl)
    {

        public void GoToBegleiter()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            writeLogs.LogAndConsoleWirite("\n\nBegleittier Abendteur...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.BackUneversal();

            deviceControl.GoStadt();

            deviceControl.ClickAtTouchPositionWithHexa("00000082", "0000047c"); // Begleiter Gebäude wählen
            deviceControl.ClickAtTouchPositionWithHexa("000002ee", "000005c8"); // Abenteuer

            int topMargin = 200; // ~2 cm Abstand vom oberen Rand
            int bottomMargin = 400; // Optional: kein Abstand vom unteren Rand
            int leftMArgin = 100; // ~2 cm Abstand vom oberen Rand
            int rigtMargin = 100; // Optional: kein Abstand vom unteren Rand
            int clickCounter = 50; // 

            while (true)
            {
                bool abendteuerFind = deviceControl.ClickAcrossScreenRandomly(topMargin, bottomMargin, leftMArgin, rigtMargin, "Begleittier Abenteuer", "Verbleibende Versuche", clickCounter);
                if (abendteuerFind == false)
                {

                    deviceControl.ClickAtTouchPositionWithHexa(" 000001d5", "000003fa"); // Abholen 
                    deviceControl.ClickAtTouchPositionWithHexa(" 000001d5", "000003fa"); // Klick zum verlassen 

                    deviceControl.ClickAtTouchPositionWithHexa("000001bb", "00000515"); // Wähle BEgleittier aus
                    deviceControl.ClickAtTouchPositionWithHexa("000001da", "000004f0"); // Start
                    deviceControl.BackUneversal();
                    writeLogs.LogAndConsoleWirite("Begleittier Abendteur erfolgreich ausgefüht! ;)");
                    return;

                }
                else { }
            }
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
