namespace EMU
{
    internal class TruppenHeilen(WriteLogs writeLogs, DeviceControl deviceControl)
    {
        public void Heilen()
        {
            writeLogs.LogAndConsoleWirite("\n\nTruppen werden geheilt...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            deviceControl.ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
            deviceControl.ClickAtTouchPositionWithHexa("000002f1", "00000540"); // Technologieforschung wälen
            deviceControl.ClickAtTouchPositionWithHexa("0000029e", "00000209"); // Gebäude Krankenhaus wählen
            deviceControl.ClickAtTouchPositionWithHexa("00000262", "0000042a"); // Heilen Button1
            deviceControl.ClickAtTouchPositionWithHexa("000002d4", "000005dd"); // Heilen Button2

            deviceControl.TakeScreenshot();
            if (deviceControl.CheckTextInScreenshot("Keine verletzten Truppen zum Anzeigen.", "Alles ist gut!") == true)
            {
                deviceControl.PressButtonBack();
            };


            deviceControl.ClickAtTouchPositionWithHexa("000001b9", "00000334"); // 
            deviceControl.ClickAtTouchPositionWithHexa("00000262", "0000042a"); // Heilen Button1
            deviceControl.ClickAtTouchPositionWithHexa("000002d4", "000005dd"); // Hilfe bitton
            deviceControl.PressButtonBack();

            Program.heilenCounter += 1;
            writeLogs.LogAndConsoleWirite("Truppen Heilung erfogreich gestartet! ;)");
        }
    }
}
