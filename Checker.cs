using System;
using Tesseract;
using OpenCvSharp;

namespace EMU
{
    internal static class Checker
    {
        // Methode zur Texterkennung mit Bildvorverarbeitung (Vergrößerung, Weichzeichner und adaptive Schwellenwertsetzung)
        public static void CheckTextInScreenshot(string screenshotDirectory)
        {
            try
            {
                // Screenshot auf dem Emulator erstellen und auf den PC übertragen
                string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png");

                // OCR-Engine initialisieren
                using (var engine = new TesseractEngine("C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Data\\", "deu", EngineMode.Default))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock; // Setze den Seitensegmentierungsmodus
                    using (var img = Pix.LoadFromFile(localScreenshotPath)) // Verwende das verarbeitete Bild
                    {
                        using (var page = engine.Process(img))
                        {
                            // Extrahiere den erkannten Text
                            string text = page.GetText();
                            Console.WriteLine("Erkannter Text:");
                            Console.WriteLine(text);

                            // Prüfe, ob ein bestimmter Text vorhanden ist
                            string textToFind = "Geheimdienst-Mission"; // Den gesuchten Text hier eingeben
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
