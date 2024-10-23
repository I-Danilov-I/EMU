
namespace EMU
{
    internal class GameSteuerung
    {
        internal static void SeitenMenuOpen(string adbPath)
        {
            ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "00000017", "000002b0"); // Öffne Seitenmenü
            Thread.Sleep(5000);
        }

        internal static void SeitenMenuClose(string adbPath)
        {
            ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "0000023f", "000002a6"); // Schliese das Seitenmenü
            Thread.Sleep(5000);
        }

        internal static void SeitenMenuScrolDown(string adbPath)
        {
            ClicksOperate.ClickAndHoldAndScroll(adbPath, "0000005b", "000003ab", "00000025", "000000b5", 300, 500); // Switsche runter im Seitenmenü
            Thread.Sleep(5000);
        }

    }
}
