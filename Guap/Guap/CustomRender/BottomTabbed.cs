﻿using Xamarin.Forms;

namespace Guap.CustomRender
{
    public class BottomTabbed : TabbedPage
    {
        public enum BarThemeTypes { Light, DarkWithAlpha, DarkWithoutAlpha }

        public bool FixedMode { get; set; }
        public BarThemeTypes BarTheme { get; set; }
        public Color IconActiveColor { get; set; }

        public BottomTabbed()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BarTextColor = Color.White;
            }
        }
    }
}