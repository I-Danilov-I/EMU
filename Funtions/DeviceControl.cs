using System.Diagnostics;
using Tesseract;

namespace EMU
{
    internal class DeviceControl
    {
        private string adbPath;
        private string inputDevice;
        private string packageName;
        private string screenshotDirectory;


        internal DeviceControl()
        {
            adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
            inputDevice = "/dev/input/event4";
            packageName = "com.gof.global";
            screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        }


        internal void TakeScreenshot(string screenshotDirectory)
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
                ExecuteAdbCommand(screenshotCommand);

                // Screenshot vom Emulator auf den PC übertragen
                string pullCommand = $"pull /sdcard/screenshot.png {screenshotDirectory}";
                ExecuteAdbCommand(pullCommand);
                //WriteLogs.LogAndConsoleWirite($"Screenshot erfolgreich erstellt und gespeichert unter: {screenshotDirectory}");
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite("Fehler beim Erstellen des Screenshots: " + ex.Message);
            }
        }


        public bool CheckTextInScreenshot(string textToFind, string textToFind2)
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


        internal string ExecuteAdbCommand(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = adbPath; // ADB-Pfad verwenden
                process.StartInfo.Arguments = command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true; // Fehlerausgabe auch protokollieren
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                // Ergebnis auslesen
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

                process.WaitForExit();
                return output;
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite(ex.Message);
                return "";
            }
        }

        internal string GetScreenDir() { return screenshotDirectory; }

        internal void ShowSetting()
        {
            WriteLogs.LogAndConsoleWirite($"ADB Path: {adbPath}");
            WriteLogs.LogAndConsoleWirite($"Inpu device: {inputDevice}");
        }


        internal void Wiederverbinden(int timeSleep)
        {
            NoxControl NoxControl = new NoxControl();
            // Prüfe um Training erfoglreich gestartet wurde.
            TakeScreenshot(screenshotDirectory); // Mache ein Screenshot
            bool erfolg = CheckTextInScreenshot("Kontankt", "Konto"); // Suche nach Text im Screenshot
            if (erfolg == true)
            {
                WriteLogs.LogAndConsoleWirite($"Akaunt wird von einem anderem Gerät verwendet. Verscuhe in {timeSleep} Min erneut.");
                StopApp();
                NoxControl.KillNoxPlayerProcess();
                Thread.Sleep(60 * 1000 * timeSleep);
            }
        }


        public void ClickAtTouchPositionWithHexa(string hexX, string hexY)
        {
            Wiederverbinden(1);
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


        internal bool IsAppRunning()
        {
            // Befehl, um zu überprüfen, ob die App läuft
            string adbCommand = $"shell pidof {packageName}";
            string result = ExecuteAdbCommand(adbCommand);
            return !string.IsNullOrEmpty(result); // Wenn ein Ergebnis vorliegt, läuft die App
        }


        internal void StartApp()
        {
            if (IsAppRunning() == true)
            {
                return;
            }
            string adbCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";
            ExecuteAdbCommand(adbCommand);
            WriteLogs.LogAndConsoleWirite($"App {packageName} wird gestartet.");
            Thread.Sleep(60 * 1000);
        }


        internal void StopApp()
        {
            try
            {
                // Befehl zum Stoppen der App
                string adbCommand = $"shell am force-stop {packageName}";
                ExecuteAdbCommand(adbCommand);
                Console.WriteLine($"App {packageName} wurde gestoppt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Stoppen der App: {ex.Message}");
            }
        }


        internal void RestartApp()
        {
            try
            {
                // App stoppen
                string stopCommand = $"shell am force-stop {packageName}";
                ExecuteAdbCommand(stopCommand);
                Console.WriteLine($"{packageName} wurde gestoppt.");

                // Kurze Pause, um sicherzustellen, dass die App vollständig gestoppt ist
                Thread.Sleep(2000);

                // App neu starten
                string startCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";
                ExecuteAdbCommand(startCommand);
                Console.WriteLine($"{packageName} wurde neu gestartet.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Neustarten der App {packageName}: " + ex.Message);
            }
        }


        public void TrackTouchEvents()
        {
            string command = $"shell getevent -lt {inputDevice}"; // Verwende getevent ohne -lp für Live-Daten
            string logFileFolder = Program.logFilePath;
            WriteLogs.LogAndConsoleWirite("Starte die Erfassung von Touch-Ereignissen...");

            // Erstelle das Verzeichnis, falls es nicht existiert
            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
            }

            // Pfad zur Log-Datei selbst
            string logFilePath = Path.Combine(logFileFolder, "touchEventsLog.txt");

            // Führe den ADB-Befehl aus und speichere die Ausgabe in einer Datei
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
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
                            // Schreibe die Touch-Ereignisse in die Logdatei und auf die Konsole
                            writer.WriteLine(args.Data);
                            WriteLogs.LogAndConsoleWirite(args.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                WriteLogs.LogAndConsoleWirite($"Touch-Ereignisse wurden in {logFilePath} gespeichert.");
            }
            catch (Exception ex)
            {
                WriteLogs.LogAndConsoleWirite($"Fehler bei der Erfassung der Touch-Ereignisse: {ex.Message}");
            }
        }


        public void ListRunningApps(string adbPath)
        {
            string command = "shell ps | grep u0_a";
            string logFileFolder = Program.logFilePath;
            WriteLogs.LogAndConsoleWirite("Liste der laufenden Apps...");
            string output = ExecuteAdbCommand(command);

            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
            }

            string logFilePath = Path.Combine(logFileFolder, "runningAppsLogs.txt");

            // Schreibe die Ausgabe in die Log-Datei
            using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Append mode
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




        //############################################################################
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
