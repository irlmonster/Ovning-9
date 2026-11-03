using Garaget.Handlers;
using Garaget.Vehicles;
using System.Text.Json;

namespace Garaget.UI
{
    public class MainMenu : IMenuInterface
    {
        private readonly IHandler _handler;

        public MainMenu(IHandler handler)
        {
            _handler = handler;
        }

        public async Task RunAsync()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                ConsoleOutput.WriteHeader($"=== Garaget ({_handler.Count}/{_handler.Capacity} platser) ===");
                Console.WriteLine();
                Console.WriteLine("1. Lista alla fordon");
                Console.WriteLine("2. Lista fordonstyper och antal");
                Console.WriteLine("3. Parkera fordon");
                Console.WriteLine("4. Hämta ut fordon");
                Console.WriteLine("5. Hitta fordon via regnummer");
                Console.WriteLine("6. Sök fordon");
                Console.WriteLine("7. Fyll garaget med testfordon");
                Console.WriteLine("8. Skapa nytt garage");
                Console.WriteLine("9. Spara eller ladda från fil");
                Console.WriteLine("0. Avsluta");
                ConsoleOutput.WriteDarkGray("Tryck Esc för att avbryta.");
                Console.WriteLine();
                Console.Write("Välj (0-9): ");
                Console.WriteLine();


                string input = Console.ReadLine() ?? "";
                Console.WriteLine();

                try
                {

                    switch (input)
                    {
                        case "1":
                            ListAllVehicles();
                            ConsoleOutput.Pause();
                            break;

                        case "2":
                            ListVehicleTypes();
                            ConsoleOutput.Pause();
                            break;

                        case "3":
                            ParkVehicle();
                            ConsoleOutput.Pause();
                            break;

                        case "4":
                            RemoveVehicle();
                            ConsoleOutput.Pause();
                            break;

                        case "5":
                            FindVehicle();
                            ConsoleOutput.Pause();
                            break;

                        case "6":
                            SearchVehicles();
                            ConsoleOutput.Pause();
                            break;

                        case "7":
                            PopulateGarage();
                            ConsoleOutput.Pause();
                            break;

                        case "8":
                            CreateNewGarage();
                            ConsoleOutput.Pause();
                            break;
                        case "9":
                            await FileMenuAsync();
                            ConsoleOutput.Pause();
                            break;

                        case "0":
                            isRunning = false;
                            ConsoleOutput.WriteSuccess("Hej då!");
                            break;

                        default:
                            ConsoleOutput.WriteError("Ogiltigt val, försök igen.");
                            ConsoleOutput.Pause();
                            break;
                    }
                }
                catch (UserCancelledException)
                {
                    ConsoleOutput.WriteWarning("Avbrutet.");
                    ConsoleOutput.Pause();
                }
            }
        }

        // Privata metoder, en per menyval

        private void ListAllVehicles()
        {
            ConsoleOutput.WriteHeader("Alla parkerade fordon:");

            var vehicles = _handler.GetAllVehicles();

            if (vehicles.Count == 0)
            {
                ConsoleOutput.WriteWarning("Garaget är tomt.");
                return;
            }

            foreach (var vehicle in vehicles)
            {
                Console.WriteLine(vehicle);
            }
        }

        private void ListVehicleTypes()
        {
            ConsoleOutput.WriteHeader("Alla fordonstyper i garaget:");

            var typeCounts = _handler.GetTypeCounts();

            if (typeCounts.Count == 0)
            {
                ConsoleOutput.WriteWarning("Garaget är tomt.");
                return;
            }

            foreach (var typeCount in typeCounts)
            {
                Console.WriteLine($"{typeCount.TypeName}: {typeCount.Count}");
            }

        }

        private void ParkVehicle()
        {
            ConsoleOutput.WriteHeader("Parkera fordon");

            // Kolla först, så användaren inte fyller i allt i onödan
            if (_handler.IsFull)
            {
                ConsoleOutput.WriteError("Garaget är fullt.");
                return;
            }

            // Undermeny: välj fordonstyp
            Console.WriteLine("1. Bil");
            Console.WriteLine("2. Motorcykel");
            Console.WriteLine("3. Buss");
            Console.WriteLine("4. Båt");
            Console.WriteLine("5. Flygplan");
            int choice = ConsoleInput.ReadInt("Välj fordonstyp (1-5): ", 1, 5);
            Console.WriteLine();

            // Regnummer först, och stoppa direkt om det redan finns
            string regNumber = ConsoleInput.ReadRegNumber("Regnummer: ");

            if (_handler.FindByRegNumber(regNumber) is not null)
            {
                ConsoleOutput.WriteError($"Ett fordon med regnummer {regNumber} finns redan i garaget.");
                return;
            }

            // Gemensamma egenskaper för alla fordon
            string color = ConsoleInput.ReadString("Färg: ");
            string brand = ConsoleInput.ReadString("Märke: ");
            int wheels = ConsoleInput.ReadInt("Antal hjul: ", 0, 50);

            // Typspecifika egenskaper, och skapa rätt sorts fordon.
            // Switch expression: varje gren returnerar ett nytt fordon som hamnar i vehicle.
            IVehicle vehicle = choice switch
            {
                1 => new Car(regNumber, color, wheels, brand,
                        ConsoleInput.ReadString("Bränsle: "),
                        ConsoleInput.ReadCarType("Välj biltyp: ")),

                2 => new Motorcycle(regNumber, color, wheels, brand,
                        ConsoleInput.ReadInt("Cylindervolym (cc): ", 50, 3000),
                        ConsoleInput.ReadMotorcycleType("Välj motorcykeltyp: ")),

                3 => new Bus(regNumber, color, wheels, brand,
                        ConsoleInput.ReadInt("Antal säten: ", 1, 200)),

                4 => new Boat(regNumber, color, wheels, brand,
                        ConsoleInput.ReadDouble("Längd (meter): ", 1, 500)),

                _ => new Airplane(regNumber, color, wheels, brand,
                        ConsoleInput.ReadInt("Antal motorer: ", 1, 8))
            };

            if (_handler.Park(vehicle))
            {
                ConsoleOutput.WriteSuccess($"Fordonet parkerades: {vehicle}");
            }
            else
            {
                ConsoleOutput.WriteError("Fordonet kunde inte parkeras.");
            }
        }

        private void RemoveVehicle()
        {
            ConsoleOutput.WriteHeader("Hämta ut fordon");
            string regNumber = ConsoleInput.ReadRegNumber("Ange regnummer: ");

            // Hämta ut fordonet först sedan visa vad som togs ut
            var vehicle = _handler.FindByRegNumber(regNumber);

            if (vehicle is null)
            {
                ConsoleOutput.WriteError($"Inget fordon med regnummer {regNumber} finns i garaget.");
                return;
            }

            _handler.Remove(regNumber);
            ConsoleOutput.WriteSuccess($"Fordonet har hämtats ut: {vehicle}");

        }

        private void FindVehicle()
        {
            ConsoleOutput.WriteHeader("Leta efter fordon baserat på regnummer");
            string regNumber = ConsoleInput.ReadRegNumber("Ange regnummer: ");

            var vehicle = _handler.FindByRegNumber(regNumber);

            if (vehicle is null)
            {
                ConsoleOutput.WriteWarning($"Inget fordon med regnummer {regNumber} finns i garaget.");
                return;
            }

            ConsoleOutput.WriteSuccess("Fordonet hittades: ");
            Console.WriteLine(vehicle);



        }

        // Låter användaren söka fordon, antingen med fritext eller på kombinerade egenskaper
        private void SearchVehicles()
        {
            ConsoleOutput.WriteHeader("Sök fordon");

            // Undermeny: välj söksätt
            Console.WriteLine("1. Fritext (söker i alla egenskaper)");
            Console.WriteLine("2. Egenskaper (kombinera en eller flera)");
            int choice = ConsoleInput.ReadInt("Välj (1-2): ", 1, 2);
            Console.WriteLine();

            // Deklareras här utanför if-satsen, så båda grenarna kan fylla den
            // och den finns kvar för utskriften längre ner
            List<IVehicle> result;

            if (choice == 1)
            {
                // Fritext: varje fordon avgör själv via Matches om sökordet matchar
                string term = ConsoleInput.ReadString("Sökord: ");
                result = _handler.Search(term);
            }
            else
            {
                // Egenskaper: tomt svar blir null, och null betyder "bryr mig inte om den här"
                // Namngivna argument gör det tydligt vilket värde som hör till vilken parameter
                Console.WriteLine("Lämna tomt för att hoppa över en egenskap.");
                result = _handler.SearchByProperties(
                    color: ConsoleInput.ReadOptionalString("Färg: "),
                    wheels: ConsoleInput.ReadOptionalInt("Antal hjul: ", 0, 50),
                    brand: ConsoleInput.ReadOptionalString("Märke: "),
                    type: ConsoleInput.ReadOptionalString("Typ (Car, Motorcycle, Bus, Boat, Airplane): "));
            }

            Console.WriteLine();

            // Inga träffar: varna och gå ut
            if (result.Count == 0)
            {
                ConsoleOutput.WriteWarning("Inga fordon matchade sökningen.");
                return;
            }

            // Visa antal träffar och skriv ut varje fordon (använder fordonets ToString)
            ConsoleOutput.WriteSuccess($"{result.Count} fordon hittades:");
            foreach (var vehicle in result)
            {
                Console.WriteLine(vehicle);
            }
        }

        // Undermeny för fil: väljer spara eller ladda
        private async Task FileMenuAsync()
        {
            ConsoleOutput.WriteHeader("Spara eller ladda garaget");
            Console.WriteLine("1. Spara garaget");
            Console.WriteLine("2. Ladda garaget");
            Console.WriteLine("0. Tillbaka");

            int choice = ConsoleInput.ReadInt("Välj (0-2): ", 0, 2);
            Console.WriteLine();

            if (choice == 1)
            {
                await SaveGarageAsync();
            }
            else if (choice == 2)
            {
                await LoadGarageAsync();
            }
            // 0 gör ingenting, vi går bara tillbaka till huvudmenyn
        }

        private async Task SaveGarageAsync()
        {
            try
            {
                await _handler.SaveAsync();
                ConsoleOutput.WriteSuccess($"{_handler.Count} fordon sparades till fil.");
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleOutput.WriteError("Programmet har inte behörighet att skriva filen.");
            }
            catch (IOException)
            {
                ConsoleOutput.WriteError("Filen kunde inte skrivas. Den kan vara öppen i ett annat program.");
            }
        }


        private async Task LoadGarageAsync()
        {
            ConsoleOutput.WriteWarning("Det nuvarande garaget och alla fordon i det ersätts.");

            try
            {
                await _handler.LoadAsync();
                ConsoleOutput.WriteSuccess($"{_handler.Count} fordon laddades. Garaget har {_handler.Capacity} platser.");
            }
            catch (FileNotFoundException)
            {
                ConsoleOutput.WriteError("Det finns inget sparat garage än.");
            }
            catch (JsonException)
            {
                ConsoleOutput.WriteError("Filen är trasig och kunde inte läsas.");
            }
            catch (InvalidOperationException)
            {
                ConsoleOutput.WriteError("Filen innehåller inget garage.");
            }
            catch (ArgumentException)
            {
                ConsoleOutput.WriteError("Filen innehåller ett fordon med ogiltiga värden.");
            }
        }



        private void PopulateGarage()
        {
            ConsoleOutput.WriteHeader("Fyller garaget med testfordon");

            // En Lista med färdiga fordon. Måsvingarna fyller den direkt vid skapandet.
            var vehicles = new List<IVehicle>()
            {
                new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi),
                new Car("ABC321", "Lila", 4, "Ford", "Bensin", CarType.Sedan),
                new Car("ATR123", "Vit", 4, "Tesla", "El", CarType.Suv),
                new Car("PPC123", "Röd", 4, "SAAB", "Diesel", CarType.Cab),
                new Car("WKA727", "Svart", 4, "Volvo", "Bensin", CarType.Sedan),
                new Bus("BUS001", "Gul", 6, "Scania", 50),
                new Bus("BSS001", "Blå", 10, "Volvo", 50),
                new Motorcycle("EXC500", "Orange", 2, "KTM", 500, MotorcycleType.Enduro),
                new Boat("BTA125", "Vit", 0, "Flipper", 12.5),
                new Airplane("FLY747", "Vit", 18, "Boeing", 4)
            };

            // Försök parkera varje fordon, räkna hur många som lyckades
            int parked = 0;
            foreach (var vehicle in vehicles)
            {
                if (_handler.Park(vehicle))
                {
                    parked++;
                }
            }

            ConsoleOutput.WriteSuccess($"{parked} fordon parkerades.");

            // Om inte alla fick plats
            if (parked < vehicles.Count)
            {
                ConsoleOutput.WriteWarning($"{vehicles.Count - parked} fordon kunde inte parkeras (garaget är fullt eller regnumret finns redan).");
            }
        }

        private void CreateNewGarage()
        {
            ConsoleOutput.WriteHeader("Skapa nytt garage");
            ConsoleOutput.WriteWarning("Det nuvarande garaget och alla fordon i det försvinner.");

            int capacity = ConsoleInput.ReadInt("Antal platser i nya garaget: ", 1, 1000);
            _handler.CreateNewGarage(capacity);

            ConsoleOutput.WriteSuccess($"Nytt garage med {capacity} platser har skapats.");
        }
    }
}