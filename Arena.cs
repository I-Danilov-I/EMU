

namespace EMU
{
    internal class Arena(WriteLogs writeLogs ,DeviceControl deviceControl)
    {
        public void GoToArena()
        {
            deviceControl.ClickAtTouchPositionWithHexa("00000084", "0000004d"); // Bonusübersicht
            deviceControl.ClickAtTouchPositionWithHexa("000001bc", "000003a8"); // Kraft
            deviceControl.ClickAtTouchPositionWithHexa("000002f4", "000002ce"); // Truppenstäerke
            deviceControl.ClickAtTouchPositionWithHexa("000002e6", "000004ba"); // Latenzträger
            deviceControl.ClickAndHoldAndScroll("000002bd", "000004bc", " 00000027", "000000da", 300, 500);
            deviceControl.ClickAtTouchPositionWithHexa("0000027f", "0000018b"); // Dock
            Thread.Sleep(4000);
        }
    }
}
