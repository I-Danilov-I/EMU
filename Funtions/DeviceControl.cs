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

        private readonly WriteLogs writeLogs;
        private readonly PrintInfo printInfo;


        internal DeviceControl(WriteLogs writeLogs, PrintInfo printInfo)
        {

            this.writeLogs = writeLogs;  // Zuweisung der writeLogs-Instanz
            this.printInfo = printInfo;
            adbPath = Program.adbPath;
            inputDevice = Program.inputDevice;
            packageName = Program.packageName; // Paketname des Spiels

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
            writeLogs.LogAndConsoleWirite($"Scrollen nach unten wurde {anzahlScroll} ausgeführt.");
        }


        internal void BackUneversal()
        {
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
            ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
            ClickAtTouchPositionWithHexa("000002f1", "00000540"); // Technologieforschung wälen
            ClickAtTouchPositionWithHexa("0000032f", "000005fd"); // Welt / Stadt
            Thread.Sleep(4000);     
        }


        internal void GoStadt()
        {
            ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
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

            writeLogs.LogAndConsoleWirite($"\n\nChekce Offline Erträge: ...");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            TakeScreenshot();
            bool offlineErtrege = CheckTextInScreenshot("Willkommen", "Offline");
            if (offlineErtrege == true)
            {
                ClickAtTouchPositionWithHexa("000001bf", "000004d3"); // Bestätigen Button klicken
                Program.offlineEarningsCounter++;
                writeLogs.LogAndConsoleWirite($"Offline Erträge wurden abgeholt.");
            }
            else
            {
                writeLogs.LogAndConsoleWirite($"Keine Offline Erträge.");
            }
        }


        internal void TakeScreenshot()
        {
            try
            {
                writeLogs.LogAndConsoleWirite($"Screenshot....");
                if (!Directory.Exists(Program.screenshotDirectory))
                {
                    Directory.CreateDirectory(Program.screenshotDirectory);
                    writeLogs.LogAndConsoleWirite($"Screenshot-Verzeichnis erstellt: {Program.screenshotDirectory}");
                }
                string screenshotCommand = "shell screencap -p /sdcard/screenshot.png";  // Screenshot auf dem Emulator erstellen und speichern
                ExecuteAdbCommand(screenshotCommand);
                string pullCommand = $"pull /sdcard/screenshot.png {Program.screenshotDirectory}"; // Screenshot vom Emulator auf den PC übertragen
                ExecuteAdbCommand(pullCommand);
                writeLogs.LogAndConsoleWirite($"Screenshot erfolgreich gespeichert unter: {Program.localScreenshotPath}");
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }





        public bool CheckTextInScreenshot(string textToFind, string textToFind2)
        {
            try
            {
                writeLogs.LogAndConsoleWirite($"[START] Checke Text im Screenshot....");

                // Setze die Umgebungsvariable für Tesseract
                writeLogs.LogAndConsoleWirite($"Setze TESSDATA_PREFIX auf: {Program.trainedDataDirectory}");
                Environment.SetEnvironmentVariable("TESSDATA_PREFIX", Program.trainedDataDirectory);
                writeLogs.LogAndConsoleWirite($"[END] TESSDATA_PREFIX wurde gesetzt.");

                // Überprüfe, ob die 'deu.traineddata' Datei vorhanden ist
                writeLogs.LogAndConsoleWirite($"[START] Überprüfe 'deu.traineddata' im Verzeichnis: {Program.trainedDataDirectory}");
                if (!File.Exists(Path.Combine(Program.trainedDataDirectory, "deu.traineddata")))
                {
                    writeLogs.LogAndConsoleWirite($"[WARNUNG] 'deu.traineddata' nicht gefunden im Verzeichnis: {Program.trainedDataDirectory}");
                    return false;
                }
                writeLogs.LogAndConsoleWirite($"[END] 'deu.traineddata' vorhanden.");

                // Überprüfe, ob der Screenshot existiert
                writeLogs.LogAndConsoleWirite($"[START] Überprüfe Screenshot unter: {Program.localScreenshotPath}");
                if (!File.Exists(Program.localScreenshotPath))
                {
                    writeLogs.LogAndConsoleWirite($"[WARNUNG] Screenshot nicht gefunden unter: {Program.localScreenshotPath}");
                    return false;
                }
                writeLogs.LogAndConsoleWirite($"[END] Screenshot vorhanden.");

                // OCR-Engine initialisieren
                writeLogs.LogAndConsoleWirite($"[START] Initialisiere Tesseract-OCR-Engine.");
                using (var engine = new TesseractEngine(Program.trainedDataDirectory, "deu", EngineMode.Default))
                {
                    writeLogs.LogAndConsoleWirite($"[END] Tesseract-OCR-Engine initialisiert.");

                    // Setze den Seitensegmentierungsmodus
                    writeLogs.LogAndConsoleWirite($"[START] Setze Seitensegmentierungsmodus auf 'SingleBlock'.");
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;
                    writeLogs.LogAndConsoleWirite($"[END] Seitensegmentierungsmodus gesetzt.");

                    // Lese das Bild ein
                    writeLogs.LogAndConsoleWirite($"[START] Lade Screenshot von: {Program.localScreenshotPath}");
                    using (var img = Pix.LoadFromFile(Program.localScreenshotPath))
                    {
                        writeLogs.LogAndConsoleWirite($"[END] Screenshot erfolgreich geladen.");

                        // Verarbeite das Bild mit OCR
                        writeLogs.LogAndConsoleWirite($"[START] Verarbeite Screenshot mit OCR.");
                        using (var page = engine.Process(img))
                        {
                            try
                            {
                                writeLogs.LogAndConsoleWirite($"[END] OCR-Verarbeitung gestartet.");

                                // Extrahiere den erkannten Text
                                writeLogs.LogAndConsoleWirite($"[START] Extrahiere Text aus dem Screenshot.");
                                string text = page.GetText();
                                writeLogs.LogAndConsoleWirite($"[Extrahierter Text]: {text}");
                                writeLogs.LogAndConsoleWirite($"[END] Text extrahiert.");

                                // Überprüfe, ob der erkannte Text eine der gesuchten Zeichenfolgen enthält
                                writeLogs.LogAndConsoleWirite($"[START] Überprüfe, ob der Text die gesuchten Begriffe enthält.");
                                if (text.Contains(textToFind) || text.Contains(textToFind2))
                                {
                                    writeLogs.LogAndConsoleWirite($"[END] Gesuchter Text gefunden.");
                                    return true;
                                }
                                else
                                {
                                    writeLogs.LogAndConsoleWirite($"[END] Gesuchter Text nicht gefunden.");
                                    return false;
                                }
                            }
                            catch (Exception ex)
                            {
                                writeLogs.LogAndConsoleWirite($"[FEHLER] Fehler beim Verarbeiten des Screenshots: {ex.Message}");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite($"[FEHLER] Ein Fehler beim Auslesen des Textes aus dem Screenshot ist aufgetreten: {ex.Message}");
                if (ex.InnerException != null)
                {
                    writeLogs.LogAndConsoleWirite($"[FEHLER] Innerer Fehler: {ex.InnerException.Message}");
                }
                return false;
            }
            finally
            {
                writeLogs.LogAndConsoleWirite($"[ENDE] CheckTextInScreenshot abgeschlossen.");
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
                writeLogs.LogAndConsoleWirite(ex.Message);
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
            writeLogs.LogAndConsoleWirite("Starte die Erfassung von Touch-Ereignissen...");

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
                            writeLogs.LogAndConsoleWirite(args.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                writeLogs.LogAndConsoleWirite($"Touch-Ereignisse wurden in {logFilePathTouchEvens} gespeichert.");
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite($"Fehler bei der Erfassung der Touch-Ereignisse: {ex.Message}");
            }
        }


        public void ListRunningApps()
        {
            
            string command = "shell ps | grep u0_a";
            writeLogs.LogAndConsoleWirite("Liste der laufenden Apps...");
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




        // [Aktuel nicht verwendet!]
        // ##################################################################
        internal void ClickAtPositionWithDecimal(string adbPath, int x, int y)
        {
            string adbCommand = $"shell input tap {x} {y}";
            ExecuteAdbCommand(adbCommand);
        }


        // Methode zum Durchklicken eines quadratischen Bereichs um die Mitte des Bildschirms
        internal void ClickInQuadraticArea(string adbPath, int width, int height, int offset, int step)
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
                    Thread.Sleep(100);
                }
            }
        }

        // Hilfsmethode, um an einer bestimmten Position zu klicken
        private void ClickAt(string adbPath, int x, int y)
        {
            // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
            string adbCommand = $"shell input tap {x} {y}";
            ExecuteAdbCommand(adbCommand);
        }


    }
}
