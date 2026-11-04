namespace Garaget.Vehicles
{
    public class Bus : Vehicle
    {
        public int NumberOfSeats { get; }
        public override string Details => $"{NumberOfSeats} säten";

        public Bus (string regNumber, string color, int wheels, string brand, int numberOfSeats) 
            : base (regNumber, color, wheels, brand)
        {
            NumberOfSeats = numberOfSeats;
        }


        // Kollar först basklassens fält (regnummer, färg, märke, hjul),
        // sen bussens egna fält (säten)
        public override bool Matches(string searchTerm)
        {
            return base.Matches(searchTerm)
                || NumberOfSeats.ToString() == searchTerm;
                
        }

        public override string ToString() => $"{base.ToString()}, antal säten: {NumberOfSeats}";
    }
}
