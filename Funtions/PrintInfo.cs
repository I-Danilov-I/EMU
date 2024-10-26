namespace EMU
{
    internal class PrintInfo(WriteLogs writeLogs)
    {
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
            writeLogs.LogAndConsoleWirite($"{formattedLabel}{formattedValue}");

            // Setze die Konsolenfarbe zurück auf den Standard.
            Console.ResetColor();
        }



        internal void PrintSetting(string label, string value)
        {
            int labelWidth = 20; // Breite der Labels, um die Ausrichtung konsistent zu halten.

            // Label in Gelb und Wert in Grün ausgeben, in einem einzigen Write-Aufruf.
            Console.ForegroundColor = ConsoleColor.Yellow;
            string formattedLabel = label.PadRight(labelWidth); // Label wird rechts mit Leerzeichen aufgefüllt.

            Console.ForegroundColor = ConsoleColor.Green;
            string formattedValue = value;

            // Die Ausgabe erfolgt in einem einzigen Aufruf, sodass die Ausrichtung erhalten bleibt.
            writeLogs.LogAndConsoleWirite($"{formattedLabel}{formattedValue}");

            // Setzt die Konsolenfarbe zurück.
            Console.ResetColor();
        }




        internal void PrintSummary()
        {
            int labelWidth = 30; // Breite der Labels für eine konsistente Ausgabe.

            Console.ForegroundColor = ConsoleColor.Cyan;
            writeLogs.LogAndConsoleWirite("\n\n_________________________[SUMMARY OVERVIEW]_________________________________");
            Console.ResetColor();

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
            PrintCounter("Alliance Healing", Program.allianceHealingCounter, labelWidth);
            PrintCounter("Advanced Hero Recruitment", Program.advancedHeroRecruitmentCounter, labelWidth);
            PrintCounter("Epic Hero Recruitment", Program.epicHeroRecruitmentCounter, labelWidth);
            PrintCounter("Beast Hunts", Program.beastHuntCounter, labelWidth);
            PrintCounter("Life Tree", Program.lifeTreeEssenceCounter, labelWidth);
            PrintCounter("VIP Status", Program.vipStatusCounter, labelWidth);

            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            Console.ResetColor();
        }

        private void PrintCounter(string label, int value, int labelWidth)
        {
            // Label und Wert in einem einheitlichen Format ausgeben.
            Console.ForegroundColor = ConsoleColor.Yellow;
            string formattedLabel = label.PadRight(labelWidth);

            Console.ForegroundColor = ConsoleColor.Green;
            string formattedValue = value.ToString().PadLeft(5);

            writeLogs.LogAndConsoleWirite($"{formattedLabel}{formattedValue}");
            Console.ResetColor();
        }


    }
}
