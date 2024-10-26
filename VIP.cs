
namespace EMU
{
    internal class VIP(WriteLogs writeLogs, DeviceControl deviceControl)
    {
        public void KistenAbholen()
        {
            writeLogs.LogAndConsoleWirite("\n\nVIP tägliches Login: ...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            deviceControl.ClickAtTouchPositionWithHexa("0000025b", "0000004b"); // VIP
            deviceControl.ClickAtTouchPositionWithHexa("00000308", "0000015e"); // Kiste 1x
            deviceControl.ClickAtTouchPositionWithHexa("00000308", "0000015e"); // Kiste 2x
            deviceControl.PressButtonBack();
            Program.vipCount += 1;
            writeLogs.LogAndConsoleWirite("VIP tägliches Login: Erfogreich)");
        }
    }
}
