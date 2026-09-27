namespace VideoGames.Settings
{
    public static class FileSettings
    {
        public static string Storage => "FileStorage";
        public static string Images => Path.Combine(Storage, "images");
        public static string Games => Path.Combine(Images, "games");
    }
}