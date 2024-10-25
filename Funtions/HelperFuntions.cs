namespace EMU.Funtions
{
    internal class HelperFuntions
    {
        internal static string CurrenDir(string folderName, string fileName)
        {
            string currentDir = Directory.GetCurrentDirectory().Replace("bin\\Debug\\net8.0", "");
            string currentPath = Path.Combine(currentDir, folderName, fileName);
            return currentPath;
        }
    }
}
