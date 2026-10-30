namespace Garaget.Vehicles
{
    public class Airplane : Vehicle
    {

        public int NumberOfEngines{ get; }

        public Airplane(string regNumber, string color, int wheels, string brand, int numberOfEngines) : base(regNumber, color, wheels, brand)
        {
            NumberOfEngines = numberOfEngines;
        }

        // Kollar först basklassens fält (regnummer, färg, märke, hjul),
        // sen flygplanets egna fält (antal motorer)
        public override bool Matches(string searchTerm)
        {
            return base.Matches(searchTerm)
                || NumberOfEngines.ToString() == searchTerm;                
        }


        public override string ToString() => $"{base.ToString()}, antal motorer: {NumberOfEngines}";



    }
}
