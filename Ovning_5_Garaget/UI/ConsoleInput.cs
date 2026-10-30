using Garaget.Validation;
using Garaget.Vehicles;
using System.Globalization;

namespace Garaget.UI
{
    // Hjälpklass för inläsning från konsolen.
    // Varje metod frågar igen tills svaret är giltigt, så programmet aldrig kraschar på dålig input.
    public static class ConsoleInput
    {
        // Läser ett regnummer och returnerar det i normaliserat format (t.ex. "ABC123")
        public static string ReadRegNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (RegNumberValidation.IsValid(input))
                {
                    return RegNumberValidation.Normalize(input!); // här litar vi på att det inte är null eftersom vi kollat det tidigare
                }

                ConsoleOutput.WriteError("Ogiltigt regnummer. Formatet är tre bokstäver, två siffror och en siffra eller bokstav (tex. ABC123).");
            }
        }

        // Läser ett heltal mellan min och max (inklusive). Frågar igen tills det är giltigt.
        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                ConsoleOutput.WriteError($"Ange ett heltal mellan {min} och {max}.");
            }
        }


        // Läser en sträng som inte får vara tom
        public static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                ConsoleOutput.WriteError("Fältet får inte vara tomt.");
            }
        }



        // Läser en valfri sträng. Tomt svar ger null, vilket betyder "hoppa över"
        public static string? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            return string.IsNullOrWhiteSpace(input) ? null : input.Trim(); // ifsats på en rad "är den tom? I så fall null, annars den trimmade strängen."
        }





        // Läser ett valfritt heltal. Tomt svar ger null, annars samma kontroll som ReadInt
        public static int? ReadOptionalInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                ConsoleOutput.WriteError($"Ange ett heltal mellan {min} och {max}, eller lämna tomt.");
            }
        }




        // Läser ett decimaltal. Godkänner både komma och punkt
        public static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input is not null
                    && double.TryParse(input.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                    && value >= min && value <= max)
                {
                    return value;
                }

                ConsoleOutput.WriteError($"Ange ett tal mellan {min} och {max}.");
            }
        }





        // Visar alla biltyper som en numrerad lista och låter användaren välja en
        public static CarType ReadCarType(string prompt)
        {
            string[] names = Enum.GetNames(typeof(CarType));

            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {names[i]}");
            }

            int choice = ReadInt(prompt, 1, names.Length);
            return (CarType)(choice - 1);
        }




        // Samma sak för motorcykeltyper
        public static MotorcycleType ReadMotorcycleType(string prompt)
        {
            string[] names = Enum.GetNames(typeof(MotorcycleType));

            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {names[i]}");
            }

            int choice = ReadInt(prompt, 1, names.Length);
            return (MotorcycleType)(choice - 1);
        }
    }
}