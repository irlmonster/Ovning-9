namespace Garaget.Vehicles
{
    public interface IVehicle
    {
        string RegNumber { get; }
        string Color { get; }
        int Wheels { get; }
        string Brand { get; }

        // en metod som måste implementeras
        bool Matches(string searchTerm);



    }
}
