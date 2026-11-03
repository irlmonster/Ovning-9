namespace Garaget.Storage
{
    public interface IGarageStorage
    {
        Task SaveAsync(GarageData garageData);
        Task<GarageData> LoadAsync();
    }
}
