namespace FootNutriScan.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        DarkModeSwitch.IsToggled = App.Current.UserAppTheme == AppTheme.Dark;
        FontSizeSlider.ValueChanged += OnFontSizeChanged;
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        App.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }

    private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
    {
        SampleTextLabel.FontSize = e.NewValue;
    }
}