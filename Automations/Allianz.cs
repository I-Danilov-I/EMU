namespace EMU
{
    internal class Allianz(WriteLogs writeLogs, DeviceControl deviceControl)
    {

        public void KistenAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nAllianz Kisten werden abgeholt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("0000029e", "000005fa"); // Allianz
            deviceControl.ClickAtTouchPositionWithHexa("000002af", "00000346"); // Kiste
            deviceControl.ClickAtTouchPositionWithHexa("000002fc", "000005f0"); // Alliangeschenke nehmen
            deviceControl.ClickAtTouchPositionWithHexa("0000000fa", "000001f1"); // Beutekiste
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000005e7"); // Beutekiste nehmen
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "00000100"); // Große Kiste
            deviceControl.ClickAtTouchPositionWithHexa("000001c1", "00000100"); // Große Kiste zum verlassen klicken
            deviceControl.ClickAtTouchPositionWithHexa("000001cb", "000005e7"); // Verlassen
            deviceControl.PressButtonBack();
            deviceControl.PressButtonBack();
            Program.alianzKistenCounter += 1;
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
            Program.alianzHilfeCounter += 1;
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
            Program.alianzTechnologie += 1;
            writeLogs.LogAndConsoleWirite("Allianz sagt Danke für dein Beitrag! ;)");
        }


    }
}
