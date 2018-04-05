namespace Guap.Helpers
{
    public class CommonPageSettings
    {
        public string Title { get; set; }

        public string HeaderText { get; set; }

        public bool HasNavigation { get; set; }

        public bool HasBack { get; set; }

        public bool IsShowCustomHeader => !HasNavigation;
    }
}