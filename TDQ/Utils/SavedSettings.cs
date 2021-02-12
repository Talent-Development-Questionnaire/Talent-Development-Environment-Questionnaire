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
        private const string GroupImageKey = "group_key";
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
    }
}
