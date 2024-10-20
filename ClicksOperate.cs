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
    }
}
