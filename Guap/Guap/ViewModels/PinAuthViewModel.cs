using Guap.Views;

namespace Guap.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using Guap.Annotations;
    using Guap.CustomRender.Pin;
    using Guap.Helpers;

    using Xamarin.Forms;

    public class PinAuthViewModel : INotifyPropertyChanged
    {
        private readonly Page _context;
        private readonly PinViewModel _pinViewModel;

        private bool _isInvalid;

        private bool _isCustomHeader;

        private bool _isReset;

        private string _error;

        private string _header;

        public PinViewModel PinViewModel => _pinViewModel;

        public ICommand ForgotCommand => new Command(async () =>
        {
            await _context.Navigation.PushSingleAsync(new ForgotPinPage());
        });

        public Func<IList<char>, bool> validatorFunc { get; set; }
        public bool IsInvalid
        {
            get
            {
                return _isInvalid;
            }
            set
            {
                this._isInvalid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInvalid)));
            }
        }

        public bool IsCustomHeader
        {
            get
            {
                return _isCustomHeader;
            }
            set
            {
                _isCustomHeader = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCustomHeader)));
            }
        }

        public bool IsReset
        {
            get
            {
                return _isReset;
            }
            set
            {
                _isReset = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsReset)));
            }
        }

        public string Header
        {
            get
            {
                return this._header;
            }
            set
            {
                this._header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Header)));
            }
        }

        public string Error
        {
            get
            {
                return this._error;
            }
            set
            {
                this._error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
            }
        }

        public PinAuthViewModel(Page context, CommonPageSettings pageSettings, bool isReset)
        {
            _context = context;
            _pinViewModel = new PinViewModel();
            this.IsCustomHeader = pageSettings.IsShowCustomHeader;
            _pinViewModel.Error += (object sender, EventArgs e) => { IsInvalid = true; };
            _pinViewModel.Success += (object sender, PinEventArgs e) => { IsInvalid = false; };

            IsReset = isReset;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected void RaisePropertyChanged([CallerMemberName] string key = null)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(key));
            }
        }

        protected void RaisePropertiesChanged(params string[] keys)
        {
            if (keys != null)
            {
                foreach (string key in keys)
                {
                    var propertyChanged = PropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs(key));
                    }
                }
            }
        }
    }
}