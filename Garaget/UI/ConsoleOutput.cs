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