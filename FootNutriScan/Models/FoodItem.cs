using System.Text.Json.Serialization;

namespace FootNutriScan.Models;

public class FoodItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("calories")]
    public int Calories { get; set; }

    [JsonPropertyName("protein")]
    public double Protein { get; set; }

    [JsonPropertyName("sugar")]
    public double Sugar { get; set; }
}