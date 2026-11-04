namespace Garaget.Vehicles
{
    public class Car : Vehicle
    {
        public string FuelType { get; }
        public CarType Type { get; }

        public override string Details => $"{FuelType}, {Type}";

        public Car(string regNumber, string color, int wheels, string brand, string fuelType, CarType type) : base(regNumber, color, wheels, brand)
        {
            FuelType = fuelType;
            Type = type;
        }

        // Kollar först basklassens fält (regnummer, färg, märke, hjul),
        // sen bilens egna fält (bränsle och typ)
        public override bool Matches(string searchTerm)
        {
            return base.Matches(searchTerm)
                || FuelType.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)
                || Type.ToString().Equals(searchTerm, StringComparison.OrdinalIgnoreCase);
        }


        public override string ToString() => $"{base.ToString()}, {Type}, bränsle: {FuelType}";
    }
}
