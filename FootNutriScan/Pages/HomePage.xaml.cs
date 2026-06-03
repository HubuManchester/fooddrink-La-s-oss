using Microsoft.Maui.Devices.Sensors;

namespace FootNutriScan.Pages;

public partial class HomePage : ContentPage
{
    private int _targetCalories;
    private double _targetProtein;
    private double _targetSugar;

    public HomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = TryGetLocationAsync();
    }

    private async void OnSaveGoalsClicked(object sender, EventArgs e)
    {
        string calText = TargetCaloriesEntry.Text?.Trim() ?? string.Empty;
        string protText = TargetProteinEntry.Text?.Trim() ?? string.Empty;
        string sugText = TargetSugarEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(calText) || string.IsNullOrWhiteSpace(protText) || string.IsNullOrWhiteSpace(sugText))
        {
            await DisplayAlert("Missing Info", "Please fill in all goal fields.", "OK");
            return;
        }

        if (!int.TryParse(calText, out int cals) || cals <= 0 ||
            !double.TryParse(protText, out double prot) || prot < 0 ||
            !double.TryParse(sugText, out double sug) || sug < 0)
        {
            await DisplayAlert("Invalid Input", "Please enter valid positive numbers.", "OK");
            return;
        }

        _targetCalories = cals;
        _targetProtein = prot;
        _targetSugar = sug;

        ProgressLabel.Text = $"Target set:\nCalories: {cals} kcal\nProtein: {prot:F1} g\nSugar: {sug:F1} g";
        await DisplayAlert("Goals Saved", "Your daily nutrition goals have been updated!", "OK");
    }

    private async void OnRefreshLocationClicked(object sender, EventArgs e)
    {
        await TryGetLocationAsync();
    }

    private async Task TryGetLocationAsync()
    {
        RefreshLocationBtn.IsEnabled = false;
        LocationLabel.Text = "Fetching location...";

        try
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                LocationLabel.Text = "Location permission was not granted.";
                return;
            }

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
            {
                string placeName = "";
                try
                {
                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                        placeName = $"{placemark.Locality}, {placemark.CountryName}";
                }
                catch { }

                LocationLabel.Text = string.IsNullOrEmpty(placeName)
                    ? $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}"
                    : $"{placeName}\nLat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";
            }
            else
            {
                LocationLabel.Text = "Could not retrieve location. Please check device settings.";
            }
        }
        catch (FeatureNotSupportedException)
        {
            LocationLabel.Text = "Location service not supported on this device.";
        }
        catch (PermissionException)
        {
            LocationLabel.Text = "Location permission missing.";
        }
        catch (Exception ex)
        {
            LocationLabel.Text = "Something went wrong. Please try again.";
        }
        finally
        {
            RefreshLocationBtn.IsEnabled = true;
        }
    }
}