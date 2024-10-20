using System;
using Tesseract;
using OpenCvSharp;

namespace EMU
{
    internal static class Checker
    {
        // Methode zur Texterkennung mit Bildvorverarbeitung (Vergrößerung, Weichzeichner und adaptive Schwellenwertsetzung)
        public static void CheckTextInScreenshot(string screenshotPath)
        {
            try
            {
                // Vergrößere das Bild vor der Texterkennung
                string resizedImagePath = "C:\\Users\\Anatolius\\source\\repos\\EMU\\Screens\\img.png";
                ResizeAndProcessImage(screenshotPath, resizedImagePath);

                // OCR-Engine initialisieren
                using (var engine = new TesseractEngine("C:\\Users\\Anatolius\\source\\repos\\EMU\\Data", "deu", EngineMode.Default))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock; // Setze den Seitensegmentierungsmodus
                    using (var img = Pix.LoadFromFile(resizedImagePath)) // Verwende das verarbeitete Bild
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

        // Methode zur Vergrößerung des Bildes und Umwandlung in Schwarz-Weiß
        public static void ResizeAndProcessImage(string imagePath, string outputPath)
        {
            try
            {
                // Lese das Bild im Farbmodus
                Mat img = Cv2.ImRead(imagePath, ImreadModes.Color);
                Mat resized = new Mat();

                // Vergrößere das Bild um den Faktor 2
                Cv2.Resize(img, resized, new Size(img.Width * 2, img.Height * 2));

                // Konvertiere das Bild in Graustufen
                Mat gray = new Mat();
                Cv2.CvtColor(resized, gray, ColorConversionCodes.BGR2GRAY);

                // Weichzeichner anwenden, um das Bild zu glätten
                Cv2.GaussianBlur(gray, gray, new Size(3, 3), 0);

                // Wende die adaptive Schwellenwertsetzung an
                Mat binary = new Mat();
                Cv2.AdaptiveThreshold(gray, binary, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);

                // Speichere das verarbeitete Bild
                Cv2.ImWrite(outputPath, binary);

                Console.WriteLine($"Bild wurde erfolgreich vergrößert, in Schwarz-Weiß konvertiert und gespeichert unter: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Verarbeiten des Bildes: {ex.Message}");
            }
        }
    }
}
