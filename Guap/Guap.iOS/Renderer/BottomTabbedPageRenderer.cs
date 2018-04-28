using System;
using System.Diagnostics;
using Guap.CustomRender;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (BottomTabbed), typeof (BottomTabbedPageRenderer))]
namespace Guap.iOS.Renderer
{
    public class BottomTabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged (VisualElementChangedEventArgs e)
        {
            base.OnElementChanged (e);

            if (e.NewElement != null)
            {
                TabBar.Translucent = false;
            }
        }
        
        public override void ViewWillAppear(bool animated)
        {
            if (TabBar?.Items != null && Element is TabbedPage tabs)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateItem(TabBar.Items[i], tabs.Children[i].Icon);
                }
            }

            base.ViewWillAppear(animated);
        }
        
        private void UpdateItem(UITabBarItem item, string icon)
        {
            if (item != null && !string.IsNullOrWhiteSpace(icon))
            {
                var newIcon = icon.Replace(".png", "_active.png");

                try
                {
                    item.Image = UIImage.FromBundle(icon);
                    item.Image = item.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

                    item.SelectedImage = UIImage.FromBundle(newIcon);
                    item.SelectedImage = item.SelectedImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

                    item.SelectedImage.AccessibilityIdentifier = icon;
                    
                    item.SetTitleTextAttributes(new UITextAttributes
                    {
                        TextColor = UIColor.White
                    }, UIControlState.Normal);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"--- Error: {e.StackTrace}");
                }
            }
        }
    }
}