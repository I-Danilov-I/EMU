namespace EMU
{
    internal class TruppenTraining(Logging writeLogs, DeviceControl deviceControl)
    {

        private void TruppenAnzahl(int truppenAnzahl)
        {
            deviceControl.ClickAtTouchPositionWithHexa("0000029d", "0000052e"); // Button Truppenanzahl klicken

            // Bestehenden Text/Zahlen löschen          
            int numberOfCharactersToDelete = 5; // Anzahl der Zeichen, die gelöscht werden sollen
            for (int i = 0; i < numberOfCharactersToDelete; i++)
            {
                string deleteCommand = "shell input keyevent KEYCODE_DEL"; // Löschen taster drücken
                deviceControl.ExecuteAdbCommand(deleteCommand);
                Thread.Sleep(50); // Kurze Pause zwischen den Löschvorgängen
            }
       
            string adbCommand = $"shell input text {truppenAnzahl}";
            deviceControl.ExecuteAdbCommand(adbCommand);

            // Enter-Taste drücken           
            string enterCommand = "shell input keyevent KEYCODE_ENTER"; // Bestätigen oder Enter drücken
            deviceControl.ExecuteAdbCommand(enterCommand);
        }


        private void CheckResoursen()
        {
            writeLogs.LogAndConsoleWirite("Checke ob Resoursen ausreichen...");
            deviceControl.TakeScreenshot(); // Mache ein Screenshot
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Erhalte mehr", "Erhalte mehr"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                writeLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(");
                deviceControl.PressButtonBack();
                deviceControl.PressButtonBack();
                return;
            }
            writeLogs.LogAndConsoleWirite("Es sind genug Resorsen da! ;)");
        }


        private void CheckeErfolg()
        {
            // Prüfe um Training erfoglreich gestartet wurde.
            deviceControl.TakeScreenshot(); // Mache ein Screenshot
            bool erfolg = deviceControl.CheckTextInScreenshot("Ausbildung", "gungen"); // Suche nach Text im Screenshot
            if (erfolg == true)
            {
                writeLogs.LogAndConsoleWirite("Truppen Training erfogreich gestartet! ;)");
                deviceControl.PressButtonBack();
            }
            else
            {
                throw new Exception("Truppen Training wurde nicht gestartet.");
            }
        }


        private void CheckeObTruppeAusgebildetWerden(int truppenAnzahl)
        {
            deviceControl.TakeScreenshot();
            bool findOrNot = deviceControl.CheckTextInScreenshot("Ausbildung", "Befördert:");
            if (!findOrNot)
            {
                TruppenAnzahl(truppenAnzahl);
                deviceControl.ClickAtTouchPositionWithHexa("0000028c", "000005d8"); // Letzter Buttton: Ausbilden
                Thread.Sleep(5000);

                CheckResoursen(); // Prüfe ob genu REsursen da sind
                CheckeErfolg(); // Prüfe um Training erfoglreich gestartet wurde.
            }
            else
            {
                writeLogs.LogAndConsoleWirite("Truppen werden bereits ausgebildet oder befödert. ;)");
                deviceControl.PressButtonBack();
            }
        }


        internal void TrainiereInfaterie(int truppenAnzahl)
        {
            writeLogs.LogAndConsoleWirite("\n\nInfaterie-Truppen Training wird gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.SeitenMenuOpen();

            deviceControl.ClickAtTouchPositionWithHexa("00000040", "000002ad"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            deviceControl.ClickAtTouchPositionWithHexa("0000028c", "000003f1"); // Button Ausbilden klicken.

            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(truppenAnzahl);
            Program.infantryUnitsTrainedCounter += truppenAnzahl; // Truppen adieren
        }


        internal void TrainiereLatenzTreger(int truppenAnzahl)
        {
            writeLogs.LogAndConsoleWirite("\n\nLatenzträger-Truppen Training wird gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.SeitenMenuOpen();

            deviceControl.ClickAtTouchPositionWithHexa("00000110", "00000304"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            deviceControl.ClickAtTouchPositionWithHexa("0000028c", "000003f1"); // Button Ausbilden klicken.

            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(truppenAnzahl); 
            Program.latencyCarrierUnitsTrainedCounter += truppenAnzahl; // Truppen adieren
        }


        internal void TrainiereSniper(int truppenAnzahl)
        {
            writeLogs.LogAndConsoleWirite("\n\nSnipers-Truppen Training wird gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.SeitenMenuOpen();

            deviceControl.ClickAtTouchPositionWithHexa("0000011a", "0000036d"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            deviceControl.ClickAtTouchPositionWithHexa("0000028c", "000003f1"); // Button Ausbilden klicken.


            deviceControl.ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(truppenAnzahl);
            Program.sniperUnitsTrainedCounter += truppenAnzahl; // Truppen adieren
        }


    }
}
