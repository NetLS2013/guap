using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Views;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class PhoneViewModel : INotifyPropertyChanged
    {
        private string _country;
        private string _phoneNumber;
        
        private readonly Page _context;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ICommand PagePhoneNumberCommand => new Command(async () => await OnPagePhoneNumber());
        public ICommand PageSuccessSignupCommand => new Command(async () => await OnPageSuccessSignup());

        public PhoneViewModel(Page context)
        {
            _context = context;
        }
        
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (_country != value)
                {
                    _country = value;
                    
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Country)));
                }
            }
        }
        
        public string PhoneNumber
        {
            get
            {
//                var resulTryParse = long.TryParse(_phoneNumber, out _);
//
//                if (resulTryParse)
//                {
//                    return string.Format("{0:(###) ### ####}", long.Parse(_phoneNumber));
//                }
                
                return _phoneNumber;
            }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneNumber)));
                }
            }
        }

        public FormattedString SendToNumber
        {
            get
            {
                return new FormattedString
                {
                    Spans = {
                        new Span
                        {
                            Text = "We sent your secure verification code to: "
                        },
                        new Span
                        {
                            Text = string.Concat(_country, " ", string.Format("{0:(###) ### ####}", long.Parse(_phoneNumber))),
                            FontSize = 19,
                            FontAttributes = FontAttributes.Bold
                        }
                    }
                };
            }
        }

        private async Task OnPagePhoneNumber()
        {
            await _context.Navigation.PushAsync(new PhoneVerificationPage(this));
        }

        private async Task OnPageSuccessSignup()
        {
            await _context.Navigation.PushAsync(new SuccessSignup());
        }
    }
}