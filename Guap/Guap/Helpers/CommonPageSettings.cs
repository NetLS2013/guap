namespace Guap.Helpers
{
    public class CommonPageSettings
    {
        public string Title { get; set; }

        public string HeaderText { get; set; }

        public string Text { get; set; }

        public bool HasNavigation { get; set; }

        public bool HasBack { get; set; }

        public string LeftButtonText { get; set; }

        public string RightButtonText { get; set; }

        public bool IsShowCustomHeader => !HasNavigation;

    }
}