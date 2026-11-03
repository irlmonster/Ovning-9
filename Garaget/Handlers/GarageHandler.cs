using Garaget.Garages;
using Garaget.Validation;
using Garaget.Vehicles;

namespace Garaget.Handlers
{
    public class GarageHandler : IHandler
    {
        private Garage<IVehicle> _garage;
        public int Count => _garage.Count; // antal parkerade fordon, direkt från garaget
        public int Capacity => _garage.Capacity; // totala antalet platser, direkt från garaget
        public bool IsFull => _garage.IsFull;

        public GarageHandler(int capacity)
        {
            _garage = new Garage<IVehicle>(capacity);
        }

        public void CreateNewGarage(int capacity)
        {
            _garage = new Garage<IVehicle>(capacity);
        }

        public bool Park(IVehicle vehicle)
        {
            return _garage.Park(vehicle); // returnerar true eller false från garagets Park-metod
        }

        public bool Remove(string regNumber)
        {
            return _garage.Remove(RegNumberValidation.Normalize(regNumber)); // städar regnumret och skickar vidare till garaget
        }

        public List<IVehicle> GetAllVehicles()
        {
            return _garage.ToList(); 
        }

        public IVehicle? FindByRegNumber(string regNumber)
        {
            return _garage.FindByRegNumber(RegNumberValidation.Normalize(regNumber)); // städar regnumret och skickar vidare till garaget
        }

        // Räknar hur många fordon det finns av varje typ i garaget.
        // Returnerar tex. car: 3, motorcycle: 2, bus: 1
        public List<TypeCount> GetTypeCounts()
        {

            var typeCounts = _garage
                .GroupBy(v => v.GetType().Name) // grupperar fordonen efter typnamn
                .Select(g => new TypeCount(g.Key, g.Count())) // skapar en TypeCount för varje grupp
                .ToList(); // konverterar till lista


            return typeCounts;
        }



        // Fritextsökning returnerar alla fordon där något fält matchar söktermen.
        // varje fordon avgör själv vad som matchar via sin egen Matches-metod (polymorf).
        public List<IVehicle> Search(string searchTerm)
        {

            var term = searchTerm.Trim();
            var searchResults = _garage.Where(v => v.Matches(term)).ToList();

            return searchResults;
        }

        // Sök på en eller flera egenskaper. Null betyder att egenskapen inte används i sökningen.
        public List<IVehicle> SearchByProperties(string? color = null, int? wheels = null, string? brand = null, string? type = null)
        {
            
            var matches = _garage.Where(v => MatchesProperties(v, color, wheels, brand, type)).ToList();

            return matches;
        }

       
        // Villkoret för SearchByProperties varje kriterium är antingen null (bryr oss inte) eller måste stämma
        private static bool MatchesProperties(IVehicle vehicle, string? color, int? wheels, string? brand, string? type)
        {

            return (color is null || vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                && (wheels is null || vehicle.Wheels == wheels)
                && (brand is null || vehicle.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase))
                && (type is null || vehicle.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }
}