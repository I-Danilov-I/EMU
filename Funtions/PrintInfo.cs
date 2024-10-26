namespace EMU
{
    internal class PrintInfo(WriteLogs writeLogs)
    {
        internal void PrintSummary()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            writeLogs.LogAndConsoleWirite("\n\n_________________________[SUMMARY OVERVIEW]_________________________________");
            Console.ResetColor();

            PrintCounter("Offline Earnings", Program.offlineEarningsCounter);
            PrintCounter("Storage Gifts", Program.storageBonusGiftCounter);
            PrintCounter("Storage Stamina", Program.storageBonusStaminaCounter);
            PrintCounter("Infantry Units", Program.infantryUnitsTrainedCounter);
            PrintCounter("Latency Carrier Units", Program.latencyCarrierUnitsTrainedCounter);
            PrintCounter("Sniper Units", Program.sniperUnitsTrainedCounter);
            PrintCounter("Exploration Bonus", Program.explorationBonusCounter);
            PrintCounter("Exploration Battles", Program.explorationBattleCounter);
            PrintCounter("Alliance Chests", Program.allianceChestsCounter);
            PrintCounter("Alliance Help", Program.allianceHelpCounter);
            PrintCounter("Alliance Technology", Program.allianceTechnologyCounter);
            PrintCounter("Alliance Healing", Program.allianceHealingCounter);
            PrintCounter("Advanced Hero Recruitment", Program.advancedHeroRecruitmentCounter);
            PrintCounter("Epic Hero Recruitment", Program.epicHeroRecruitmentCounter);
            PrintCounter("Beast Hunts", Program.beastHuntCounter);
            PrintCounter("Life Tree", Program.lifeTreeEssenceCounter);
            PrintCounter("VIP Status", Program.vipStatusCounter);

            Console.ForegroundColor = ConsoleColor.Cyan;
            writeLogs.LogAndConsoleWirite("---------------------------------------------------------------------------");
            Console.ResetColor();
        }

        private void PrintCounter(string label, int value)
        {
            // Die Breite, die das Label einnehmen soll, damit alle Werte an der gleichen Stelle beginnen.
            int labelWidth = 30;

            // Label in Gelb und Wert in Grün ausgeben, in einem einzigen Write-Aufruf.
            Console.ForegroundColor = ConsoleColor.Yellow;
            string formattedLabel = label.PadRight(labelWidth); // Label wird rechts mit Leerzeichen aufgefüllt.

            Console.ForegroundColor = ConsoleColor.Green;
            string formattedValue = value.ToString();

            // Die Ausgabe erfolgt in einem einzigen Aufruf, sodass die Ausrichtung erhalten bleibt.
            writeLogs.LogAndConsoleWirite($"{formattedLabel}    :{formattedValue}");

            // Setzt die Konsolenfarbe zurück.
            Console.ResetColor();
        }


    }
}
