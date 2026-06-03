using System.Net.Http.Json;
using FootNutriScan.Models;

namespace FootNutriScan.Services;

public class ApiFoodService
{
    private readonly HttpClient _httpClient = new();
    private const string BaseUrl = "https://6a192229489e471575197906.mockapi.io/food";

    public async Task<FoodItem> GetFoodAsync(string name)
    {
        try
        {
            var url = $"{BaseUrl}?name={Uri.EscapeDataString(name)}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var foods = await response.Content.ReadFromJsonAsync<List<FoodItem>>();
                return foods?.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"API error: {ex.Message}");
        }
        return null;
    }

    public async Task<List<FoodItem>> GetAllFoodsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<FoodItem>>(BaseUrl) ?? new();
        }
        catch
        {
            return new();
        }
    }
}