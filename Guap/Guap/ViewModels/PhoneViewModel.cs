using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Views;
using MvvmValidation;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    using Guap.Helpers;

    public class PhoneViewModel : INotifyPropertyChanged
    {
        private string _country;
        private string _phoneNumber;
        private ValidationErrorCollection _errors;
        
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
                if (_country != value && value.Length <= 4)
                {
                    _country = value;
                }
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Country)));
            }
        }
        
        public string PhoneNumber
        {
            get
            {
                var number = new string(_phoneNumber?.Where(char.IsDigit).ToArray());

                if (!string.IsNullOrWhiteSpace(number))
                {
                    return Regex.Replace(number, @"(\d{3})(\d{1,3})(\d{0,6})", "($1) $2 $3").TrimEnd();
                }

                return number;
            }
            set
            {
                if (_phoneNumber != value && value.Length <= 16)
                {
                    _phoneNumber = value;
                }
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneNumber)));
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
                            Text = string.Concat(_country, " ", _phoneNumber),
                            FontSize = 19,
                            FontAttributes = FontAttributes.Bold
                        }
                    }
                };
            }
        }

        public ValidationErrorCollection Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
            }
        }

        private bool ValidatePhoneNumber()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Country, "Country is required.");
            validator.AddRequiredRule(() => PhoneNumber, "Phone number is required.");

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }
        
        private bool ValidateVerifyNumber()
        {
            var validator = new ValidationHelper();
//            validator.AddRequiredRule(() => PhoneNumber, "Code is required.");

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }

        private async Task OnPagePhoneNumber()
        {
            if(!ValidatePhoneNumber())
            {
                return;
            }
            
            await _context.Navigation.PushAsync(new PhoneVerificationPage(this));
        }

        private async Task OnPageSuccessSignup()
        {
            if (!ValidateVerifyNumber())
            {
                return;
            }

            await _context.Navigation.PushAsync(
                new SuccessSignup(
                    new CommonPageSettings()
                        {
                            HasNavigation = false,
                            HeaderText =
                                "Your identity has been verified." + Environment.NewLine
                                + "You can create your wallet"
                        },
                    () => this._context.Navigation.PushAsync(new NewUserExistPage())));
        }
    }
}