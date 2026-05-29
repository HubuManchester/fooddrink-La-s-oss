using FootNutriScan.Services;

namespace FootNutriScan.Pages;

public partial class ScanPage : ContentPage
{
    private readonly ApiFoodService _foodService = new();


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
                await DisplayAlert("Not Found", $"No food found named '{foodName}'", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not retrieve data: {ex.Message}", "OK");
        }
    }
}