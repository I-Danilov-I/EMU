﻿using Tesseract;

namespace EMU
{
    internal static class Screenshot
    {
        internal static void TakeScreenshot(string adbPath, string screenshotDirectory)
        {
            try
            {
                Thread.Sleep(3000);
                // Console.WriteLine("Screenshot wird erstellt...");
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
                Thread.Sleep(3000);
                //WriteLogs.LogAndConsoleWirite($"Screenshot erfolgreich erstellt und gespeichert unter: {screenshotDirectory}");
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }




        public static bool CheckTextInScreenshot(string screenshotDirectory, string textToFind, string textToFind2)
        {
            try
            {
                // Console.WriteLine("\nPrüfe verfügbarkeit, suche nach Text in Screenshot...");
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
                            /*
                            WriteLogs.LogAndConsoleWirite($"\n[Extrahierter Text]");
                            WriteLogs.LogAndConsoleWirite($"______________________________________________________________");
                            WriteLogs.LogAndConsoleWirite(text);
                            WriteLogs.LogAndConsoleWirite($"______________________________________________________________\n");
                            */
                            if (text.Contains(textToFind) || text.Contains(textToFind2))
                            {
                                //WriteLogs.LogAndConsoleWirite($"Der Text '{textToFind}' wurde gefunden!\n");
                                return true;
                            }
                            else
                            {
                                //WriteLogs.LogAndConsoleWirite($"Der Text '{textToFind}' wurde nicht gefunden.\n");
                                return false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite($"Ein Fehler ist aufgetreten: {ex.Message}");
                return false;
            }
        }




    }
}