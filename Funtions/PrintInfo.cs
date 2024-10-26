namespace EMU
{
    internal class PrintInfo(WriteLogs writeLogs)
    {


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
