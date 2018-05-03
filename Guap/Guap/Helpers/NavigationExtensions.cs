using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Guap.Helpers
{
    public static class NavigationExtensions
    {
        public static async Task PushPopupSingleAsync(this INavigation nav, PopupPage page, bool animated = false)
        {
            if (nav.ModalStack.Count == 0 || 
                nav.ModalStack.Last().GetType() != page.GetType())
            {
                await nav.PushPopupAsync(page, animated);
            }
        }

        public static async Task PushSingleAsync(this INavigation nav, Page page, bool animated = false)
        {
            if (nav.NavigationStack.Count == 0 ||
                nav.NavigationStack.Last().GetType() != page.GetType())
            {
                await nav.PushAsync(page, animated);
            }
        }
    }
}