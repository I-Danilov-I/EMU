namespace EMU
{
    internal static class Makro
    {

        public static void PlayMacro(string adbPath,string macroFilePath)
        {
            if (File.Exists(macroFilePath))
            {
                string[] lines = File.ReadAllLines(macroFilePath);
                string xValue = null;
                string yValue = null;

                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("ABS_MT_POSITION_X"))
                        {
                            // Extrahiere den Hex-Wert für X
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            xValue = parts[parts.Length - 1]; // Letzter Teil enthält den Hex-Wert
                        }
                        else if (line.Contains("ABS_MT_POSITION_Y"))
                        {
                            // Extrahiere den Hex-Wert für Y
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            yValue = parts[parts.Length - 1]; // Letzter Teil enthält den Hex-Wert
                        }

                        // Wenn sowohl X als auch Y gefunden wurden, führe den Klick aus
                        if (xValue != null && yValue != null)
                        {
                            Console.WriteLine($"Klick bei X: {xValue}, Y: {yValue}");
                            ClicksOperate.ClickAtTouchPositionWithHexa(adbPath, xValue, yValue);

                            // Zurücksetzen, um den nächsten Klick zu erfassen
                            xValue = null;
                            yValue = null;
                        }
                    }
                }
                Console.WriteLine("Makro-Wiedergabe abgeschlossen.");
            }
            else
            {
                Console.WriteLine("Keine Makro-Datei für die Wiedergabe gefunden.");
            }
        }

    }
}
