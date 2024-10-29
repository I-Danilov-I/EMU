namespace EMU
{
    internal class Logging
    {

        private string logFileFolderPath = Program.logFileFolderPath;


        internal void LogAndConsoleWirite(string inputString)
        {
            try
            {
                // Überprüfen, ob das Verzeichnis existiert
                if (!Directory.Exists(logFileFolderPath))
                {
                    Directory.CreateDirectory(logFileFolderPath);
                }

                // Ändere den Namen der lokalen Variablen, um Konflikte zu vermeiden
                string logFilePath = Path.Combine(logFileFolderPath, "Logs.txt");

                // Falls die Datei nicht existiert, erstelle sie
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close();
                }

                // Schreiben in die Datei und Konsole
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) // 'true' bedeutet, dass an die Datei angehängt wird
                {
                    if (!string.IsNullOrEmpty(inputString))
                    {
                        Console.WriteLine($"{inputString}");
                        writer.WriteLine($"{inputString}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist beim Schreiben in die Logdatei aufgetreten: {ex.Message}");
            }
        }


        internal void PrintFormatet(string label, string value)
        {
            int labelWidth = 20;
            // Setze die Farbe des Labels auf Gelb.
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Formatiere das Label, damit es rechts mit Leerzeichen aufgefüllt wird.
            string formattedLabel = label.PadRight(labelWidth);

            // Setze die Farbe des Wertes auf Grün.
            Console.ForegroundColor = ConsoleColor.Green;

            // Der Wert wird direkt übernommen, mit optionaler Ausrichtung.
            string formattedValue = value.PadLeft(labelWidth);

            // Schreibe das formatierte Label und den Wert in einem einheitlichen Format.
            LogAndConsoleWirite($"{formattedLabel}{formattedValue}");

            // Setze die Konsolenfarbe zurück auf den Standard.
            Console.ResetColor();
        }


        internal void PrintFormatetInSameLine(string label, string value)
        {
            int labelWidth = 20;

            // Setze die Farbe des Labels auf Gelb.
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Formatiere das Label, damit es rechts mit Leerzeichen aufgefüllt wird.
            string formattedLabel = label.PadRight(labelWidth);

            // Setze die Farbe des Wertes auf Grün.
            Console.ForegroundColor = ConsoleColor.Green;

            // Der Wert wird direkt übernommen, mit optionaler Ausrichtung.
            string formattedValue = value.PadLeft(labelWidth);

            // Speichere die aktuelle Cursorposition und gehe an den Anfang der Zeile.
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;
            Console.SetCursorPosition(0, currentTop);

            // Schreibe das formatierte Label und den Wert in einem einheitlichen Format.
            Console.Write($"{formattedLabel}{formattedValue}");

            // Setze den Cursor zurück an die Originalposition, falls weitere Ausgaben folgen.
            Console.SetCursorPosition(currentLeft, currentTop);

            // Setze die Konsolenfarbe zurück auf den Standard.
            Console.ResetColor();
        }



        internal void PrintSetting(string label, string value)
        {
            int labelWidth = 30; // Breite der Labels, um die Ausrichtung konsistent zu halten.

            // Label in Gelb und Wert in Grün ausgeben, in einem einzigen Write-Aufruf.
            Console.ForegroundColor = ConsoleColor.Yellow;
            string formattedLabel = label.PadRight(labelWidth); // Label wird rechts mit Leerzeichen aufgefüllt.

            Console.ForegroundColor = ConsoleColor.Green;
            string formattedValue = value;

            // Die Ausgabe erfolgt in einem einzigen Aufruf, sodass die Ausrichtung erhalten bleibt.
            LogAndConsoleWirite($"{formattedLabel}{formattedValue}");

            // Setzt die Konsolenfarbe zurück.
            Console.ResetColor();
        }


        internal void PrintSummary()
        {
            int labelWidth = 30; // Breite der Labels für eine konsistente Ausgabe.

            Console.ForegroundColor = ConsoleColor.Cyan;
            LogAndConsoleWirite("\n\n_________________________[SUMMARY OVERVIEW]_________________________________");
            Console.ResetColor();
            PrintCounter("Truppen Ausgleich", Program.truppenAusgleich, labelWidth);
            PrintCounter("Alliance Join Rally", Program.allianceAutobeitrit, labelWidth);
            PrintCounter("Reconnect Sleep Time", Program.reconnectSleepTime, labelWidth);
            PrintCounter("Command Delay", Program.commandDelay, labelWidth);
            PrintCounter("Round Count", Program.roundCount, labelWidth);
            LogAndConsoleWirite("---------------------------------------------------------------------------");

            PrintCounter("Offline Earnings", Program.offlineEarningsCounter, labelWidth);

            PrintCounter("Storage Gifts", Program.storageBonusGiftCounter, labelWidth);
            PrintCounter("Storage Stamina", Program.storageBonusStaminaCounter, labelWidth);

            PrintCounter("Infantry Units", Program.infantryUnitsTrainedCounter, labelWidth);
            PrintCounter("Latency Carrier Units", Program.latencyCarrierUnitsTrainedCounter, labelWidth);
            PrintCounter("Sniper Units", Program.sniperUnitsTrainedCounter, labelWidth);

            PrintCounter("Exploration Bonus", Program.explorationBonusCounter, labelWidth);
            PrintCounter("Exploration Battles", Program.explorationBattleCounter, labelWidth);

            PrintCounter("Alliance Chests", Program.allianceChestsCounter, labelWidth);
            PrintCounter("Alliance Help", Program.allianceHelpCounter, labelWidth);
            PrintCounter("Alliance Technology", Program.allianceTechnologyCounter, labelWidth);


            PrintCounter("Heilings", Program.healingCounter, labelWidth);

            PrintCounter("Advanced Hero Recruitment", Program.advancedHeroRecruitmentCounter, labelWidth);
            PrintCounter("Epic Hero Recruitment", Program.epicHeroRecruitmentCounter, labelWidth);

            PrintCounter("Beast Hunts", Program.beastHuntCounter, labelWidth);

            PrintCounter("Life Tree", Program.lifeTreeEssenceCounter, labelWidth);

            PrintCounter("VIP Status", Program.vipStatusCounter, labelWidth);

            PrintCounter("Arena Fights", Program.arenaFightsCounter, labelWidth);

            LogAndConsoleWirite("---------------------------------------------------------------------------");
            Console.ResetColor();
        }

        private void PrintCounter(string label, object value, int labelWidth)
        {
            // Label und Wert in einem einheitlichen Format ausgeben.
            Console.ForegroundColor = ConsoleColor.Yellow;
            string formattedLabel = label.PadRight(labelWidth);

            Console.ForegroundColor = ConsoleColor.Green;
            string formattedValue = value.ToString()!.PadLeft(5);  // Konvertiert den Wert zu einem String, egal welcher Datentyp.

            LogAndConsoleWirite($"{formattedLabel}{formattedValue}");
            Console.ResetColor();
        }


        internal void ShowSetting()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            LogAndConsoleWirite("\n[PROGRAMM START]");
            LogAndConsoleWirite("------------------------------------------------------------------------------------------------");

            PrintSetting("Program Directory: ", Program.baseDirectory);
            PrintSetting("ADB Path: ", Program.adbPath);
            PrintSetting("Input Device: ", Program.inputDevice);
            PrintSetting("Packege Name: ", Program.packageName);
            PrintSetting("Scrrenshot Directory: ", Program.screenshotDirectory);
            PrintSetting("Logfiles Directory: ", Program.logFileFolderPath);
            PrintSetting("Trained Data Dir: ", Program.trainedDataDirectory);

            LogAndConsoleWirite("------------------------------------------------------------------------------------------------");
            Console.ResetColor();
        }


    }
}
