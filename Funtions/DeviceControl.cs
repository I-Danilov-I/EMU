using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime;
using Tesseract;

namespace EMU
{
    internal class DeviceControl
    {
        private readonly string adbPath;
        private readonly string inputDevice;
        private readonly string packageName;
        private readonly string screenshotDirectory;

        private readonly WriteLogs writeLogs;
        private readonly PrintInfo printInfo;

        internal DeviceControl(WriteLogs writeLogs, PrintInfo printInfo)
        {
            this.writeLogs = writeLogs;  // Zuweisung der writeLogs-Instanz
            this.printInfo = printInfo;

            adbPath = "C:\\Program Files\\Nox\\bin\\adb.exe";
            inputDevice = "/dev/input/event4";
            packageName = "com.gof.global"; // Paketname des Spiels
            screenshotDirectory = "C:\\Users\\Anatolius\\Source\\Repos\\I-Danilov-I\\EMU\\Screens";
        }


        internal void ShowSetting()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            writeLogs.LogAndConsoleWirite("\n[PROGRAMM START]");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

            // Ausgabe der Einstellungen mit einheitlicher Ausrichtung.
            printInfo.PrintSetting("ADB Path: ", adbPath);
            printInfo.PrintSetting("Input Device: ", inputDevice);

            // Auflösung abrufen.
            string adbCommand = "shell wm size";
            string output = ExecuteAdbCommand(adbCommand);

            // Ausgabe der Auflösung, falls verfügbar.
            if (!string.IsNullOrEmpty(output))
            {
                printInfo.PrintSetting("Resolution: ", output.Trim());
            }
            else
            {
                printInfo.PrintSetting("Resolution", "Fehler beim Abrufen der Bildschirmauflösung");
            }

            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            Console.ResetColor();
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


        internal void CheckePositionAndGoWelt()
        {
            ClickAtTouchPositionWithHexa("00000081", "0000004f"); // Bonusübersicht klick
            ClickAtTouchPositionWithHexa("000001cf", "000003a6"); // Kraft klick
            ClickAtTouchPositionWithHexa("000002f1", "00000540"); // Technologieforschung wälen
            ClickAtTouchPositionWithHexa("0000032f", "000005fd"); // Welt / Stadt
            Thread.Sleep(4000);
            
        }


        internal void CheckePositionAndGoStadt()
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
            TakeScreenshot();
            bool offlineErtrege = CheckTextInScreenshot("Willkommen", "Offline");
            if (offlineErtrege == true)
            {
                ClickAtTouchPositionWithHexa("000001bf", "000004d3"); // Bestätigen Button klicken
                Program.offlineEarningsCounter++;
                writeLogs.LogAndConsoleWirite($"Offline Erträge wurden abgeholt.");
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
                Thread.Sleep(Program.commandDelay);
                return output;
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite(ex.Message);
                return "";
            }
        }



        


        internal bool IsAppResponsive()
        {
            try
            {
                // ADB-Befehl, um Informationen über den App-Status zu erhalten
                string adbCommand = "shell dumpsys activity";
                string output = ExecuteAdbCommand(adbCommand);

                // Überprüfen, ob der ANR-Status in der Ausgabe enthalten ist
                if (!string.IsNullOrEmpty(output) && output.Contains("ANR"))
                {
                    printInfo.PrintFormatet("App Zustand :", "No Action");
                    return false;
                }
                else
                {
                    printInfo.PrintFormatet("App Zustand :", "Responsiv");
                    return true;
                }
            }
            catch (Exception ex)
            {
                writeLogs.LogAndConsoleWirite($"Fehler bei der Überprüfung der App-Responsivität: {ex.Message}");
                return false;
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


        internal bool IsNetworkAvailable()
        {
            try
            {
                // Überprüft, ob irgendein Netzwerkinterface eine Verbindung hat
                bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

                if (isNetworkAvailable)
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
                printInfo.PrintFormatet("VM Network Status:", ex.Message);
                return false;
            }
        }


        internal void StableControl()
        {
            writeLogs.LogAndConsoleWirite($"\n\n[Stabilitätskontrolle]");
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");

            // Netwerz am der MAschine
            if (IsNetworkAvailable() == false)
            {
                printInfo.PrintFormatet("VM Network Status:", $"Not Network, reconnection in: {Program.reconnectSleepTime} Min");
                Thread.Sleep(Program.reconnectSleepTime);
                throw new Exception();
            }
            else 
            {
                printInfo.PrintFormatet("VM Network Status:", "Online");
            }

            // NOX
            if (IsNoxPlayerRunning() == false)
            {
                printInfo.PrintFormatet("Status Nox :", "Not Running");
                StartNoxPlayer();
                printInfo.PrintFormatet("Status Nox :", "Running");
            }
            else
            {
                printInfo.PrintFormatet("Status Nox :", "Running");
            }

            // ADB
            if (IsADBConnected() == false)
            {
                printInfo.PrintFormatet("Status ADB:", "Not Connected");
                StartADBConnection();
                printInfo.PrintFormatet("Status ADB:", "Conneted");
            }
            else
            {
                printInfo.PrintFormatet("Status ADB:", "Conneted");
            }

            if (IsNetworkNoxConnected() == false)
            {
                KillNoxPlayerProcess();
                StartNoxPlayer();
                printInfo.PrintFormatet("Network Status ADB:", "Online");
            }

            // App
            if (IsAppRunning() == false)
            {
                printInfo.PrintFormatet("Status App :", "Offline");
                StartApp();
                printInfo.PrintFormatet("Status App :", "Online");
            }
            else
            {
                printInfo.PrintFormatet("Status App :", "Online");
            }

            if (IsAppResponsive() == false)
            {
                printInfo.PrintFormatet("Status App :", "Restarting");
                RestartApp();
            }


            // App Anderes Konto
            TakeScreenshot();
            bool checkAnoterDeviceAtviti = CheckTextInScreenshot("Tipps", "Konto");
            if (checkAnoterDeviceAtviti == true)
            {
                printInfo.PrintFormatet("Status Accaunt :", $" Wird verwendet     (Verscuhe in {Program.reconnectSleepTime} Min erneut.)");
                CloseApp();
                Thread.Sleep(60 * 1000 * Program.reconnectSleepTime);
                StartApp();
                throw new Exception();
            }
            else
            {
                printInfo.PrintFormatet("Status Accaunt :", $"Frei");
            }
            writeLogs.LogAndConsoleWirite($"---------------------------------------------------------------------------");
        }





        // [APP]
        // ##################################################################
        internal bool IsAppRunning()
        {
            string adbCommand = $"shell pidof {packageName}"; // Befehl, um zu überprüfen, ob die App läuft
            string result = ExecuteAdbCommand(adbCommand);

            // Überprüfen, ob das Ergebnis nicht leer ist
            if (!string.IsNullOrEmpty(result))
            {
                return true; // Wenn ein Ergebnis vorliegt, läuft die App
            }
            else
            {        
                return false;
            }
        }


        internal void StartApp()
        {
            string adbCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";
            ExecuteAdbCommand(adbCommand);
            printInfo.PrintFormatet("Status App :", "Starting");
            Thread.Sleep(30 * 1000);
        }


        internal void RestartApp()
        {
            try
            {
                
                string stopCommand = $"shell am force-stop {packageName}"; // App stoppen
                ExecuteAdbCommand(stopCommand);
                Thread.Sleep(2000);
                string startCommand = $"shell monkey -p {packageName} -c android.intent.category.LAUNCHER 1";  // App neu starten
                ExecuteAdbCommand(startCommand);
                Thread.Sleep(60 * 1000);
            }
            catch (Exception ex)
            {
                printInfo.PrintFormatet("Status App :", ex.Message);
            }
        }


        internal void CloseApp()
        {
            if (IsAppRunning() == false)
            {
                return;
            }
            string adbCommand = $"shell am force-stop {packageName}";  // Befehl zum Schließen der App
            ExecuteAdbCommand(adbCommand);
            printInfo.PrintFormatet("Status App :", "Stoping");
        }




        // [NOX]
        // ##################################################################
        internal void StartNoxPlayer()
        {
            try
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
                printInfo.PrintFormatet("Status Nox :", "Starting");
                Thread.Sleep(30 * 1000);
                
            }
            catch (Exception ex)
            {
                printInfo.PrintFormatet("Status Nox :", ex.Message);
            }
        }

        internal bool IsNoxPlayerRunning()
        {
            try
            {
                // Überprüfen, ob ein Prozess mit dem Namen "Nox" läuft.
                Process[] processes = Process.GetProcessesByName("Nox");

                if (processes.Length > 0)
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
                printInfo.PrintFormatet("Status Nox :", ex.Message);
                return false;
            }
        }


        internal bool IsNetworkNoxConnected()
        {
            try
            {
                // ADB-Befehl, um die Erreichbarkeit von Google zu überprüfen (als Beispiel)
                string adbCommand = "shell ping -c 1 www.google.com";
                string output = ExecuteAdbCommand(adbCommand);

                // Überprüfen, ob die Ausgabe den Erfolg des Pings anzeigt
                if (!string.IsNullOrEmpty(output) && output.Contains("1 packets transmitted, 1 received"))
                {
                    printInfo.PrintFormatet("NOX Network Status :", "Online");
                    return true;
                }
                else
                {
                    printInfo.PrintFormatet("NOX Network Status :", "Offline");
                    return false;
                }
            }
            catch (Exception ex)
            {
                printInfo.PrintFormatet("NOX Network Status :", ex.Message);
                return false;
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
                    printInfo.PrintFormatet("Status Nox :", "Closed");
                }
            }
            catch (Exception ex)
            {
                printInfo.PrintFormatet("Status Nox :", ex.Message);
            }
        }
        



        // [ADB]
        // ##################################################################
        internal void StartADBConnection()
        {
            try
            {
                printInfo.PrintFormatet("Status ADB :", "Connecting");
                ExecuteAdbCommand("start-server");
                ExecuteAdbCommand("connect 127.0.0.1");
                Thread.Sleep(30 * 1000);

            }
            catch (Exception ex)
            {
                printInfo.PrintFormatet($"Status ADB :", ex.Message);
            }
        }

        internal bool IsADBConnected()
        {
            try
            {
                // ADB-Befehl, um verbundene Geräte aufzulisten
                string adbCommand = "devices";
                string output = ExecuteAdbCommand(adbCommand);

                // Überprüfe, ob in der ADB-Ausgabe eine NoxPlayer-Verbindung angezeigt wird
                if (output.Contains("emulator") || output.Contains("127.0.0.1"))
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
                printInfo.PrintFormatet("Status ADB:", ex.Message);
                return false;
            }
        }

        internal void DisconnectADB()
        {
            try
            {
                // ADB-Befehl zum Trennen der Verbindung
                string adbCommand = "disconnect 127.0.0.1"; // Standard-ADB-Port von Nox
                ExecuteAdbCommand(adbCommand);
                printInfo.PrintFormatet($"Status ADB :", $"Disconecting... [Standard-ADB-Port von Nox | {adbCommand} ]");
            }
            catch (Exception ex)
            {
                printInfo.PrintFormatet($"Status ADB :", $"Disconecting... [Standard-ADB-Port von Nox | {ex.Message} ]");
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
