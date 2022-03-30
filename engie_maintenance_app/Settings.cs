////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Settings.cs
//FileType: Visual C# Source file
//Author : Archie Vann
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Stores user settings locally and provides GlobalSettings to allow Xamarin bindings
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace engie_maintenance_app
{
    public static class Settings
    {
        // Settings are saved/set here.
        // To edit defaults edit them here
        // To use settings in your code look at GlobalSettings()

        #region Settings default values
        public readonly static string engieBlueHex = "#0af";
        public readonly static string whiteHex = "#ffffff";
        public readonly static string blackHex = "#000000";
        public readonly static string greyHex = "#d6d7d7";
        public readonly static string darkGreyHex = "#494949";

        public static readonly Color EngieBlueColor = Color.FromHex(engieBlueHex);
        public static readonly Color BlackColor = Color.FromHex(blackHex);
        public static readonly Color WhiteColor = Color.FromHex(whiteHex);
        public static readonly Color GreyColor = Color.FromHex(greyHex);
        public static readonly Color DarkGreyColor = Color.FromHex(darkGreyHex);

        public static double defaultFontSize8 = 20;
        // How much less that font size is than defaultFontSize8
        // To have fonts larger than defaultFontSize8 uncomment commented code and add them to GlobalSettings

        //public static double fontSize10Diff = -2; // 22
        //public static double fontSize9Diff = -1; // 21
        public readonly static double fontSize7Diff = 1; // 19
        public readonly static double fontSize6Diff = 2; // 18
        public readonly static double fontSize5Diff = 3; // 17
        public readonly static double fontSize4Diff = 4; // 16
        public readonly static double fontSize3Diff = 5; // 15
        public readonly static double fontSize2Diff = 6; // 14
        public readonly static double fontSize1Diff = 7; // 13
        public readonly static double fontSize0Diff = 8; // 12
        // To set the min/max you must edit it in SettingsPage.xaml.cs as bindings do not work. Possible Xamarin bug
        #endregion

        #region Settings Backend
        static ISettings AppSettings
        {
            get
            {
                if (CrossSettings.IsSupported)
                    return CrossSettings.Current;

                return null;
            }
        }

        public static bool DarkMode
        {
            get => AppSettings.GetValueOrDefault(nameof(DarkMode), false);
            set
            {
                // Set everything to dark mode varient
                if (value)
                {
                    TextColorParaHex = WhiteColor.ToHex();
                    BackgroundColorHex = BlackColor.ToHex();
                }
                else
                {
                    TextColorParaHex = BlackColor.ToHex();
                    BackgroundColorHex = WhiteColor.ToHex();
                }

                // Apply settings change
                AppSettings.AddOrUpdateValue(nameof(DarkMode), value);
            }
        }
        public static string TextColorParaHex
        {
            get => AppSettings.GetValueOrDefault(nameof(TextColorParaHex), blackHex);
            set => AppSettings.AddOrUpdateValue(nameof(TextColorParaHex), value);
        }
        public static string BackgroundColorHex
        {
            get => AppSettings.GetValueOrDefault(nameof(BackgroundColorHex), whiteHex);
            set => AppSettings.AddOrUpdateValue(nameof(BackgroundColorHex), value);
        }
        public static string TextColor1Hex
        {
            get => AppSettings.GetValueOrDefault(nameof(TextColor1Hex), blackHex);
            set => AppSettings.AddOrUpdateValue(nameof(TextColor1Hex), value);
        }
        public static string TextColor2Hex
        {
            get => AppSettings.GetValueOrDefault(nameof(TextColor2Hex), blackHex);
            set => AppSettings.AddOrUpdateValue(nameof(TextColor2Hex), value);
        }
        public static string ButtonBackgroundColorHex
        {
            get => AppSettings.GetValueOrDefault(nameof(ButtonBackgroundColorHex), darkGreyHex);
            set => AppSettings.AddOrUpdateValue(nameof(ButtonBackgroundColorHex), value);
        }
        public static string EntryBackgroundColorHex
        {
            get => AppSettings.GetValueOrDefault(nameof(EntryBackgroundColorHex), whiteHex);
            set => AppSettings.AddOrUpdateValue(nameof(EntryBackgroundColorHex), value);
        }

        #region FontSizes
        public static double FontSize0
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize0), defaultFontSize8 - fontSize0Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize0), value);
        }
        public static double FontSize1
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize1), defaultFontSize8 - fontSize1Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize1), value);
        }
        public static double FontSize2
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize2), defaultFontSize8 - fontSize2Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize2), value);
        }
        public static double FontSize3
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize3), defaultFontSize8 - fontSize3Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize3), value);
        }
        public static double FontSize4
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize4), defaultFontSize8 - fontSize4Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize4), value);
        }
        public static double FontSize5
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize5), defaultFontSize8 - fontSize5Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize5), value);
        }
        public static double FontSize6
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize6), defaultFontSize8 - fontSize6Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize6), value);
        }
        public static double FontSize7
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize7), defaultFontSize8 - fontSize7Diff);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize7), value);
        }
        public static double FontSize8
        {
            get => AppSettings.GetValueOrDefault(nameof(FontSize8), defaultFontSize8);
            set => AppSettings.AddOrUpdateValue(nameof(FontSize8), value);
        }
        #endregion
        #endregion
    }

    class GlobalSettings : INotifyPropertyChanged
    {
        /*
         * To use settings use `Property="{Binding SETTING_NAME}"`
         * Make sure to have `BindingContext = new GlobalSettings();` in the constructor of the page's .xaml.cs
         * Settings will be automatically saved to the device. Nothing else needs to be done
         */

        #region How to add new settings
        /*
         * To add new setting create:
         * 
         * private TYPE _camelCase;
         * public TYPE PascalCase
         * {
         *      set
         *      {
         *          _camelCase = value;
         *          // If using a non-basic type (eg Color) convert it to a primitive type here
         *          Settings.PascalCase = value;
         *          OnPropertyChanged();
         *      }
         *      get { return _camelCase; }
         * }
         */

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public GlobalSettings()
        {
            _darkMode = Settings.DarkMode;
            _backgroundColor = Color.FromHex(Settings.BackgroundColorHex);
            _textColor1 = Color.FromHex(Settings.TextColor1Hex);
            _textColor2 = Color.FromHex(Settings.TextColor2Hex);
            _buttonBackgroundColor = Color.FromHex(Settings.ButtonBackgroundColorHex);
            _entryBackgroundColor = Color.FromHex(Settings.EntryBackgroundColorHex);

            _fontSize0 = Settings.FontSize0;
            _fontSize1 = Settings.FontSize1;
            _fontSize2 = Settings.FontSize2;
            _fontSize3 = Settings.FontSize3;
            _fontSize4 = Settings.FontSize4;
            _fontSize5 = Settings.FontSize5;
            _fontSize6 = Settings.FontSize6;
            _fontSize7 = Settings.FontSize7;
            _fontSize8 = Settings.FontSize8;
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Colors
        private Color _entryBackgroundColor;
        public Color EntryBackgroundColor
        {
            set
            {
                _entryBackgroundColor = value;
                Settings.EntryBackgroundColorHex = value.ToHex();
                OnPropertyChanged();
            }
            get
            {
                return _entryBackgroundColor;
            }
        }

        private bool _darkMode;
        public bool DarkMode
        {
            set
            {
                _darkMode = value;
                Settings.DarkMode = value;

                // If dark mode
                if (value)
                {
                    BackgroundColor = Settings.BlackColor;
                    TextColor1 = Settings.WhiteColor;
                    TextColor2 = Settings.WhiteColor;
                    ButtonBackgroundColor = Settings.DarkGreyColor;
                }
                else
                {
                    BackgroundColor = Settings.WhiteColor;
                    TextColor1 = Settings.BlackColor;
                    TextColor2 = Settings.BlackColor;
                    ButtonBackgroundColor = Settings.GreyColor;
                }

                OnPropertyChanged();
            }
            get { return _darkMode; }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            set
            {
                _backgroundColor = value;
                Settings.BackgroundColorHex = value.ToHex();
                OnPropertyChanged();
            }
            get { return _backgroundColor; }
        }

        private Color _textColor1;
        public Color TextColor1
        {
            set
            {
                _textColor1 = value;
                Settings.TextColor1Hex = value.ToHex();
                OnPropertyChanged();
            }
            get { return _textColor1; }
        }

        private Color _textColor2;
        public Color TextColor2
        {
            set
            {
                _textColor2 = value;
                Settings.TextColor2Hex = value.ToHex();
                OnPropertyChanged();
            }
            get { return _textColor2; }
        }

        private Color _buttonBackgroundColor;
        public Color ButtonBackgroundColor
        {
            set
            {
                _buttonBackgroundColor = value;
                Settings.ButtonBackgroundColorHex = value.ToHex();
                OnPropertyChanged();
            }
            get { return _buttonBackgroundColor; }
        }
        #endregion

        #region FontSizes
        private double _fontSize0;
        public double FontSize0
        {
            set
            {
                _fontSize0 = value;
                Settings.FontSize0 = value;
                OnPropertyChanged();
            }
            get { return _fontSize0; }
        }

        private double _fontSize1;
        public double FontSize1
        {
            set
            {
                _fontSize1 = value;
                Settings.FontSize1 = value;
                OnPropertyChanged();
            }
            get { return _fontSize1; }
        }

        private double _fontSize2;
        public double FontSize2
        {
            set
            {
                _fontSize2 = value;
                Settings.FontSize2 = value;
                OnPropertyChanged();
            }
            get { return _fontSize2; }
        }

        private double _fontSize3;
        public double FontSize3
        {
            set
            {
                _fontSize3 = value;
                Settings.FontSize3 = value;
                OnPropertyChanged();
            }
            get { return _fontSize3; }
        }

        private double _fontSize4;
        public double FontSize4
        {
            set
            {
                _fontSize4 = value;
                Settings.FontSize4 = value;
                OnPropertyChanged();
            }
            get { return _fontSize4; }
        }

        private double _fontSize5;
        public double FontSize5
        {
            set
            {
                _fontSize5 = value;
                Settings.FontSize5 = value;
                OnPropertyChanged();
            }
            get { return _fontSize5; }
        }

        private double _fontSize6;
        public double FontSize6
        {
            set
            {
                _fontSize6 = value;
                Settings.FontSize6 = value;
                OnPropertyChanged();
            }
            get { return _fontSize6; }
        }

        private double _fontSize7;
        public double FontSize7
        {
            set
            {
                _fontSize7 = value;
                Settings.FontSize7 = value;
                OnPropertyChanged();
            }
            get { return _fontSize7; }
        }

        private double _fontSize8;
        public double FontSize8
        {
            set
            {
                double newValue = value;
                double oldValue = FontSize8;
                double diff = newValue - oldValue;

                FontSize0 += diff;
                FontSize1 += diff;
                FontSize2 += diff;
                FontSize3 += diff;
                FontSize4 += diff;
                FontSize5 += diff;
                FontSize6 += diff;
                FontSize7 += diff;

                _fontSize8 = value;
                Settings.FontSize8 = value;
                OnPropertyChanged();
            }
            get { return _fontSize8; }
        }
        #endregion

        // These are just mappings to the settings version of the colors.
        // They are just here to be accessed inside .xaml files
        public Color EngieBlueColor = Settings.EngieBlueColor;
        public Color BlackColor = Settings.BlackColor;
        public Color WhiteColor = Settings.WhiteColor;
        public Color GreyColor = Settings.GreyColor;
        public Color DarkGreyColor = Settings.DarkGreyColor;
    }
}