using System.Globalization;

namespace Garaget.Vehicles
{
    public class Boat : Vehicle
    {
        public double Length { get; }
        public override string Details => $"{Length} m";

        public Boat(string regNumber, string color, int wheels, string brand, double length) : base(regNumber, color, wheels, brand)
        {
            Length = length;
        }


        // Kollar basklassens fält, sen båtens längd.
        // Söktermen görs om till ett tal istället för att göra längden till text.
        public override bool Matches(string searchTerm)
        {
            return base.Matches(searchTerm)
                || (double.TryParse(searchTerm.Replace(',', '.'),
                                    NumberStyles.Float,
                                    CultureInfo.InvariantCulture,
                                    out var length)
                    && Length == length);
        }

        public override string ToString() => $"{base.ToString()}, längd: {Length}";
    }
}
