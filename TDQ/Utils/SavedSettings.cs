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

        private const string ImageKey = "image_key";
        private const string ThemeIndexKey = "theme_index_key";
        private const string ThemeKey = "theme_key";
        private const string BgKey = "background_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string AccountImageSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(ImageKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ImageKey, value);
            }
        }

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

        public static string BackgroundSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(BgKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BgKey, value);
            }
        }
    }
}
