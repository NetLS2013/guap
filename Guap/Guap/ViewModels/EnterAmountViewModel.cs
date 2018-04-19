using System;
using System.Globalization;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class EnterAmountViewModel : BaseViewModel
    {
        public event Action<decimal> AmountChanged;

        private decimal _inputAmmount;
        
        private readonly Page _context;
        
        private readonly NumberFormatInfo _noneSymbolFormat;

        public EnterAmountViewModel(Page context)
        {
            _context = context;
            
            _noneSymbolFormat = (NumberFormatInfo) CultureInfo.CurrentCulture.NumberFormat.Clone();
            _noneSymbolFormat.CurrencySymbol = "";
        }
        
        public string InputAmmount
        {
            get
            {
                return _inputAmmount.ToString();
            }
            set
            {
                _inputAmmount = decimal.TryParse(value, out var amount) ? amount : 0;
                OnPropertyChanged(nameof(AmountTrigger));
                OnPropertyChanged(nameof(CurrencyConverter));
                if (this._inputAmmount > decimal.Zero)
                {
                    AmountChanged(_inputAmmount);
                }
            }
        }
        
        public FormattedString AmountTrigger
        {
            get
            {
                return new FormattedString
                {
                    Spans = {
                        new Span
                        {
                            FontSize = 32,
                            Text = $"{_inputAmmount}",
                            FontAttributes = FontAttributes.Bold
                        },
                        new Span
                        {
                            FontSize = 16,
                            Text = " ETH",
                            FontAttributes = FontAttributes.Bold
                        }
                    }
                };
            }
        }

        public FormattedString CurrencyConverter
        {
            get
            {
                return new FormattedString
                {
                    Spans = {
                        new Span
                        {
                            Text = "= ",
                            FontAttributes = FontAttributes.Bold
                        },
                        new Span
                        {
                            Text = string.Format(_noneSymbolFormat, "{0:c}", _inputAmmount * (decimal)379.05),
                            FontAttributes = FontAttributes.Bold
                        },
                        new Span
                        {
                            Text = " USD",
                            FontAttributes = FontAttributes.Bold
                        }
                    }
                };
            }
        }
    }
}