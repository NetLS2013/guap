using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Guap.Models;
using Guap.Service;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class EnterAmountViewModel : BaseViewModel
    {
        public event Action<decimal> AmountChanged;

        private decimal? _inputAmmount;
        private decimal _usdPrice;
        
        private readonly Page _context;
        
        private readonly NumberFormatInfo _noneSymbolFormat;
        private readonly RequestProvider _requestProvider;

        public EnterAmountViewModel(Page context)
        {
            _context = context;
            
            _requestProvider = new RequestProvider();
            
            _noneSymbolFormat = (NumberFormatInfo) CultureInfo.CurrentCulture.NumberFormat.Clone();
            _noneSymbolFormat.CurrencySymbol = "";

            Task.Run(async () => await Initialization());
        }

        private async Task Initialization()
        {
            try
            {
                var fiatResult = await _requestProvider.GetAsync<Fiat[]>(GlobalSetting.Instance.FiatEndpoint);
                {
                    decimal.TryParse(fiatResult[0].PriceUsd, out _usdPrice);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
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
                    AmountChanged(_inputAmmount.GetValueOrDefault());
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
                            Text = string.Format(_noneSymbolFormat, "{0:c}", _inputAmmount * _usdPrice),
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