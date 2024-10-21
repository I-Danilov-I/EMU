namespace EMU
{
    internal class ClicksOperate
    {
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
            // Ausgabe der berechneten Koordinaten im event-ähnlichen Format
            Console.WriteLine($"0003 0035 {x:X}"); // ABS_MT_POSITION_X (X-Koordinate im Hex-Format)
            Console.WriteLine($"0003 0036 {y:X}"); // ABS_MT_POSITION_Y (Y-Koordinate im Hex-Format)
            Console.WriteLine("0000 0002 00000000"); // Abschluss des Events

            // Erstelle den ADB-Befehl, um auf die berechneten Koordinaten zu klicken
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
