namespace AssetInventory
{
    public abstract class AssertProgress
    {
        public static string CurrentMain { get; protected set; }
        public static int MainCount { get; protected set; }
        public static int MainProgress { get; protected set; }
        public static string CurrentSub { get; protected set; }
        public static int SubCount { get; protected set; }
        public static int SubProgress { get; protected set; }
        public static bool CancellationRequested { get; set; }
        public static bool Running { get; set; }
        public static Cooldown Cooldown;

        protected void ResetState(bool done)
        {
            Running = !done;
            CurrentMain = null;
            CurrentSub = null;
            MainCount = 0;
            MainProgress = 0;
            SubCount = 0;
            SubProgress = 0;

            Cooldown = new Cooldown(AssetInventory.Config.cooldownInterval, AssetInventory.Config.cooldownDuration);
            Cooldown.Enabled = AssetInventory.Config.useCooldown;
        }
    }
}