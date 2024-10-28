﻿using OpenCvSharp;
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
                    printInfo.PrintSetting("Resolution", $"{width}x{height}");
                    return (width, height);
                }
            }

            // Fehlerfall
            printInfo.PrintSetting("Resolution", "Fehler beim Abrufen der Bildschirmauflösung");
            return (0, 0);
        }

        internal bool ClickAcrossScreenWithMargins(int topMargin, int bottomMargin, int leftMargin, int rightMargin, int step, string searchText1, string searchText2)
        {
            // Bildschirmauflösung abrufen
            (int screenWidth, int screenHeight) = GetResolution();

            if (screenWidth == 0 || screenHeight == 0)
            {
                writeLogs.LogAndConsoleWirite("Auflösung konnte nicht abgerufen werden. Klickvorgang wird abgebrochen.");
                return false;
            }

            // Berechnung der Start- und Endbereiche für die X- und Y-Koordinaten
            int startX = leftMargin; // Startet nach dem linken Rand
            int endX = screenWidth - rightMargin; // Endet vor dem rechten Rand
            int startY = topMargin; // Startet nach dem oberen Rand
            int endY = screenHeight - bottomMargin; // Endet vor dem unteren Rand

            writeLogs.LogAndConsoleWirite("Click/Screen/Search...");
            // Schleifen, um den definierten Bereich des Bildschirms durchzuklicken
            for (int x = startX; x <= endX; x += step)  // Schleife über die X-Koordinate
            {
                for (int y = startY; y <= endY; y += step) // Schleife über die Y-Koordinate
                {
                    ClickAt(x, y);
                    //Thread.Sleep(100);
                    TakeScreenshot();
                    if (CheckTextInScreenshot(searchText1, searchText2) == true)
                    {
                        return true;
                    }
                }      
            }
            return false;
        }

        // Hilfsmethode, um an einer bestimmten Position zu klicken
        private void ClickAt(int x, int y)
        {
            // ADB-Befehl erstellen, um auf die berechneten Koordinaten zu klicken
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
                writeLogs.LogAndConsoleWirite("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }


        public bool CheckTextInScreenshot(string textToFind, string textToFind2)
        {
            try
            {
                // Setze die Umgebungsvariable für Tesseract
                Environment.SetEnvironmentVariable("TESSDATA_PREFIX", Program.trainedDataDirectory);

                if (!File.Exists(Path.Combine(Program.trainedDataDirectory, "deu.traineddata")))
                {
                    writeLogs.LogAndConsoleWirite($"[WARNUNG] 'deu.traineddata' nicht gefunden im Verzeichnis: {Program.trainedDataDirectory}");
                    return false;
                }


                if (!File.Exists(Program.localScreenshotPath))
                {
                    writeLogs.LogAndConsoleWirite($"[WARNUNG] Screenshot nicht gefunden unter: {Program.localScreenshotPath}");
                    return false;
                }


                using (var engine = new TesseractEngine(Program.trainedDataDirectory, "deu", EngineMode.Default))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;
                    using (var img = Pix.LoadFromFile(Program.localScreenshotPath))
                    {
                        using (var page = engine.Process(img))
                        {
                            try
                            {
                                string text = page.GetText();
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

            writeLogs.LogAndConsoleWirite($"\n\nChekce Offline Erträge");
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
        /*internal void ClickAtPositionWithDecimal(string adbPath, int x, int y)
        {
            string adbCommand = $"shell input tap {x} {y}";
            ExecuteAdbCommand(adbCommand);
        }
        */

    }
}
