namespace EMU
{
    internal class ClicksOperate
    {
        internal static void ClickToPosition(string adbPath, int with, int hight)
        {
            // Liste von relativen Koordinaten für die Klicks (z.B. 50% der Breite und 50% der Höhe)
            double[,] relativeCoordinates = new double[,]
            {
            {0.5, 0.5},  // Mitte des Bildschirms
            {0.3, 0.4},  // Links oben
            {0.7, 0.8},  // Rechts unten
            {0.4, 0.6},  // Mittig leicht versetzt
            {0.6, 0.3}   // Oben rechts
            };

            // ADB-Befehl für jeden Satz von Koordinaten ausführen
            for (int i = 0; i < relativeCoordinates.GetLength(0); i++)
            {
                // Berechne absolute Koordinaten basierend auf relativen Werten und Bildschirmauflösung
                int x = (int)(relativeCoordinates[i, 0] * with);
                int y = (int)(relativeCoordinates[i, 1] * hight);

                // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
                string adbCommand = $"shell input tap {x} {y}";

                Console.WriteLine($"ADB Command to be executed: {adbPath} {adbCommand}");

                // ADB-Befehl ausführen
                AdbCommand.ExecuteAdbCommand(adbPath, adbCommand);

                // Verzögerung von 500 Millisekunden zwischen den Klicks
                Thread.Sleep(500);
            }
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
