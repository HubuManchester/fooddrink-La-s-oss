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
        LoadPopularFoods();
    }

    private async void LoadPopularFoods()
    {
        List<FoodItem> foods = null;
        try
        {
            foods = await _foodService.GetAllFoodsAsync();
        }
        catch { }

        if (foods == null || foods.Count == 0)
            foods = GetLocalFoods();

        PopularFoodsView.ItemsSource = foods;
    }

    private async void OnSearchClicked(object sender, EventArgs e) => await SearchFood();
    private async void OnSearchPressed(object sender, EventArgs e) => await SearchFood();

    private async Task SearchFood()
    {
        string food = FoodSearchBar.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(food))
        {
            await DisplayAlert("Missing Info", "Please enter a food name.", "OK");
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
                ResultFrame.IsVisible = false;
                await DisplayAlert("Not Found", $"'{food}' is not in our database. Please try another food.", "OK");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Connection Issue", "We couldn't connect to the server. Please check your internet.", "OK");
        }
    }

    private void UpdateUI(FoodItem item)
    {
        FoodNameLabel.Text = item.Name;
        CaloriesLabel.Text = $"Calories: {item.Calories} kcal";
        ProteinLabel.Text = $"Protein: {item.Protein} g";
        SugarLabel.Text = $"Sugar: {item.Sugar} g";
        SpeakButton.IsVisible = true;
        ResultFrame.IsVisible = true;
    }

    private async void OnSpeakClicked(object sender, EventArgs e)
    {
        if (_currentResult == null) return;
        string text = $"{_currentResult.Name}. Calories: {_currentResult.Calories} kilocalories. " +
                      $"Protein: {_currentResult.Protein} grams. Sugar: {_currentResult.Sugar} grams.";
        try
        {
            await TextToSpeech.Default.SpeakAsync(text);
        }
        catch (Exception)
        {
            await DisplayAlert("Playback Error", "Unable to read aloud at this moment.", "OK");
        }
    }

    private async void OnPopularFoodSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is FoodItem selected)
        {
            _currentResult = selected;
            UpdateUI(selected);
            PopularFoodsView.SelectedItem = null; // deselect
        }
    }

    private List<FoodItem> GetLocalFoods()
    {
        return new List<FoodItem>
        {
            new() { Name = "Apple", Calories = 52, Protein = 0.3, Sugar = 10.4 },
            new() { Name = "Banana", Calories = 89, Protein = 1.1, Sugar = 12.2 },
            new() { Name = "Orange Juice", Calories = 45, Protein = 0.7, Sugar = 8.4 },
            new() { Name = "Coca Cola", Calories = 42, Protein = 0, Sugar = 10.6 },
            new() { Name = "Milk Tea", Calories = 120, Protein = 2.1, Sugar = 18 },
            new() { Name = "Latte", Calories = 64, Protein = 3.3, Sugar = 5.2 },
            new() { Name = "Fried Chicken", Calories = 246, Protein = 21, Sugar = 0.5 },
            new() { Name = "Burger", Calories = 295, Protein = 17, Sugar = 5 },
            new() { Name = "Pizza", Calories = 266, Protein = 11, Sugar = 3.6 },
            new() { Name = "French Fries", Calories = 312, Protein = 3.4, Sugar = 0.4 },
            new() { Name = "Chicken Rice", Calories = 607, Protein = 27, Sugar = 4 },
            new() { Name = "Sushi", Calories = 130, Protein = 6, Sugar = 3 },
            new() { Name = "Salad", Calories = 33, Protein = 2, Sugar = 2.5 },
            new() { Name = "Ice Cream", Calories = 207, Protein = 3.5, Sugar = 21 },
            new() { Name = "Chocolate Cake", Calories = 371, Protein = 4.9, Sugar = 38 },
            new() { Name = "Instant Noodles", Calories = 470, Protein = 9, Sugar = 2 },
            new() { Name = "Green Tea", Calories = 1, Protein = 0.2, Sugar = 0 },
            new() { Name = "Protein Shake", Calories = 160, Protein = 25, Sugar = 3 },
            new() { Name = "Yogurt", Calories = 59, Protein = 10, Sugar = 3.6 },
            new() { Name = "Energy Drink", Calories = 45, Protein = 0, Sugar = 11 },
            new() { Name = "Strawberry", Calories = 33, Protein = 0.7, Sugar = 4.9 },
            new() { Name = "Watermelon", Calories = 30, Protein = 0.6, Sugar = 6.2 },
            new() { Name = "Mango Smoothie", Calories = 98, Protein = 1.2, Sugar = 20 },
            new() { Name = "Espresso", Calories = 9, Protein = 0.1, Sugar = 0 },
            new() { Name = "Soy Milk", Calories = 54, Protein = 3.3, Sugar = 4 },
            new() { Name = "Fried Rice", Calories = 163, Protein = 5, Sugar = 1.7 },
            new() { Name = "Spaghetti", Calories = 158, Protein = 5.8, Sugar = 0.8 },
            new() { Name = "Hot Dog", Calories = 290, Protein = 10, Sugar = 4 },
            new() { Name = "Taco", Calories = 226, Protein = 9, Sugar = 2 },
            new() { Name = "Sandwich", Calories = 250, Protein = 12, Sugar = 5 },
            new() { Name = "Donut", Calories = 452, Protein = 4.9, Sugar = 25 },
            new() { Name = "Cheesecake", Calories = 321, Protein = 6, Sugar = 22 },
            new() { Name = "Pancake", Calories = 227, Protein = 6, Sugar = 8 },
            new() { Name = "Oatmeal", Calories = 68, Protein = 2.4, Sugar = 0.5 },
            new() { Name = "Peanut Butter", Calories = 588, Protein = 25, Sugar = 9 },
            new() { Name = "Grilled Salmon", Calories = 208, Protein = 20, Sugar = 0 },
            new() { Name = "Steak", Calories = 271, Protein = 25, Sugar = 0 },
            new() { Name = "Bubble Tea", Calories = 180, Protein = 1, Sugar = 30 },
            new() { Name = "Lemonade", Calories = 40, Protein = 0.1, Sugar = 9 },
            new() { Name = "Sports Drink", Calories = 24, Protein = 0, Sugar = 6 }
        };
    }
}