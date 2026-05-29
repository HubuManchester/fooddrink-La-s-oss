using Microsoft.Maui.Devices.Sensors;

namespace FootNutriScan.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        _ = TryGetLocationAsync();
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
                LocationLabel.Text = "Location permission denied.";
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
                    {
                        placeName = $"{placemark.Locality}, {placemark.CountryName}";
                    }
                }
                catch
                {

                }

                LocationLabel.Text = string.IsNullOrEmpty(placeName)
                    ? $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}"
                    : $"{placeName}\nLat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";
            }
            else
            {
                LocationLabel.Text = "Could not get location. Check device settings.";
            }
        }
        catch (FeatureNotSupportedException)
        {
            LocationLabel.Text = "Geolocation not supported on this device.";
        }
        catch (PermissionException)
        {
            LocationLabel.Text = "Location permission missing.";
        }
        catch (Exception ex)
        {
            LocationLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            RefreshLocationBtn.IsEnabled = true;
        }
    }
}