using Garaget.Validation;
using Garaget.Vehicles;

namespace Garaget.Handlers
{
    public class GarageHandler
    {
        private readonly Garage _garage;

        public GarageHandler(int capacity)
        {
            _garage = new Garage(capacity);
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
            return _garage.GetAllVehicles(); // returnerar en kopia av garagets List
        }

        public IVehicle? FindByRegNumber(string regNumber)
        {
            return _garage.FindByRegNumber(RegNumberValidation.Normalize(regNumber)); // städar regnumret och skickar vidare till garaget
        }

        public int Count => _garage.Count; // antal parkerade fordon, direkt från garaget

        public int Capacity => _garage.Capacity; // totala antalet platser, direkt från garaget



        // Räknar hur många fordon det finns av en viss typ
        public int CountByType(string typeName)
        {
            int count = 0;

            foreach (var vehicle in GetAllVehicles())
            {
                if (vehicle.GetType().Name == typeName)
                {
                    count++;
                }
            }

            return count;
        }

        // Räknar hur många fordon det finns av varje typ i garaget.
        // Returnerar t.ex. [Car: 3, Motorcycle: 2, Bus: 1]
        public List<TypeCount> GetTypeCounts()
        {
            var vehicles = GetAllVehicles(); // en kopia av alla parkerade fordon
            var typeNames = new List<string>(); // här samlas varje typnamn, bara en gång per typ

            // först ta reda på vilka typer som finns
            foreach (var vehicle in vehicles)
            {
                var typeName = vehicle.GetType().Name; // klassens namn, t.ex. "Car" eller "Bus"

                // lägg bara till typen om vi inte redan sett den, så blir det inga dubbletter
                if (!typeNames.Contains(typeName))
                    typeNames.Add(typeName);
            }

            // sedan räkna antal fordon för varje typ vi hittade
            var result = new List<TypeCount>();
            foreach (var typeName in typeNames)
            {
                // Skapar ett TypeCount med typnamnet och antalet, och lägger det i resultatet
                result.Add(new TypeCount(typeName, CountByType(typeName)));
            }

            return result;
        }



        // Fritextsökning: returnerar alla fordon där något fält matchar söktermen.
        // Varje fordon avgör själv vad som matchar via sin egen Matches-metod (polymorfism).
        public List<IVehicle> Search(string searchTerm)
        {
            var term = searchTerm.Trim();
            var vehicles = GetAllVehicles();


            // Tar bort den tidigare loopen och går bara igenom listan med vehicles
            var result = new List<IVehicle>();
            foreach (var vehicle in vehicles)
            {
                if (vehicle.Matches(term))
                {
                    result.Add(vehicle);
                }
            }

            return result;
        }

        // Sök på en eller flera egenskaper. Null betyder att egenskapen inte används i sökningen.
        public List<IVehicle> SearchByProperties(string? color = null, int? wheels = null, string? brand = null, string? type = null)
        {
            var vehicles = GetAllVehicles();
            var result = new List<IVehicle>();

            foreach (var vehicle in vehicles)
            {
                if (MatchesProperties(vehicle, color, wheels, brand, type))
                {
                    result.Add(vehicle);
                }
            }

            return result;
        }

        //Denna står kvar eftersom den inte gör något med arrayer.
        // Villkoret för SearchByProperties: varje kriterium är antingen null (bryr oss inte) eller måste stämma
        private static bool MatchesProperties(IVehicle vehicle, string? color, int? wheels, string? brand, string? type)
        {
            return (color is null || vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                && (wheels is null || vehicle.Wheels == wheels)
                && (brand is null || vehicle.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase))
                && (type is null || vehicle.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }
}