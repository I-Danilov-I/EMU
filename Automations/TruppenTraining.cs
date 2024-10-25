namespace EMU
{
    internal class TruppenTraining : DeviceControl
    {
        GameControl GameControl = new GameControl();
        WriteLogs WriteLogs = new WriteLogs();

        private void TruppenAnzahl(int truppenAnzahl)
        {
            ClickAtTouchPositionWithHexa("0000029d", "0000052e"); // Button Truppenanzahl klicken

            // Bestehenden Text/Zahlen löschen          
            int numberOfCharactersToDelete = 5; // Anzahl der Zeichen, die gelöscht werden sollen
            for (int i = 0; i < numberOfCharactersToDelete; i++)
            {
                string deleteCommand = "shell input keyevent KEYCODE_DEL"; // Löschen taster drücken
                ExecuteAdbCommand(deleteCommand);
                Thread.Sleep(100); // Kurze Pause zwischen den Löschvorgängen
            }
       
            string adbCommand = $"shell input text {truppenAnzahl}";
            ExecuteAdbCommand(adbCommand);

            // Enter-Taste drücken           
            string enterCommand = "shell input keyevent KEYCODE_ENTER"; // Bestätigen oder Enter drücken
            ExecuteAdbCommand(enterCommand);
        }


        private void CheckResoursen()
        {
            // PÜrfe o bresursen reichen
            TakeScreenshot(GetScreenDir()); // Mache ein Screenshot
            bool reichenResursen = CheckTextInScreenshot("Erhalte mehr", "Erhalte mehr"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                WriteLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(");
                PressButtonBack();
                PressButtonBack();
                return;
            }
        }


        private void CheckeErfolg()
        {
            // Prüfe um Training erfoglreich gestartet wurde.
            TakeScreenshot(GetScreenDir()); // Mache ein Screenshot
            bool erfolg = CheckTextInScreenshot("Ausbildung", "gungen"); // Suche nach Text im Screenshot
            if (erfolg == true)
            {
                WriteLogs.LogAndConsoleWirite("Truppen Training erfogreich gestartet! ;)");
                PressButtonBack();
            }
            else
            {
                throw new Exception("Truppen Training wurde nicht gestartet.");
            }
        }


        private void CheckeObTruppeAusgebildetWerden(int truppenAnzahl)
        {
            TakeScreenshot(GetScreenDir());
            bool findOrNot = CheckTextInScreenshot("Ausbildung", "gungen");
            if (!findOrNot)
            {
                TruppenAnzahl(truppenAnzahl);
                ClickAtTouchPositionWithHexa("0000028c", "000005d8"); // Letzter Buttton: Ausbilden
                Thread.Sleep(5000);

                CheckResoursen(); // Prüfe ob genu REsursen da sind
                CheckeErfolg(); // Prüfe um Training erfoglreich gestartet wurde.
            }
            else
            {
                WriteLogs.LogAndConsoleWirite("Truppen werden bereits ausgebildet. ;)");
                PressButtonBack();
            }
        }


        internal void TrainiereInfaterie(int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nInfaterie-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");          
            GameControl.SeitenMenuOpen();
            
            ClickAtTouchPositionWithHexa("00000040", "000002ad"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            ClickAtTouchPositionWithHexa("0000028c", "000003f1"); // Button Ausbilden klicken.

            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(truppenAnzahl);
            Program.infaterieTruppenTraniert += truppenAnzahl; // Truppen adieren
        }

        internal void TrainiereLatenzTreger(int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nLatenzträger-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameControl.SeitenMenuOpen();

            ClickAtTouchPositionWithHexa("00000110", "00000304"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            ClickAtTouchPositionWithHexa("0000028c", "000003f1"); // Button Ausbilden klicken.

            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(truppenAnzahl); 
            Program.latenztregerTruppenTraniert += truppenAnzahl; // Truppen adieren
        }

        internal void TrainiereSniper(int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nSnipers-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameControl.SeitenMenuOpen();

            ClickAtTouchPositionWithHexa("0000011a", "0000036d"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            ClickAtTouchPositionWithHexa("0000028c", "000003f1"); // Button Ausbilden klicken.


            ClickAtTouchPositionWithHexa("000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(truppenAnzahl);
            Program.sniperTruppenTraniert += truppenAnzahl; // Truppen adieren
        }
    }
}
