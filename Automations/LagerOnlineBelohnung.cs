namespace EMU
{
    internal class LagerOnlineBelohnung
    {


        internal static void Abholen(string adbPath, string screenshotDirectory)
        {
            Console.WriteLine("\n\n___________________________________________________________________________");
            Console.WriteLine("Lager Online Belohnung wird abgeholt...");
            ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "00000017", "000002b0"); // Öffne Seitenmenü
            Thread.Sleep(5000);
            ClicksOperate.ClickAndHoldAndScroll(adbPath, "0000005b", "000003ab", "00000025", "000000b5", 300, 500); // Switsche runter
            Thread.Sleep(5000);

            Screenshot.TakeScreenshot(adbPath, screenshotDirectory);
            bool findOrNot = Screenshot.CheckTextInScreenshot(screenshotDirectory, "Belohnung");
            if (findOrNot)
            {
                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "0000003b", "000003f8"); // Wähle BEgleiter Abenteuer
                Thread.Sleep(5000);

                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce");
                Thread.Sleep(5000);

                ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, "000001c3", "000002ce");
                Thread.Sleep(5000);

                
                Console.WriteLine("Lager Online Belohnung erforgreich abgeholt!");

            }
            else
            {
                Console.WriteLine("Keine Online Belohnung verfügbar, veersuche später erneut.");
                Thread.Sleep(5000);
            }
        }


    }
}
