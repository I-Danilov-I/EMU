﻿namespace EMU
{
    internal class Jagt(Logging writeLogs, DeviceControl deviceControl)
    {
        internal void PolarTerrorStarten(int tierLevel)
        {
            writeLogs.LogAndConsoleWirite("\n\nPolar Terror wird gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.GoWelt();
            deviceControl.ClickAtTouchPositionWithHexa("00000036", "00000443"); // Such icon wählen
            deviceControl.ClickAtTouchPositionWithHexa("00000133", "0000047a"); // Polar Terror Auswahl
            TierLevel(tierLevel); // Bestienlevel Eingabe
            deviceControl.ClickAtTouchPositionWithHexa("000001be", "000005eb"); // Suche
            Thread.Sleep(1000);
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "00000261"); // Rally
            deviceControl.ClickAtTouchPositionWithHexa("000001bf", "00000419"); // Rally bestätigen // ZEitauwahl
       
            if (Program.truppenAusgleich == true)
            {
                deviceControl.ClickAtTouchPositionWithHexa("000000fa", "000005ba"); // Ausgleichen?
            }

            deviceControl.ClickAtTouchPositionWithHexa("000002b6", "000005eb"); // Einsetzen
            if (CheckAusdauer())
            {
                deviceControl.GoStadt();
                writeLogs.LogAndConsoleWirite("Polar Terror erfogreich gestartet! ;)");
            }
        }

        internal void BestienJagtStarten(int bestienLevel)
        {
            writeLogs.LogAndConsoleWirite("\n\nBestien Jagt wird gestartet...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.GoWelt();
            deviceControl.ClickAtTouchPositionWithHexa("00000036", "00000443"); // Suchicon wählen
            deviceControl.ClickAtTouchPositionWithHexa("00000061", "0000046d"); // Bestien Auswahl
            TierLevel(bestienLevel); // Bestienlevel Eingabe
            deviceControl.ClickAtTouchPositionWithHexa("000001be", "000005eb"); // Suchen Butto
            Thread.Sleep(2000);
            deviceControl.ClickAtTouchPositionWithHexa("000001c6", "000002ff"); // Angriff

            if (Program.truppenAusgleich == true)
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
            deviceControl.GoStadt();
        }


        private bool CheckAusdauer()
        {
            deviceControl.TakeScreenshot();
            bool reichenResursen = deviceControl.CheckTextInScreenshot("Ausdauer", "Gouverneur"); // Suche nach Text im Screenshot
            if (reichenResursen)
            {
                writeLogs.LogAndConsoleWirite("Resoursen reichen nicht aus :(");
                deviceControl.PressButtonBack();
                deviceControl.PressButtonBack();
                return false;
            }
            Program.beastHuntCounter++;
            writeLogs.LogAndConsoleWirite("Bestien Jagt erfogreich gestartet! ;)");
            return true;
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
