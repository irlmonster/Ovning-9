using Garaget.Vehicles;
using System.Collections;

namespace Garaget.Garages
{
    public class Garage<T> : IEnumerable<T> where T : class, IVehicle
    {
        private readonly List<T> _vehicles;

        public int Capacity { get; }
        public int Count => _vehicles.Count;
        public bool IsFull => Count >= Capacity;

        public Garage(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Kapaciteten måste vara större än 0.");
            }

            Capacity = capacity; // sätter det här objektet till den listan
            _vehicles = new List<T>();
        }

        // Park
        public bool Park(T vehicle)
        {
            if (IsFull) return false;

            foreach (var v in _vehicles)
            {
                // jämför regnumren som strängar, oavsett stor/liten bokstav
                if (v.RegNumber.Equals(vehicle.RegNumber, StringComparison.OrdinalIgnoreCase))
                    return false; // regnumret finns redan
            }

            _vehicles.Add(vehicle); // lägger fordonet sist i listan
            return true;
        }

        // Remove
        public bool Remove(string regNumber)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.RegNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase)); // FirstOrDefault returnerar null om inget matchar
            return vehicle is not null && _vehicles.Remove(vehicle);
        }


        // Find regnumber
        public T? FindByRegNumber(string regNumber)
        {
            foreach (var v in _vehicles)
            {
                if (v.RegNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase))
                    return v; // hittade fordonet
            }
            return null; // inget fordon med det regnumret
        }



        public IEnumerator<T> GetEnumerator()
        {
            foreach (var vehicle in _vehicles)
            {
                yield return vehicle;
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}