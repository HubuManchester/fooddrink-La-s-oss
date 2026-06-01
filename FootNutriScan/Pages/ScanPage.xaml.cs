using FootNutriScan.Services;   

namespace FootNutriScan.Pages;

public partial class ScanPage : ContentPage
{
    private readonly ApiFoodService _foodService = new();

    public ScanPage()
    {
        InitializeComponent();
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        await CapturePhoto();
    }

    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        await PickPhoto();
    }

    private async Task CapturePhoto()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied",
                    "Camera permission is needed to take photos.", "OK");
                return;
            }

            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                await LoadPhoto(photo);
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Not Supported", "Camera is not available on this device.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task PickPhoto()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied",
                    "Storage permission is needed to pick photos.", "OK");
                return;
            }

            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                await LoadPhoto(photo);
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Not Supported", "Photo picking is not supported.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task LoadPhoto(FileResult file)
    {
        var stream = await file.OpenReadAsync();
        CapturedImage.Source = ImageSource.FromStream(() => stream);
        PhotoStatusLabel.Text = "Photo loaded. You can look up a food.";
    }

    private async void OnLookUpClicked(object sender, EventArgs e)
    {
        string foodName = ManualFoodEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(foodName))
        {
            await DisplayAlert("Input Error", "Please enter a food name to look up.", "OK");
            return;
        }

        try
        {
            var food = await _foodService.GetFoodAsync(foodName);
            if (food != null)
            {
                string info = $"{food.Name}\nCalories: {food.Calories} kcal\n" +
                              $"Protein: {food.Protein} g\nSugar: {food.Sugar} g";
                await DisplayAlert("Food Info", info, "OK");
            }
            else
            {
                await DisplayAlert("Not Found", $"No food found with name '{foodName}'", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not retrieve data: {ex.Message}", "OK");
        }
    }
}