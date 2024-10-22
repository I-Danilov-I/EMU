namespace EMU
{
    internal class ClicksOperate
    {

        public static void ClickAndHoldAndScroll(string adbPath, string startXHex, string startYHex, string endXHex, string endYHex, int holdDuration, int scrollDuration)
        {
            // Hex-Werte in Dezimalwerte umwandeln
            int startX = int.Parse(startXHex, System.Globalization.NumberStyles.HexNumber);
            int startY = int.Parse(startYHex, System.Globalization.NumberStyles.HexNumber);
            int endX = int.Parse(endXHex, System.Globalization.NumberStyles.HexNumber);
            int endY = int.Parse(endYHex, System.Globalization.NumberStyles.HexNumber);

            // Schritt 1: Klicken und Halten (Finger auf dem Bildschirm gedrückt halten)
            string adbCommandHold = $"shell input swipe {startX} {startY} {startX} {startY} {holdDuration}";
            Console.WriteLine($"ADB Command to be executed: {adbPath} {adbCommandHold}");
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommandHold);

            // Schritt 2: Ziehen (Finger auf dem Bildschirm bewegen)
            string adbCommandScroll = $"shell input swipe {startX} {startY} {endX} {endY} {scrollDuration}";
            Console.WriteLine($"ADB Command to be executed: {adbPath} {adbCommandScroll}");
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommandScroll);

            Console.WriteLine($"Scroll von ({startX}, {startY}) nach ({endX}, {endY}) in {scrollDuration} ms nach Halten für {holdDuration} ms.");
        }


        public static void ClickAtTouchPositionWithHexa(string adbPath, string hexX, string hexY)
        {
            // Konvertiere die Hexadezimalwerte in Dezimal
            int x = int.Parse(hexX, System.Globalization.NumberStyles.HexNumber);
            int y = int.Parse(hexY, System.Globalization.NumberStyles.HexNumber);

            // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
            string adbCommand = $"shell input tap {x} {y}";

            // Führe den ADB-Befehl aus
            Console.WriteLine($"ADB Command to be executed: {adbPath} {adbCommand}");
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);

            Console.WriteLine($"Klick auf Position ({x}, {y}) ausgeführt.");
        }



        internal static void ClickAtPositionWithDecimal(string adbPath, int x, int y)
        {
            string adbCommand = $"shell input tap {x} {y}";

            // Führe den ADB-Befehl aus
            Console.WriteLine($"ADB Command to be executed: {adbPath} {adbCommand}");
            AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);

            Console.WriteLine($"Klick auf Position ({x}, {y}) ausgeführt.");
        }




        // Methode zum Durchklicken eines quadratischen Bereichs um die Mitte des Bildschirms
        internal static void ClickInQuadraticArea(string adbPath, int width, int height, int offset, int step)
            {
                // Berechne die Mitte des Bildschirms
                int centerX = width / 2;
                int centerY = height / 2;

                // Berechne die Grenzen des quadratischen Bereichs
                int leftX = centerX - offset;   // Links von der Mitte
                int rightX = centerX + offset;  // Rechts von der Mitte
                int topY = centerY - offset;    // Oben von der Mitte
                int bottomY = centerY + offset; // Unten von der Mitte

                // Schleifen, um innerhalb des quadratischen Bereichs zu klicken
                for (int x = leftX; x <= rightX; x += step)  // Schleife über die X-Koordinate
                {
                    for (int y = topY; y <= bottomY; y += step) // Schleife über die Y-Koordinate
                    {
                        ClickAt(adbPath, x, y);

                        // Optional: Verzögerung einfügen, um dem Emulator Zeit zu geben, die Klicks zu verarbeiten
                        Thread.Sleep(100);  // 100 ms Verzögerung zwischen den Klicks
                    }
                }
            }

            // Hilfsmethode, um an einer bestimmten Position zu klicken
            private static void ClickAt(string adbPath, int x, int y)
            {
                // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
                string adbCommand = $"shell input tap {x} {y}";

                Console.WriteLine($"ADB Command to be executed: {adbCommand}");

                // ADB-Befehl ausführen
                AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);
            }
        
    }
}
