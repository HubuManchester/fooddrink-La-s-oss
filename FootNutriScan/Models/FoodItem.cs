using SQLite;

namespace FootNutriScan.Models;

public class FoodItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Sugar { get; set; }
}