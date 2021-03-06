﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Models;
using Guap.Service;
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
        private string _verificationCode;
        private ValidationErrorCollection _errors;
        
        private readonly Page _context;
        
        private readonly RequestProvider _requestProvider;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public ICommand PagePhoneNumberCommand => new Command(async () => await OnPagePhoneNumber());
        public ICommand PageSuccessSignupCommand => new Command(async () => await OnPageSuccessSignup());
        
        public ICommand BackCommand => new Command(async () => await _context.Navigation.PopAsync());

        public PhoneViewModel(Page context)
        {
            _context = context;
            _requestProvider = new RequestProvider();
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
        
        public string VerificationCode
        {
            get
            {
                return _verificationCode;
            }
            set
            {
                if (_verificationCode != value && value.Length <= 6)
                {
                    _verificationCode = value;
                }
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VerificationCode)));
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
                if (_phoneNumber != value && value.Length <= 15)
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
        
        private bool Validate(ValidationHelper validator)
        {
            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }

        private async Task OnPagePhoneNumber()
        {
            var validator = new ValidationHelper();
            
            validator.AddRequiredRule(() => Country, "Country is required.");
            validator.AddRequiredRule(() => PhoneNumber, "Phone number is required.");
            
            if(!Validate(validator))
            {
                return;
            }

            try
            {
                var phoneNumber = string.Concat(_country.Trim(), new string(_phoneNumber?.Where(char.IsDigit).ToArray()));
                var result = await _requestProvider
                    .PostAsync<UserModel, ResultModel>(GlobalSetting.Instance.RegisterNumberEndpoint, 
                        new UserModel { PhoneNumber = phoneNumber });

                validator.AddRule(PhoneNumber,
                    () => RuleResult.Assert(result.Result, result.Message));
                
                if (!Validate(validator))
                {
                    return;
                }
                
                if (result.Result)
                {
                    await _context.Navigation.PushSingleAsync(new PhoneVerificationPage(this));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
        }

        private async Task OnPageSuccessSignup()
        {
            var validator = new ValidationHelper();
            
            validator.AddRequiredRule(() => VerificationCode, "The verification code is required.");
            
            if (!Validate(validator))
            {
                return;
            }

            try
            {
                var phoneNumber = string.Concat(_country, new string(_phoneNumber?.Where(char.IsDigit).ToArray()));
                var result = await _requestProvider
                    .PostAsync<UserModel, bool>(GlobalSetting.Instance.VerificationCodeEndpoint, 
                        new UserModel { PhoneNumber = phoneNumber, VerificationCode = _verificationCode });

                validator.AddRule(VerificationCode,
                    () => RuleResult.Assert(result, "The verification code was incorrect.\nPlease try again."));
                
                if (!Validate(validator))
                {
                    return;
                }
                
                await _context.Navigation.PushSingleAsync(
                    new SuccessSignup(
                        new CommonPageSettings
                        {
                            HasNavigation = false,
                            HeaderText =
                                "Your identity has been verified." + Environment.NewLine
                                                                   + "You can create your wallet."
                        },
                        () => this._context.Navigation.PushSingleAsync(new EnterEmailPage())));
                
                Settings.Set(Settings.Key.VerificationCode, _verificationCode);
                Settings.Set(Settings.Key.PhoneNumber, phoneNumber);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
        }
    }
}