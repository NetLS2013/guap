using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views
{
    using Guap.CustomRender.Pin;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinView : ContentView
    {
        private readonly ImageSource _emptyCircle;
        private readonly ImageSource _filledCircle;

        public PinView()
        {
            InitializeComponent();

            _emptyCircle = "img_circle.png";
            _filledCircle = "img_circle_filled.png";

            BindingContextChanged += (sender, e) =>
            {
                if (BindingContext is PinViewModel)
                {
                    var vm = BindingContext as PinViewModel;
                    vm.DisplayedTextUpdated += Handle_OnUpdateDisplayedText;
                    Handle_OnUpdateDisplayedText(vm, EventArgs.Empty);
                }
            };
        }

        private void Handle_OnUpdateDisplayedText(object sender, EventArgs e)
        {
            var vm = sender as PinViewModel;
            if (vm.EnteredPin != null && vm.TargetPinLength > 0)
            {
                if (circlesStackLayout.Children.Count == 0)
                {
                    for (int i = 0; i < vm.TargetPinLength; ++i)
                    {
                        circlesStackLayout.Children.Add(new Image
                        {
                            Source = _emptyCircle,
                            HeightRequest = 14,
                            WidthRequest = 14,
                            MinimumWidthRequest = 8,
                            MinimumHeightRequest = 8,
                            VerticalOptions = LayoutOptions.Center
                        }, i, 0);
                    }
                }
                else
                {
                    for (int i = 0; i < vm.EnteredPin.Count; ++i)
                    {
                        (circlesStackLayout.Children[i] as Image).Source = _filledCircle;
                    }
                    for (int i = vm.EnteredPin.Count; i < vm.TargetPinLength; ++i)
                    {
                        (circlesStackLayout.Children[i] as Image).Source = _emptyCircle;
                    }
                }
            }
        }
    }
}