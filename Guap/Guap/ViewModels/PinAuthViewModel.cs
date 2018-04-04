namespace Guap.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Guap.Annotations;
    using Guap.CustomRender.Pin;

    public class PinAuthViewModel : INotifyPropertyChanged
    {
        private readonly PinViewModel _pinViewModel;

        private bool _isInvalid;

        private string _error;

        private string _header;

        public PinViewModel PinViewModel => _pinViewModel;

        public Func<IList<char>, bool> validatorFunc { get; set; }
        public bool IsInvalid {
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

        public PinAuthViewModel()
        {
            _pinViewModel = new PinViewModel();

            _pinViewModel.Error += (object sender, EventArgs e) => { IsInvalid = true; };
            _pinViewModel.Success += (object sender, PinEventArgs e) => { IsInvalid = false; };
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