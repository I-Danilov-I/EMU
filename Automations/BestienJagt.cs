namespace EMU
{
    internal class Jagt(WriteLogs writeLogs, DeviceControl deviceControl)
    {
        internal void BestienJagtStarten(int bestienLevel, bool ausgleichen)
        {
            writeLogs.LogAndConsoleWirite("\n\nBestien Jagt wird gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.CheckePosition();
            deviceControl.ClickAtTouchPositionWithHexa("00000036", "00000443"); // Suchicon wählen
            TierLevel(bestienLevel); // Bestienlevel Eingabe
            deviceControl.ClickAtTouchPositionWithHexa("000001be", "000005eb"); // Suchen Butto
            deviceControl.ClickAtTouchPositionWithHexa("000001c6", "000002ff"); // Angriff

            if (ausgleichen == true)
            {
                deviceControl.ClickAtTouchPositionWithHexa("000000fa", "000005ba"); // Ausgleichen?
            }

            // Prüfe Ausgangsituation
            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("scheitern", "Einsatz wird") == true)
            {
                deviceControl.PressButtonBack();
                writeLogs.LogAndConsoleWirite("Der Ausgang währe fatal gewesen, Jagt nicht gestartet. :)");
            };

            deviceControl.ClickAtTouchPositionWithHexa("000002b6", "000005eb");       
            CheckAusdauer();
            deviceControl.CheckePosition();
        }


        private void CheckAusdauer()
        {
            writeLogs.LogAndConsoleWirite("Checke ob genug Ausdauer vorhaden ist...");
            deviceControl.TakeScreenshot();
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Ausdauer", "VIP"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                writeLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(");
                deviceControl.PressButtonBack();
                deviceControl.PressButtonBack();
                writeLogs.LogAndConsoleWirite("Jagt nicht gestartet. :)");
                return;
            }
            Program.bestienJagtCount++;
            writeLogs.LogAndConsoleWirite("Es ist genug Aadauer da!");
            writeLogs.LogAndConsoleWirite("Bestien Jagt erfogreich gestartet! ;)");
        }


        private void TierLevel(int bestienLevel)
        {
            deviceControl.ClickAtTouchPositionWithHexa("000002f7", "00000522"); // Level zahl anklicken

            // Bestehenden Text/Zahlen löschen          
            int numberOfCharactersToDelete = 5; // Anzahl der Zeichen, die gelöscht werden sollen
            for (int i = 0; i < numberOfCharactersToDelete; i++)
            {
                string deleteCommand = "shell input keyevent KEYCODE_DEL"; // Löschen taster drücken
                deviceControl.ExecuteAdbCommand(deleteCommand);
                Thread.Sleep(10); // Kurze Pause zwischen den Löschvorgängen
            }

            string adbCommand = $"shell input text {bestienLevel}";
            deviceControl.ExecuteAdbCommand(adbCommand);

            // Enter-Taste drücken           
            string enterCommand = "shell input keyevent KEYCODE_ENTER"; // Bestätigen oder Enter drücken
            deviceControl.ExecuteAdbCommand(enterCommand);
        }

    }
}
