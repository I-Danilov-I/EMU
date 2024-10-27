namespace EMU
{
    internal class Allianz(WriteLogs writeLogs, DeviceControl deviceControl)
    {
        public void AutobeitritAktivieren()
        {
            writeLogs.LogAndConsoleWirite("\n\nAllianz Autobeitrit re/aktivieren...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("0000029e", "000005fa"); // Allianz
            deviceControl.ClickAtTouchPositionWithHexa("000000ec", " 00000338"); // Krieg
            deviceControl.ClickAtTouchPositionWithHexa("000001c9", " 000005f3"); // Autobeitrit

            deviceControl.ClickAtTouchPositionWithHexa("000000dd", "00000558"); // Stopen
            if (Program.allianceAutobeitrit == true)
            {
                deviceControl.ClickAtTouchPositionWithHexa("0000026e", "0000054a"); // Aktivieren
                writeLogs.LogAndConsoleWirite("Allianz autobeitrit reaktiviert! :)");
            }
            else
            {
                deviceControl.ClickAtTouchPositionWithHexa("000000dd", "00000558"); // Stopen
                writeLogs.LogAndConsoleWirite("Allianz autobeitrit angehalten. :)");

            }
            deviceControl.BackUneversal();      
        }



        public void KistenAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nAllianz Kisten werden abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("0000029e", "000005fa"); // Allianz
            deviceControl.ClickAtTouchPositionWithHexa("000002af", "00000346"); // Kiste
            deviceControl.ClickAtTouchPositionWithHexa("0000029d", "000001f3"); // Alliangeschenke 
            deviceControl.ClickAtTouchPositionWithHexa("000002fc", "000005f0"); // Alliangeschenke nehmen
            deviceControl.ClickAtTouchPositionWithHexa("000002fc", "000005f0"); // Alliangeschenke nehmen2
            deviceControl.ClickAtTouchPositionWithHexa("0000000fa", "000001f1"); // Beutekiste
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000005e7"); // Beutekiste nehmen
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000005e7"); // Beutekiste nehmen
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "00000100"); // Große Kiste
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "00000100"); // Große Kiste zum verlassen klicken
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000005e7"); // Verlassen
            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            Program.allianceChestsCounter += 1;
            writeLogs.LogAndConsoleWirite("Allianz Kisten abholung beendet! :)");
        }


        public void Hilfe()
        {
            writeLogs.LogAndConsoleWirite("\n\nAllianz Hilfe wird ausgeführt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("0000029e", "000005fa"); // Allianz
            deviceControl.ClickAtTouchPositionWithHexa("00000298", "0000052f"); // Hilfe Auswahl Label
            deviceControl.ClickAtTouchPositionWithHexa("000001bf", " 000005dc"); // Allen helfen
            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            Program.allianceHelpCounter += 1;
            writeLogs.LogAndConsoleWirite("Allianz sagt Danke für deine Hilfe! ;)");
        }


        public void Technologie(int anzahlBeitrege)
        {
            writeLogs.LogAndConsoleWirite("\n\nAllianz Technologie Beitrag wird ausgeführt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("0000029e", "000005fa"); // Allianz
            deviceControl.ClickAtTouchPositionWithHexa("000002a5", "00000486"); // Technologie Label
            deviceControl.ClickAtTouchPositionWithHexa("000001c3", "0000050c"); // Eigeschaft wählen

            int counter = 0;
            while (counter < anzahlBeitrege)
            {
                counter++;
                deviceControl.ClickAtTouchPositionWithHexa("00000284", "00000505"); // Beitragen
                Thread.Sleep(500);
            }

            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            Program.allianceTechnologyCounter += 1;
            writeLogs.LogAndConsoleWirite("Allianz sagt Danke für dein Beitrag! ;)");
        }


    }
}
