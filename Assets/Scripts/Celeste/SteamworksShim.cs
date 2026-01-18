namespace Steamworks
{
    // Minimal shim for Steamworks stats/achievements used by Celeste code.
    public static class SteamUserStats
    {
        public static bool RequestCurrentStats() => false;
        public static bool RequestGlobalStats(int days) => false;
        public static bool GetAchievement(string name, out bool achieved)
        {
            achieved = false;
            return false;
        }

        public static bool SetAchievement(string name) => false;

        public static bool GetStat(string name, out int data)
        {
            data = 0;
            return false;
        }

        public static bool SetStat(string name, int data) => false;

        public static bool GetGlobalStat(string name, out long data)
        {
            data = 0;
            return false;
        }

        public static bool StoreStats() => false;
    }
}
