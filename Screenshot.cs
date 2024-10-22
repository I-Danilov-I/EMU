using Tesseract;

namespace EMU
{
    internal static class Screenshot
    {
        internal static void TakeScreenshot(string adbPath, string screenshotDirectory)
        {
            try
            {
                if (!Directory.Exists(screenshotDirectory))
                {
                    Directory.CreateDirectory(screenshotDirectory);
                }

                // Screenshot auf dem Emulator erstellen und auf den PC übertragen
                string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png");

                // Screenshot auf dem Emulator erstellen und speichern
                string screenshotCommand = "shell screencap -p /sdcard/screenshot.png";
                AdbCommand.ExecuteAdbCommand(adbPath, screenshotCommand);

                // Screenshot vom Emulator auf den PC übertragen
                string pullCommand = $"pull /sdcard/screenshot.png {screenshotDirectory}";
                AdbCommand.ExecuteAdbCommand(adbPath, pullCommand);

                Console.WriteLine($"Screenshot erfolgreich erstellt und gespeichert unter: {screenshotDirectory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }




        public static bool CheckTextInScreenshot(string screenshotDirectory, string textToFind)
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

                            if (text.Contains(textToFind))
                            {
                                Console.WriteLine($"Der Text '{textToFind}' wurde gefunden!");
                                return true;
                            }
                            else
                            {
                                Console.WriteLine($"Der Text '{textToFind}' wurde nicht gefunden.");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                return false;
            }
        }




    }
}
