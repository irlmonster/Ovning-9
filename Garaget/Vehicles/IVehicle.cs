using System.Text.Json.Serialization;

namespace Garaget.Vehicles
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(Car), "Car")]
    [JsonDerivedType(typeof(Bus), "Bus")]
    [JsonDerivedType(typeof(Motorcycle), "Motorcycle")]
    [JsonDerivedType(typeof(Boat), "Boat")]
    [JsonDerivedType(typeof(Airplane), "Airplane")] 
    public interface IVehicle
    {
        string RegNumber { get; }
        string Color { get; }
        int Wheels { get; }
        string Brand { get; }
        string Details { get; } 

        // en metod som måste implementeras
        bool Matches(string searchTerm);



    }
}
