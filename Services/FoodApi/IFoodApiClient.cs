namespace CalorieTracker.Services.FoodApi;

public interface IFoodApiClient
{
    Task<List<FoodProduct>> SearchAsync(string query, int pageSize = 10);
    Task<FoodProduct?> GetByBarcodeAsync(string barcode);
}