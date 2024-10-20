using System;
using Tesseract;

namespace EMU
{
    internal static class Checker
    {
        // Statische Methode zur Texterkennung
        public static void CheckTextInScreenshot()
        {
            // Der Pfad zum Screenshot
            string screenshotPath = "C:\\Users\\Anatolius\\Desktop\\Unbenannt.png";

            try
            {
                // OCR-Engine initialisieren
                using (var engine = new TesseractEngine("C:\\Users\\Anatolius\\source\\repos\\EMU\\Data", "deu", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(screenshotPath))
                    {
                        using (var page = engine.Process(img))
                        {
                            // Extrahiere den erkannten Text
                            string text = page.GetText();
                            Console.WriteLine("Erkannter Text:");
                            Console.WriteLine(text);

                            // Prüfe, ob ein bestimmter Text vorhanden ist
                            string textToFind = "Anzeigen"; // Den gesuchten Text hier eingeben
                            if (text.Contains(textToFind))
                            {
                                Console.WriteLine($"Der Text '{textToFind}' wurde gefunden!");
                            }
                            else
                            {
                                Console.WriteLine($"Der Text '{textToFind}' wurde nicht gefunden.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }
    }
}
