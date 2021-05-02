using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TDQ.Utils
{
    public static class SavedSettings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string VerifiedKey = "verified_key";
        private const string GroupImageKey = "group_key";
        private const string ThemeIndexKey = "theme_index_key";
        private const string ThemeKey = "theme_key";
        private const string FontIndexKey = "font_index_key";
        private const string FontKey = "font_key";
        private const string BgKey = "background_key";
        private const string LogInKey = "login_key";
        private static readonly string SettingsDefault = string.Empty;


        #endregion

        //Save value to phone to use for later
        public static string AccountImageSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(VerifiedKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(VerifiedKey, value);
            }
        }
        //
        public static string GroupImageSetting
        {
            get
            {
                return AppSettings.GetValueOrDefault(GroupImageKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(GroupImageKey, value);
            }
        }
        //Save selected index for the theme picker
        public static string ThemeIndexSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(ThemeIndexKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ThemeIndexKey, value);
            }
        }
        //Save theme colour
        public static string ThemeSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(ThemeKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ThemeKey, value);
            }
        }
        //Save selected index for font picker
        public static string FontIndexSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(FontIndexKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FontIndexKey, value);
            }
        }
        //Save font size
        public static string FontSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(FontKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FontKey, value);
            }
        }

        //Save email
        public static string LoginSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(LogInKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LogInKey, value);
            }
        }
    }
}
