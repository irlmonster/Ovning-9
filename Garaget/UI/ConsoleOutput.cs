using Garaget.Vehicles;

namespace Garaget.UI
{
    // Hjälpklass för färgad utskrift i konsolen
    public static class ConsoleOutput
    {
        // Grön text för när något gick bra
        public static void WriteSuccess(string message) => WriteInColor(message, ConsoleColor.Green);

        // Röd text för när något gick snett
        public static void WriteError(string message) => WriteInColor(message, ConsoleColor.Red);

        // Gul text för varningar
        public static void WriteWarning(string message) => WriteInColor(message, ConsoleColor.Yellow);

        // Cyan text för rubriker
        public static void WriteHeader(string message) => WriteInColor(message, ConsoleColor.Cyan);

        // DarkGray text för simpelare meddelande eller info
        public static void WriteDarkGray(string message) => WriteInColor(message, ConsoleColor.DarkGray);



        // Stoppar programmet tills användaren tryckt på en tangent,
        // så att utskrifter hinner läsas innan skärmen rensas
        public static void Pause()
        {
            WriteInColor("\nTryck på valfri tangent för att fortsätta...", ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        // Skriver ut fordon som en tabell med raka kolumner.
        // Bredd efter kommatecknet: negativt = vänsterjusterat, positivt = högerjusterat.
        public static void WriteVehicleTable(IEnumerable<IVehicle> vehicles)
        {
            // Rubrikraden
            Console.WriteLine($"{"Typ",-11}{"Regnr",-8}{"Märke",-16}{"Färg",-8}{"Hjul",5}  Detaljer");

            // Streck under rubriken
            Console.WriteLine(new string('-', 70));

            // En rad per fordon, med samma bredder som rubriken
            foreach (var v in vehicles)
            {
                Console.WriteLine($"{v.GetType().Name,-11}{v.RegNumber,-8}{v.Brand,-16}{v.Color,-8}{v.Wheels,5}  {v.Details}");
            }
        }


        // Gemensam logik: sätt färg, skriv, återställ.
        // Enda stället som rör Console.ForegroundColor.
        private static void WriteInColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}