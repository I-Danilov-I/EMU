namespace EMU
{
    public class NoxPlayerFinder
    {
        public static string SearchForNoxPath()
        {
            string[] possiblePaths = {
            @"C:\Program Files\Nox\bin\Nox.exe",
            @"D:\Program Files\Nox\bin\Nox.exe",
            @"E:\Nox\bin\Nox.exe",
            // Füge weitere übliche Pfade hinzu
        };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    Console.WriteLine("NoxPlayer gefunden unter: " + path);
                    return path;
                }
                else
                {
                    Console.WriteLine("NoxPlayer nicht gefunden.");
                    
                }
            }
            return null!; // Kein Pfad gefunden
        }
    }
}
