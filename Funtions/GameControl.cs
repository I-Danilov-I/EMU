
namespace EMU
{
    internal class GameControl
    {

        internal static void SeitenMenuOpen(string adbPath)
        {
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "00000017", "000002b0"); // Öffne Seitenmenü
        }

        internal static void SeitenMenuClose(string adbPath)
        {
            DeviceControl.ClickAtTouchPositionWithHexa(adbPath, "0000023f", "000002a6"); // Schliese das Seitenmenü
        }

        internal static void SeitenMenuScrolDown(string adbPath)
        {
            DeviceControl.ClickAndHoldAndScroll(adbPath, "0000005b", "000003ab", "00000025", "000000b5", 300, 500); // Switsche runter im Seitenmenü
        }

        internal static void OfflineErtregeAbholen(string adbPath, string screenshotDirectory)
        {
            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool offlineErtrege = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Willkommen", "Offline");
            if (offlineErtrege == true)
            {
                DeviceControl.ClickAtTouchPositionWithHexa(adbPath, hexX: "000001bf", hexY: "000004d3"); // Bestätigen Button klicken
            }

        }

    }
}
