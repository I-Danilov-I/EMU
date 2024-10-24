
namespace EMU
{
    internal class GameSteuerung
    {

        internal static void SeitenMenuOpen(string adbPath)
        {
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "00000017", "000002b0"); // Öffne Seitenmenü
            Thread.Sleep(5000);
        }

        internal static void SeitenMenuClose(string adbPath)
        {
            DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, "0000023f", "000002a6"); // Schliese das Seitenmenü
            Thread.Sleep(5000);
        }

        internal static void SeitenMenuScrolDown(string adbPath)
        {
            DeviceRemote.ClickAndHoldAndScroll(adbPath, "0000005b", "000003ab", "00000025", "000000b5", 300, 500); // Switsche runter im Seitenmenü
            Thread.Sleep(5000);
        }


        internal static void FirstStart(string adbPath, string screenshotDirectory)
        {
            // Offline Erträge sammeln, bestätigen drücken.
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool offlineErtrege = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Willkommen", "Offline");
            if (offlineErtrege == true)
            {
                DeviceRemote.ClickAtTouchPositionWithHexa(adbPath, hexX: "000001bf", hexY: "000004d3"); // Bestätigen Button klicken
                Thread.Sleep(5000);
            }

            // Falls ein unerwartetes Fenster auftaucht
            DeviceRemote.DrueckeZurueckTaste(adbPath);
            DeviceRemote.DrueckeZurueckTaste(adbPath);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool prufeObSpielBereit = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Spiel verlassen", "Spiel verlassen");
            if (prufeObSpielBereit == true)
            {
                DeviceRemote.DrueckeZurueckTaste(adbPath);
                Thread.Sleep(2000);
            }
        }

    }
}
