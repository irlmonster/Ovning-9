using Garaget.Vehicles;

namespace Garaget.Handlers
{
    public interface IHandler
    {
        int Capacity { get; }
        int Count { get; }
        bool IsFull { get; }

        void CreateNewGarage(int capacity);
        IVehicle? FindByRegNumber(string regNumber);
        List<IVehicle> GetAllVehicles();
        List<TypeCount> GetTypeCounts();
        bool Park(IVehicle vehicle);
        bool Remove(string regNumber);
        List<IVehicle> Search(string searchTerm);
        List<IVehicle> SearchByProperties(string? color = null, int? wheels = null, string? brand = null, string? type = null);
        Task SaveAsync();
        Task LoadAsync();

    }
}



