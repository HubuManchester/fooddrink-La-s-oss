using System.Collections.ObjectModel;

namespace FootNutriScan.Pages;

public partial class MealPlanPage : ContentPage
{
    private ObservableCollection<PlannedFood> _plannedFoods = new();
    private int _totalCalories = 0;

    public MealPlanPage()
    {
        InitializeComponent();
        FoodListView.ItemsSource = _plannedFoods;
    }

    private void OnAddFoodClicked(object sender, EventArgs e)
    {
        string name = FoodNameEntry.Text?.Trim() ?? string.Empty;
        string calStr = CaloriesEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(calStr))
        {
            DisplayAlert("Input Error", "Please enter both food name and calories.", "OK");
            return;
        }

        if (!int.TryParse(calStr, out int calories) || calories < 0)
        {
            DisplayAlert("Input Error", "Please enter a valid positive number for calories.", "OK");
            return;
        }

        _plannedFoods.Add(new PlannedFood { Name = name, Calories = calories });
        _totalCalories += calories;
        TotalCaloriesLabel.Text = $"Total: {_totalCalories} kcal";

        FoodNameEntry.Text = string.Empty;
        CaloriesEntry.Text = string.Empty;
    }
}

public class PlannedFood
{
    public string Name { get; set; } = string.Empty;
    public int Calories { get; set; }
}