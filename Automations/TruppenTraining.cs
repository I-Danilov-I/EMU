namespace EMU
{
    internal class TruppenTraining
    {
        internal static int gesamtTruppenTraniert = 0;

        internal static void TruppenAnzahldieTraniertWerden(string adbPath, int truppenAnzahl)
        {
            // Truppen Anzahl_____________________________________________________________________
            
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "0000029d", "0000052e"); // Button Truppenanzahl klicken
            Thread.Sleep(1000);

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
            Thread.Sleep(1000);

            // Enter-Taste drücken
            
            string enterCommand = "shell input keyevent KEYCODE_ENTER"; // Bestätigen oder Enter drücken
            AdbCommand.ExecuteAdbCommand(adbPath, enterCommand);
            Thread.Sleep(1000);
        }

        internal static void CheckResoursenByTruppenAusbildung(string adbPath, string screenshotDirectory)
        {
            // PÜrfe o bresursen reichen
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool reichenResursen = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Erhalte mehr", "Erhalte mehr"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                WriteLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(");
                DeviceRemote.DrueckeZurueckTaste(adbPath);  // Zurück Taste Drücken
                DeviceRemote.DrueckeZurueckTaste(adbPath);  // Zurück Taste Drücken
                return; // Beende
            }
        }


        internal static void TrainiereInfaterie(string adbPath, string screenshotDirectory, int truppenAnzahl)
        {
            WriteLogs.LogAndConsoleWirite("\n\nInfaterie-Truppen Training wird gestartet...");
            WriteLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");          
            GameSteuerung.SeitenMenuOpen(adbPath);
            
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "00000040", "000002ad"); // Auswahl im Menü, Infaterie Truppe ausbilden klicken.
            Thread.Sleep(5000);

           
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Abholung der fertig tranierten Truppen
            Thread.Sleep(2000);

           
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "000001ba", "000002d0"); // Anklicken des Gebäudes der Infaterie Truppen
            Thread.Sleep(2000);

            
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "0000028c", "000003f1"); // Button Ausbilden klicken.
            Thread.Sleep(5000);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory); // Mache ein Screenshot
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Ausbildung", "Ausbildung"); // Suche nach Text im Screenshot
            if (!findOrNot)
            {
                TruppenAnzahldieTraniertWerden(adbPath, truppenAnzahl);
                DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "0000028c", "000005d8"); // Letzter Buttton: Ausbilden
                Thread.Sleep(5000);

                CheckResoursenByTruppenAusbildung(adbPath, screenshotDirectory);

                gesamtTruppenTraniert += truppenAnzahl; // Truppen adieren
                WriteLogs.LogAndConsoleWirite("Infaterie Truppen Training erfogreich gestartet! ;)");
                DeviceRemote.DrueckeZurueckTaste(adbPath);
            }
            else 
            {
                WriteLogs.LogAndConsoleWirite("Infaterie Truppen werden bereits ausgebildet. ;)");
                DeviceRemote.DrueckeZurueckTaste(adbPath);  // Zurück Taste Drücken
            }
        }
    }
}
