using Garaget.Validation;
namespace Garaget.Vehicles
{
    public abstract class Vehicle : IVehicle
    {
        public string RegNumber { get; }
        public string Color { get; }
        public int Wheels { get; }
        public string Brand { get; }


        protected Vehicle( string regNumber, string color, int wheels, string brand)
        {
            // Kallar på var validation för regnummer som vi byggt, kollar först så vi inte bygger ett halvt objekt i onödan
            if (!RegNumberValidation.IsValid(regNumber))
            {
                throw new ArgumentException("Regnumret måste vara tre bokstäver, två siffror och en siffra eller bokstav", nameof(regNumber));
            }


            RegNumber = RegNumberValidation.Normalize(regNumber);    
            Color = color;
            Wheels = wheels;
            Brand = brand;
        }

        // Returnerar true om söktermen matchar något av fordonets grundfält. IgnoreCase hoppar stor/liten bokstac och ordinal hoppar språkregler
        // virtual så att subklasser kan lägga till sina egna fält med override.
        public virtual bool Matches(string searchTerm)
        {
            return RegNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || Color.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)
                || Brand.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)
                || Wheels.ToString() == searchTerm;
        }



        public override string ToString() =>
        $"{GetType().Name} {RegNumber}, {Brand}, {Color}, {Wheels} hjul";



    }
}
