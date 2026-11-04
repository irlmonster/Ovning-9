namespace Garaget.Vehicles
{
    public class Motorcycle : Vehicle
    {
        public int CylinderVolume { get; }
        public MotorcycleType Type { get; }
        public override string Details => $"{CylinderVolume} cc, {Type}";

        public Motorcycle(string regNumber, string color, int wheels, string brand, int cylinderVolume, MotorcycleType type) : base (regNumber, color, wheels, brand)
        {
            CylinderVolume = cylinderVolume;
            Type = type;
        }



        // Kollar först basklassens fält (regnummer, färg, märke, hjul),
        // sen motorcykelns egna fält (cylindervolym och typ)
        public override bool Matches(string searchTerm)
        {
            return base.Matches(searchTerm)
                || CylinderVolume.ToString() == searchTerm
                || Type.ToString().Equals(searchTerm, StringComparison.OrdinalIgnoreCase);
        }


        public override string ToString() =>
            $"{base.ToString()}, {Type}, {CylinderVolume} cc";
    }
}
