using System;
using System.Collections.Generic;
using System.Diagnostics;
using Guap.Models;
using Guap.Views.Setting;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly Page _context;
        private List<SettingsModel> _settingsList;

        public SettingsViewModel(Page context)
        {
            _context = context;
            
            SettingsList = new List<SettingsModel>
            {
                new SettingsModel { Title = "Help & Support", Method = HelpAndSupport },
                new SettingsModel { Title = "Send Feedback", Method = SendFeedBack },
                new SettingsModel { Title = "About $Guapcoin", Method = AboutGuap },
            };
        }
        
        public List<SettingsModel> SettingsList
        {
            get
            {
                return _settingsList;
            }
            set
            {
                _settingsList = value;
                
                OnPropertyChanged(nameof(SettingsList));
            }
        }

        public SettingsModel SelectedSetting
        {
            set
            {
                if (value != null)
                {
                    OnSelectedSetting(value);
                }
            }
        }

        private async void OnSelectedSetting(SettingsModel settings)
        {
            if (settings == null)
                return;
            
            settings.Method?.Invoke();
        }

        private async void HelpAndSupport()
        {
            await _context.Navigation.PushAsync(new HelpAndSupportPage());
        }
        
        private async void AboutGuap()
        {
            await _context.Navigation.PushAsync(new AboutGuapPage());
        }

        private async void SendFeedBack()
        {
            var url = string.Empty;
            var appId = string.Empty;

            if (Device.RuntimePlatform == Device.iOS)
            {
                appId = "407690035";
                url = "itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?" +
                        $"id={appId}&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software";
            }
            else if(Device.RuntimePlatform == Device.Android)
            {
                appId = "com.hoteltonight.android.prod";
                url = $"market://details?id={appId}";
            }

            try
            {
                Device.OpenUri(new Uri(url));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.Message}");
            }
        }
    }
}