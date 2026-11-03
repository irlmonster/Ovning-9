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


            // regnumret finns redan
            if (FindByRegNumber(vehicle.RegNumber) is not null)
            {
                return false;
            }

            _vehicles.Add(vehicle);
            return true;
        }

        // Remove
        public bool Remove(string regNumber)
        {
            var vehicle = FindByRegNumber(regNumber);
            return vehicle is not null && _vehicles.Remove(vehicle);
        }


        // Find regnumber
        public T? FindByRegNumber(string regNumber)
        {
            var result = _vehicles.FirstOrDefault(v => v.RegNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase));
            return result;
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