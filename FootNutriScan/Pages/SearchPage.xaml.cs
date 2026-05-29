using FootNutriScan.Services;
using FootNutriScan.Models;

namespace FootNutriScan.Pages;

public partial class SearchPage : ContentPage
{
    private readonly ApiFoodService _foodService = new();
    private FoodItem _currentResult;

    public SearchPage()
    {
        InitializeComponent();
    }

    private async void OnSearchClicked(object sender, EventArgs e) => await SearchFood();
    private async void OnSearchPressed(object sender, EventArgs e) => await SearchFood();

    private async Task SearchFood()
    {
        string food = FoodSearchBar.Text?.Trim();
        if (string.IsNullOrWhiteSpace(food))
        {
            await DisplayAlert("Input Error", "Please enter a food name.", "OK");
            return;
        }

        try
        {
            var result = await _foodService.GetFoodAsync(food);
            if (result != null)
            {
                _currentResult = result;
                UpdateUI(result);
            }
            else
            {
                _currentResult = null;
                ClearResult();
                await DisplayAlert("Not Found",
                    $"'{food}' was not found in the database.\nMake sure you typed it exactly as stored.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Connection Error",
                $"Could not connect to the food database. Check your internet.\n{ex.Message}", "OK");
        }
    }

    private void UpdateUI(FoodItem item)
    {
        FoodNameLabel.Text = item.Name;
        CaloriesLabel.Text = $"Calories: {item.Calories} kcal";
        ProteinLabel.Text = $"Protein: {item.Protein} g";
        SugarLabel.Text = $"Sugar: {item.Sugar} g";
        SpeakButton.IsVisible = true;
    }

    private void ClearResult()
    {
        FoodNameLabel.Text = "Food Name";
        CaloriesLabel.Text = "Calories: ";
        ProteinLabel.Text = "Protein: ";
        SugarLabel.Text = "Sugar: ";
        SpeakButton.IsVisible = false;
    }

    private async void OnSpeakClicked(object sender, EventArgs e)
    {
        if (_currentResult == null) return;
        string text = $"{_currentResult.Name}. Calories: {_currentResult.Calories} kilocalories...";
        try
        {
            await TextToSpeech.Default.SpeakAsync(text);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Text-to-speech failed: {ex.Message}", "OK");
        }
    }
}