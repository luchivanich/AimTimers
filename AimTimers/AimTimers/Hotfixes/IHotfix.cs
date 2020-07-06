namespace AimTimers.Hotfixes
{
    public interface IHotfix
    {
        string HotfixId { get; }
        void Apply();
    }
}
