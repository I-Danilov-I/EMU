namespace EMU
{
    internal class TruppenTraining
    {
        
        private static void TruppenAnzahl(string adbPath, int truppenAnzahl)
        {
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000029d", "0000052e"); // Button Truppenanzahl klicken

            // Bestehenden Text/Zahlen löschen          
            int numberOfCharactersToDelete = 5; // Anzahl der Zeichen, die gelöscht werden sollen
            for (int i = 0; i < numberOfCharactersToDelete; i++)
            {
                string deleteCommand = "shell input keyevent KEYCODE_DEL"; // Löschen taster drücken
                AdbCommand.ExecuteAdbCommand(adbPath, deleteCommand);
                Thread.Sleep(100); // Kurze Pause zwischen den Löschvorgängen
            }
       
            string adbCommand = $"shell input text {truppenAnzahl}";
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);

            // Enter-Taste drücken           
            string enterCommand = "shell input keyevent KEYCODE_ENTER"; // Bestätigen oder Enter drücken
            AdbCommand.ExecuteAdbCommand(adbPath, enterCommand);
        }


        private static void CheckResoursen(string adbPath, string screenshotDirectory)
        {
            // PÜrfe o bresursen reichen
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool reichenResursen = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Erhalte mehr", "Erhalte mehr"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                WriteLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(");
                DeviceControl.DrueckeZurueckTaste(adbPath);  // Zurück Taste Drücken
                DeviceControl.DrueckeZurueckTaste(adbPath);  // Zurück Taste Drücken
                return;
            }
        }


        private static void CheckeErfolg(string adbPath, string screenshotDirectory)
        {
            // Prüfe um Training erfoglreich gestartet wurde.
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool erfolg = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Ausbildung", "gungen"); // Suche nach Text im Screenshot
            if (erfolg == true)
            {
                WriteLogs.LogAndConsoleWirite("Truppen Training erfogreich gestartet! ;)");
                DeviceControl.DrueckeZurueckTaste(adbPath);
            }
            else
            {
                throw new Exception("Truppen Training wurde nicht gestartet.");
            }
        }


        private static void CheckeObTruppeAusgebildetWerden(string adbPath, string screenshotDirectory, int truppenAnzahl)
        {
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Ausbildung", "gungen");
            if (!findOrNot)
            {
                TruppenAnzahl(adbPath, truppenAnzahl);
                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000028c", "000005d8"); // Letzter Buttton: Ausbilden
                Thread.Sleep(5000);

                CheckResoursen(adbPath, screenshotDirectory); // Prüfe ob genu REsursen da sind
                CheckeErfolg(adbPath, screenshotDirectory); // Prüfe um Training erfoglreich gestartet wurde.
            }
            else
            {
                WriteLogs.LogAndConsoleWirite("Truppen werden bereits ausgebildet. ;)");
                DeviceControl.DrueckeZurueckTaste(adbPath);  // Zurück Taste Drücken
            }
        }


        internal static void TrainiereInfaterie(string adbPath, string screenshotDirectory, int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nInfaterie-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");          
            GameControl.SeitenMenuOpen(adbPath);
            
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "00000040", "000002ad"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000028c", "000003f1"); // Button Ausbilden klicken.

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(adbPath, screenshotDirectory, truppenAnzahl);
            Program.infaterieTruppenTraniert += truppenAnzahl; // Truppen adieren
        }

        internal static void TrainiereLatenzTreger(string adbPath, string screenshotDirectory, int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nLatenzträger-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameControl.SeitenMenuOpen(adbPath);

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "00000110", "00000304"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000028c", "000003f1"); // Button Ausbilden klicken.

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(adbPath, screenshotDirectory, truppenAnzahl); 
            Program.latenztregerTruppenTraniert += truppenAnzahl; // Truppen adieren
        }

        internal static void TrainiereSniper(string adbPath, string screenshotDirectory, int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nSnipers-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            GameControl.SeitenMenuOpen(adbPath);

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000011a", "0000036d"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.

            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000028c", "000003f1"); // Button Ausbilden klicken.


            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // !!!!!!

            CheckeObTruppeAusgebildetWerden(adbPath, screenshotDirectory, truppenAnzahl);
            Program.sniperTruppenTraniert += truppenAnzahl; // Truppen adieren
        }
    }
}
