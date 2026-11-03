using Garaget.Vehicles;

namespace Garaget.Storage
{
    public class GarageData
    {
        public int Capacity { get; set; }
        public List<IVehicle> Vehicles { get; set; } = new List<IVehicle>();
    }
}
