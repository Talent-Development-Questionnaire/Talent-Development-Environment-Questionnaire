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
    }
}
