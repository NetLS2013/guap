namespace Guap.CustomRender
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Xamarin.Forms;

    public class ToggleButton : ContentView
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<ToggleButton, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<ToggleButton, object>(p => p.CommandParameter, null);


        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create<ToggleButton, bool>(p => p.Checked, false, BindingMode.TwoWay, null, PropertyChanged);

        private static void PropertyChanged(BindableObject bindable, bool oldValue, bool newValue)
        {
            var control = (ToggleButton)bindable;
            control.SetImage(newValue);
        }

        public static readonly BindableProperty CheckedImageProperty =
            BindableProperty.Create<ToggleButton, ImageSource>(p => p.CheckedImage, null);

        public static readonly BindableProperty UnCheckedImageProperty =
            BindableProperty.Create<ToggleButton, ImageSource>(p => p.UnCheckedImage, null);

        private ICommand _toggleCommand;

        private Image _toggleImage;

        public ToggleButton()
        {
            Initialize();
        }

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        public bool Checked
        {
            get
            {
                return (bool)GetValue(CheckedProperty);
            }
            set
            {
                SetValue(CheckedProperty, value);
            }
        }

        public ImageSource CheckedImage
        {
            get
            {
                return (ImageSource)GetValue(CheckedImageProperty);
            }
            set
            {
                SetValue(CheckedImageProperty, value);
            }
        }

        public ImageSource UnCheckedImage
        {
            get
            {
                return (ImageSource)GetValue(UnCheckedImageProperty);
            }
            set
            {
                SetValue(UnCheckedImageProperty, value);
            }
        }

        public void SetImage(bool value)
        {
            if (value)
            {
                _toggleImage.Source = CheckedImage;
            }
            else
            {
                _toggleImage.Source = UnCheckedImage;
            }
        }

        public ICommand ToogleCommand
        {
            get
            {
                return _toggleCommand ?? (_toggleCommand = new Command(
                                              async () =>
                                              {
                                                  if (_toggleImage.Source == UnCheckedImage)
                                                  {
                                                      _toggleImage.Source = CheckedImage;
                                                      Checked = true;
                                                  }
                                                  else
                                                  {
                                                      _toggleImage.Source = UnCheckedImage;
                                                      Checked = false;
                                                  }

                                                  if (Command != null)
                                                  {
                                                      Command.Execute(CommandParameter);
                                                  }
                                              }));
            }
        }

        private void Initialize()
        {
            _toggleImage = new Image();

            GestureRecognizers.Add(new TapGestureRecognizer { Command = ToogleCommand });

            _toggleImage.Source = Checked ? CheckedImage : UnCheckedImage;
            Content = _toggleImage;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            _toggleImage.Source = Checked ? CheckedImage : UnCheckedImage;
            Content = _toggleImage;
        }

    }
}