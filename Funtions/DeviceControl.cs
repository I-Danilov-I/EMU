using OpenCvSharp;
using System.Diagnostics;
using Tesseract;

namespace EMU
{
    internal class DeviceControl
    {
        private readonly string adbPath;
        private readonly string inputDevice;
        internal readonly string packageName;

        private readonly Logging logging;


        internal DeviceControl(Logging logging)
        {

            this.logging = logging;  // Zuweisung der writeLogs-Instanz

            adbPath = Program.adbPath;
            inputDevice = Program.inputDevice;
            packageName = Program.packageName; // Paketname des Spiels

        }


        internal (int width, int height) GetResolution()
        {
            // ADB-Befehl zum Abrufen der Auflösung
            string adbCommand = "shell wm size";
            string output = ExecuteAdbCommand(adbCommand);

            if (!string.IsNullOrEmpty(output) && output.Contains("Physical size:"))
            {
                // Extrahiere die Auflösung (Breite und Höhe)
                string resolution = output.Split(':')[1].Trim();
                string[] dimensions = resolution.Split('x');
                if (dimensions.Length == 2 &&
                    int.TryParse(dimensions[0], out int width) &&
                    int.TryParse(dimensions[1], out int height))
                {
                    //printInfo.PrintSetting("Resolution", $"{width}x{height}");
                    return (width, height);
                }
            }

            // Fehlerfall
            logging.LogAndConsoleWirite("Resolution Fehler beim Abrufen der Bildschirmauflösung.");
            return (0, 0);
        }




        internal bool ClickAcrossScreenRandomly(int topMargin, int bottomMargin, int leftMargin, int rightMargin, string searchText1, string searchText2, int clickCount)
        {
            // Bildschirmauflösung abrufen
            (int screenWidth, int screenHeight) = GetResolution();

            if (screenWidth == 0 || screenHeight == 0)
            {
                logging.LogAndConsoleWirite("Auflösung konnte nicht abgerufen werden. Klickvorgang wird abgebrochen.");
                return false;
            }

            // Berechnung der Start- und Endbereiche für die X- und Y-Koordinaten
            int startX = leftMargin;
            int endX = screenWidth - rightMargin;
            int startY = topMargin;
            int endY = screenHeight - bottomMargin;

            //writeLogs.LogAndConsoleWirite("Clicking randomly across the screen...");

            // Initialisierung eines Zufallsgenerators
            Random random = new Random();

            // Führe die Klicks an zufälligen Positionen durch
            for (int i = 0; i < clickCount; i++)
            {
                // Generiere eine zufällige Position innerhalb des definierten Bereichs
                int randomX = random.Next(startX, endX);
                int randomY = random.Next(startY, endY);

                // Klick an der zufälligen Position
                ClickAt(randomX, randomY);

                // Screenshot aufnehmen und Text überprüfen
                TakeScreenshot();
                if (CheckTextInScreenshot(searchText1, searchText2) == true)
                {
                   return true;
                }
                else { }
            }

            return false;
        }

        // Hilfsmethode, um an einer bestimmten Position zu klicken
        private void ClickAt(int x, int y)
        {
            string adbCommand = $"shell input tap {x} {y}";
            ExecuteAdbCommand(adbCommand);
        }





        internal void TakeScreenshot()
        {
            try
            {
                if (!Directory.Exists(Program.screenshotDirectory))
                {
                    Directory.CreateDirectory(Program.screenshotDirectory);
                }
                string screenshotCommand = "shell screencap -p /sdcard/screenshot.png";  // Screenshot auf dem Emulator erstellen und speichern
                ExecuteAdbCommand(screenshotCommand);
                string pullCommand = $"pull /sdcard/screenshot.png {Program.screenshotDirectory}"; // Screenshot vom Emulator auf den PC übertragen
                ExecuteAdbCommand(pullCommand);
            }
            catch (Exception ex)
            {
                logging.LogAndConsoleWirite("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }


        // Hilfsmethode zur Verbesserung des Bildes
        public static string ProcessImageAndExtractText(string imagePath, string trainedDataDirectory, string language = "deu")
        {
            try
            {
                // Lade das Bild
                Mat img = Cv2.ImRead(imagePath);
                Cv2.Resize(img, img, new Size(img.Width * 2, img.Height * 2));
                Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
                Cv2.AdaptiveThreshold(img, img, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 15, 10);
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
                Cv2.MorphologyEx(img, img, MorphTypes.Open, kernel);

                // Speichere das bearbeitete Bild temporär
                string tempPath = Path.Combine(Program.screenshotDirectory, "processed_image.png");
                Cv2.ImWrite(tempPath, img);

                // OCR mit Tesseract durchführen
                using (var engine = new TesseractEngine(trainedDataDirectory, language, EngineMode.Default))
                {
                    using (var pix = Pix.LoadFromFile(tempPath))
                    using (var page = engine.Process(pix))
                    {
                        return page.GetText();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler bei der Bildverarbeitung oder Textextraktion: {ex.Message}");
            }
        }



        public bool CheckTextInScreenshot(string textToFind, string textToFind2)
        {
            try
            {
                Environment.SetEnvironmentVariable("TESSDATA_PREFIX", Program.trainedDataDirectory);

                if (!File.Exists(Program.localScreenshotPath))
                {
                    logging.LogAndConsoleWirite($"[WARNUNG] Screenshot nicht gefunden unter: {Program.localScreenshotPath}");
                    return false;
                }

                // Bild vorbereiten und Text extrahieren
                string text = ProcessImageAndExtractText(Program.localScreenshotPath, Program.trainedDataDirectory);

                // Text überprüfen
                if (text.Contains(textToFind) || text.Contains(textToFind2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logging.LogAndConsoleWirite($"[FEHLER] Ein Fehler beim Auslesen des Textes aus dem Screenshot ist aufgetreten: {ex.Message}");
                return false;
            }
        }



        internal void ScrollDown(int anzahlScroll)
        {
            int count = 0;
            while (count < anzahlScroll) 
            {
                count++;
                // Beispielkoordinaten für eine Wischgeste von oben nach unten
                int startX = 500;  // X-Koordinate für den Startpunkt des Wischens
                int startY = 1000; // Y-Koordinate für den Startpunkt des Wischens (oben)
                int endX = 500;    // X-Koordinate für den Endpunkt des Wischens (gleichbleibend)
                int endY = 300;    // Y-Koordinate für den Endpunkt des Wischens (unten)
                int duration = 300; // Dauer der Wischgeste in Millisekunden

                // ADB-Befehl zum Wischen
                string adbCommand = $"shell input swipe {startX} {startY} {endX} {endY} {duration}";
                ExecuteAdbCommand(adbCommand);
            }
            logging.LogAndConsoleWirite($"Scrollen nach unten wurde {anzahlScroll} ausgeführt.");
        }


        internal void BackUneversal()
        {
            PressButtonBack();
            PressButtonBack();
            PressButtonBack();
            PressButtonBack();
            TakeScreenshot();
            if (CheckTextInScreenshot("Spiel", "verlassen?") == true)
            {
                PressButtonBack();
            }
        }


        internal void GoWelt()
        {
            ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            Thread.Sleep(1000);
            ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
            Thread.Sleep(1000);
            ClickAtTouchPositionWithHexa("000002f1", "00000540"); // Technologieforschung wälen
            Thread.Sleep(4000);
            ClickAtTouchPositionWithHexa("0000032f", "000005fd"); // Welt / Stadt
            Thread.Sleep(4000);     
        }


        internal void GoStadt()
        {
            ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            Thread.Sleep(1000);
            ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
            Thread.Sleep(1000);
            ClickAtTouchPositionWithHexa("000002f1", "00000540"); // Technologieforschung wälen
            Thread.Sleep(4000);
        }


        internal void SeitenMenuOpen()
        {
            ClickAtTouchPositionWithHexa("00000017", "000002b0"); // Öffne Seitenmenü
        }


        internal void SeitenMenuClose()
        {
            ClickAtTouchPositionWithHexa("0000023f", "000002a6"); // Schliese das Seitenmenü
        }


        internal void SeitenMenuScrolDown()
        {
            ClickAndHoldAndScroll("0000005b", "000003ab", "00000025", "000000b5", 300, 500); // Switsche runter im Seitenmenü
        }


        internal void OfflineErtregeAbholen()
        {

            logging.LogAndConsoleWirite($"\n\nChekce Offline Erträge");
            logging.LogAndConsoleWirite("---------------------------------------------------------------------------");
            TakeScreenshot();
            bool offlineErtrege = CheckTextInScreenshot("Willkommen", "Offline");
            if (offlineErtrege == true)
            {
                ClickAtTouchPositionWithHexa("000001bf", "000004d3"); // Bestätigen Button klicken
                Program.offlineEarningsCounter++;
                logging.LogAndConsoleWirite($"Offline Erträge wurden abgeholt.");
            }
            else
            {
                logging.LogAndConsoleWirite($"Keine Offline Erträge.");
            }
        }


        internal string ExecuteAdbCommand(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = adbPath;
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

                process.WaitForExit();
                Thread.Sleep(Program.commandDelay);
                return output;
            }
            catch (Exception ex)
            {
                logging.LogAndConsoleWirite(ex.Message);
                return "";
            }
        }


        public void ClickAtTouchPositionWithHexa(string hexX, string hexY)
        {          
            int x = int.Parse(hexX, System.Globalization.NumberStyles.HexNumber);
            int y = int.Parse(hexY, System.Globalization.NumberStyles.HexNumber);

            string adbCommand = $"shell input tap {x} {y}";
            ExecuteAdbCommand(adbCommand);          
        }


        internal void PressButtonBack()
        {
            string adbCommand = "shell input keyevent 4";
            ExecuteAdbCommand(adbCommand);
        }


        public void ClickAndHoldAndScroll(string startXHex, string startYHex, string endXHex, string endYHex, int holdDuration, int scrollDuration)
        {

            // Hex-Werte in Dezimalwerte umwandeln
            int startX = int.Parse(startXHex, System.Globalization.NumberStyles.HexNumber);
            int startY = int.Parse(startYHex, System.Globalization.NumberStyles.HexNumber);
            int endX = int.Parse(endXHex, System.Globalization.NumberStyles.HexNumber);
            int endY = int.Parse(endYHex, System.Globalization.NumberStyles.HexNumber);

            // Schritt 1: Klicken und Halten (Finger auf dem Bildschirm gedrückt halten)
            string adbCommandHold = $"shell input swipe {startX} {startY} {startX} {startY} {holdDuration}";
            ExecuteAdbCommand(adbCommandHold);

            // Schritt 2: Ziehen (Finger auf dem Bildschirm bewegen)
            string adbCommandScroll = $"shell input swipe {startX} {startY} {endX} {endY} {scrollDuration}";
            ExecuteAdbCommand(adbCommandScroll);
        }


     

        // [FÜR ETWICKLEN]
        // ##################################################################
        public void TrackTouchEvents()
        {
            string command = $"shell getevent -lt {inputDevice}"; // Verwende getevent ohne -lp für Live-Daten
            logging.LogAndConsoleWirite("Starte die Erfassung von Touch-Ereignissen...");

            if (!Directory.Exists(Program.logFileFolderPath))
            {
                Directory.CreateDirectory(Program.logFileFolderPath);
            }

            string logFilePathTouchEvens = Path.Combine(Program.logFileFolderPath, "TouchEventsLogs.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePathTouchEvens, true)) // Append mode
                {
                    Process process = new Process();
                    process.StartInfo.FileName = adbPath;
                    process.StartInfo.Arguments = command;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            writer.WriteLine(args.Data);
                            logging.LogAndConsoleWirite(args.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                logging.LogAndConsoleWirite($"Touch-Ereignisse wurden in {logFilePathTouchEvens} gespeichert.");
            }
            catch (Exception ex)
            {
                logging.LogAndConsoleWirite($"Fehler bei der Erfassung der Touch-Ereignisse: {ex.Message}");
            }
        }


        public void ListRunningApps()
        {
            
            string command = "shell ps | grep u0_a";
            logging.LogAndConsoleWirite("Liste der laufenden Apps...");
            string output = ExecuteAdbCommand(command);

            if (!Directory.Exists(Program.logFileFolderPath))
            {
                Directory.CreateDirectory(Program.logFileFolderPath);
            }

            string logFilePathRunningApps = Path.Combine(Program.logFileFolderPath, "RunningAppsLogs.txt");
            using (StreamWriter writer = new StreamWriter(logFilePathRunningApps, true)) // Append mode
            {
                if (!string.IsNullOrEmpty(output))
                {
                    writer.WriteLine(output);
                }
                else
                {
                    writer.WriteLine("Es laufen derzeit keine Apps.");
                }
            }
        }



    }
}
