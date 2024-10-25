using System.Diagnostics;
using EMU.Funtions;
using Tesseract;

namespace EMU
{
    internal class DeviceControl
    {
        private readonly string adbPath;
        private readonly string inputDevice;
        private readonly string packageName;
        private readonly string screenshotDirectory;
        private int timeSleepMin;

        private readonly WriteLogs writeLogs;

        internal DeviceControl(WriteLogs writeLogs)
        {
            this.writeLogs = writeLogs;  // Zuweisung der writeLogs-Instanz

            adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
            inputDevice = "/dev/input/event4";
            packageName = "com.gof.global"; // Paketname des Spiels
            screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
            timeSleepMin = 1;
        }


        internal void CloseApp()
        {
            string adbCommand = $"shell am force-stop {packageName}";  // Befehl zum Schließen der App
            ExecuteAdbCommand(adbCommand);
            writeLogs.LogAndConsoleWirite($"Die App {packageName} wurde geschlossen.");
        }


        internal void StartNoxPlayer()
        {
            try
            {
                // Überprüfen, ob NoxPlayer bereits läuft
                Process[] processes = Process.GetProcessesByName("Nox"); // Name des Prozesses für NoxPlayer

                if (processes.Length > 0)
                {
                    //WriteLogs.LogAndConsoleWirite("NoxPlayer läuft bereits.");
                }
                else
                {
                    string noxPath = @"C:\Program Files\Nox\bin\Nox.exe"; // Pfad zu Nox
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = noxPath,
                        Arguments = "-clone:Nox_0", // Verwende dies, um eine spezifische Instanz zu starten (z.B. Nox_0)
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process.Start(startInfo);
                    writeLogs.LogAndConsoleWirite("NoxPlayer wurde gestartet.");
                    Thread.Sleep(30000); // Warten, bis Nox vollständig gestartet ist
                }
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite("Fehler beim Starten des NoxPlayers: " + ex.Message);
            }
        }


        internal void KillNoxPlayerProcess()
        {
            try
            {
                // Finde und beende den NoxPlayer-Prozess
                foreach (var process in Process.GetProcessesByName("Nox"))
                {
                    process.Kill();
                    Console.WriteLine("NoxPlayer wurde geschlossen.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Schließen des NoxPlayers: {ex.Message}");
            }
        }


        internal void StartADBConnection()
        {
            try
            {
                string adbDevicesOutput = ExecuteAdbCommand("devices"); // Überprüfen, ob bereits eine ADB-Verbindung besteht
                if (adbDevicesOutput.Contains("127.0.0.1:62001"))
                {
                    //WriteLogs.LogAndConsoleWirite("ADB ist bereits mit Nox verbunden.");
                }
                else
                {
                    // ADB Server neu starten und mit Nox verbinden, wenn keine Verbindung besteht
                    ExecuteAdbCommand("kill-server");
                    ExecuteAdbCommand("start-server");
                    ExecuteAdbCommand("connect 127.0.0.1:62001"); // Standard-ADB-Port von Nox
                    writeLogs.LogAndConsoleWirite("ADB-Verbindung neu hergestellt.");
                }
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite("Fehler bei der ADB-Verbindung: " + ex.Message);
            }
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
            TakeScreenshot();
            bool offlineErtrege = CheckTextInScreenshot("Willkommen", "Offline");
            if (offlineErtrege == true)
            {
                ClickAtTouchPositionWithHexa("000001bf", "000004d3"); // Bestätigen Button klicken
                Program.offlineErtregeCounter++;
                writeLogs.LogAndConsoleWirite($"Offline Erträge wurden abgeholt. {Program.offlineErtregeCounter}");
            }
        }


        internal void TakeScreenshot()
        {
            try
            {
                if (!Directory.Exists(screenshotDirectory))
                {
                    Directory.CreateDirectory(screenshotDirectory);
                }

                string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png"); // Screenshot auf dem Emulator erstellen und auf den PC übertragen
                string screenshotCommand = "shell screencap -p /sdcard/screenshot.png";  // Screenshot auf dem Emulator erstellen und speichern
                ExecuteAdbCommand(screenshotCommand);
                string pullCommand = $"pull /sdcard/screenshot.png {screenshotDirectory}"; // Screenshot vom Emulator auf den PC übertragen
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
                string localScreenshotPath = Path.Combine(screenshotDirectory, "screenshot.png");

                string trainedDataPath = HelperFuntions.CurrenDir("TrainedData", "");

                // OCR-Engine initialisieren
                using (var engine = new TesseractEngine(trainedDataPath, "deu", EngineMode.Default))
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
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite($"Ein Fehler ist aufgetreten: {ex.Message}");
                return false;
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
                Thread.Sleep(Program.geschwindigkeit);
                return output;
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite(ex.Message);
                return "";
            }
        }


        internal void ShowSetting()
        {
            writeLogs.LogAndConsoleWirite($"ADB Path: {adbPath}");
            writeLogs.LogAndConsoleWirite($"Inpu device: {inputDevice}");
        }


        internal void StableControl()
        {
            TakeScreenshot();
            bool checkAnoterDeviceAtviti = CheckTextInScreenshot("Kontankt", "Konto");
            if (checkAnoterDeviceAtviti == true)
            {
                writeLogs.LogAndConsoleWirite($"Akaunt wird von einem anderem Gerät verwendet. Verscuhe in {timeSleepMin} Min erneut.");
                CloseApp();
                KillNoxPlayerProcess();
                Thread.Sleep(60 * 1000 * timeSleepMin);
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


        internal bool IsAppRunning()
        {
            string adbCommand = $"shell pidof {packageName}"; // Befehl, um zu überprüfen, ob die App läuft
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
            writeLogs.LogAndConsoleWirite($"App {packageName} wird gestartet.");
            Thread.Sleep(60 * 1000);
        }


        internal void RestartApp()
        {
            try
            {
                writeLogs.LogAndConsoleWirite($"Die App wird neugestartet...");
                string stopCommand = $"shell am force-stop {packageName}"; // App stoppen
                ExecuteAdbCommand(stopCommand);
                Thread.Sleep(2000);
                string startCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";  // App neu starten
                ExecuteAdbCommand(startCommand);
                writeLogs.LogAndConsoleWirite($"{packageName} wurde neu gestartet.");
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite($"Fehler beim Neustarten der App {packageName}: " + ex.Message);
            }
        }





        // [FÜR ETWICKLER]
        // ##################################################################
        public void TrackTouchEvents()
        {

            string logFileFolderPath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
            string command = $"shell getevent -lt {inputDevice}"; // Verwende getevent ohne -lp für Live-Daten
            writeLogs.LogAndConsoleWirite("Starte die Erfassung von Touch-Ereignissen...");

            if (!Directory.Exists(logFileFolderPath))
            {
                Directory.CreateDirectory(logFileFolderPath);
            }

            string logFilePathTouchEvens = Path.Combine(logFileFolderPath, "TouchEventsLogs.txt");

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
            string logFileFolderPath = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Logs\\";
            string command = "shell ps | grep u0_a";
            writeLogs.LogAndConsoleWirite("Liste der laufenden Apps...");
            string output = ExecuteAdbCommand(command);

            if (!Directory.Exists(logFileFolderPath))
            {
                Directory.CreateDirectory(logFileFolderPath);
            }

            string logFilePathRunningApps = Path.Combine(logFileFolderPath, "RunningAppsLogs.txt");
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
