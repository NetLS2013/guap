using System;
using UIKit;

namespace Guap.iOS.Helpers
{
    public static class UICustomColor
    {
        public static UIColor FromHex(uint rgbValue, float alpha = 1.0f)
        {
            var red = ((rgbValue & 0xFF0000) >> 16) / 256.0f;
            var green = ((rgbValue & 0xFF00) >> 8) / 256.0f;
            var blue = (rgbValue & 0xFF) / 256.0f;
            
            return new UIColor(red, green, blue, alpha);
        }
    }
}