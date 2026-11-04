# Garage 3.0

En konsolapplikation i C# som simulerar ett parkeringsgarage. Övning 9 i .NET-utbildningen hos Lexicon.

I version 3 har jag gjort om garaget till en generisk samling med LINQ, brutit ut UI och handler till interfaces, och lagt till att spara och ladda garaget som JSON med async och await.

## Funktioner

- Skapa ett garage med valfri storlek vid start, eller ett nytt via menyn
- Parkera fordon via en undermeny för varje fordonstyp
- Hämta ut fordon via registreringsnummer
- Lista alla parkerade fordon i en tabell med raka kolumner
- Lista vilka fordonstyper som står i garaget och hur många av varje
- Hitta ett fordon via registreringsnummer (ABC123, abc123 och AbC123 fungerar)
- Sök med fritext i alla egenskaper, även fordonsspecifika (t.ex. "500" hittar en motorcykel med 500 cc)
- Sök på en eller flera egenskaper i kombination, t.ex. alla svarta fordon med fyra hjul
- Fyll garaget med testfordon
- Spara garaget till fil och ladda det igen (JSON), via menyval 9
- Avbryt ett påbörjat menyval med Esc och kom tillbaka till huvudmenyn
- Färgad feedback: grönt när något gick bra, rött vid fel, gult vid varningar
- Robust inmatning: programmet frågar igen vid felaktig input istället för att krascha

--> (det ligger även en `garage.json` i projektmappen med 1000 fordon som man kan ladda och testa runt med) <--

## Fordonstyper

| Typ | Egen egenskap |
|---|---|
| `Car` | Bränsle, biltyp (Sedan, Kombi, Halvkombi, Suv, Cab) |
| `Motorcycle` | Cylindervolym, motorcykeltyp (Sport, Naked, Touring, Cruiser, Enduro) |
| `Bus` | Antal säten |
| `Boat` | Längd |
| `Airplane` | Antal motorer |

Alla fordon har registreringsnummer, färg, märke och antal hjul.

## Teknik och upplägg

**Allmänt**

- **.NET 10 / C#**, **xUnit** för enhetstester
- Abstrakt basklass `Vehicle` med gemensamma egenskaper, en subklass per fordonstyp
- Polymorfism: varje fordon har sin egen `Matches`-metod för fritextsökningen och sin egen `Details`-egenskap för tabellutskriften
- Registreringsnummer valideras med regex (tre bokstäver, två siffror, en siffra eller bokstav) och normaliseras till versaler

**Generics och LINQ**

- `Garage<T>` är generiskt med constrainten `where T : class, IVehicle` och lagrar fordonen i en privat `List<T>`
- Garaget implementerar `IEnumerable<T>` med `yield return`, så man kan använda `foreach` och LINQ direkt på garaget
- I `Garage` används `FirstOrDefault` för att hitta ett fordon på regnummer. `Park` och `Remove` använder samma metod, så regeln för regnummer finns på ett ställe
- I `GarageHandler` används LINQ för sökning och gruppering (`Where`, `GroupBy`, `Select`, `ToList`)

**Lager och interfaces**

- `GarageHandler` är mellanlagret mellan menyn och garaget, så menyn aldrig pratar direkt med garaget
- Menyn och handlern används via interfaces: `IMenuInterface` och `IHandler`
- `Program.cs` skapar lagringen, handlern och menyn och kopplar ihop dem (dependency injection för hand). Det är det enda stället som känner till de konkreta klasserna

**Spara och ladda**

- Lagringen ligger bakom interfacet `IGarageStorage`, med `JsonGarageStorage` som implementation. En annan lagring (t.ex. CSV) blir en ny klass, och bara en rad i `Program.cs` behöver ändras
- `GarageData` håller det som sparas: kapaciteten och fordonen
- `System.Text.Json` med `[JsonPolymorphic]` och `[JsonDerivedType]` på `IVehicle`, så att varje fordon får ett `"$type"`-fält i filen och blir rätt klass igen vid inläsning
- Async och await hela vägen från `Main` ner till `File.WriteAllTextAsync` och `File.ReadAllTextAsync`
- Filen läses in innan garaget byts ut, så om filen saknas eller är trasig finns det nuvarande garaget kvar
- Felhantering med begripliga meddelanden för saknad fil, trasig JSON och ogiltiga värden

**Inmatning och utskrift**

- All färgad utskrift och all inmatning är samlad i `ConsoleOutput` och `ConsoleInput`
- `ReadLineOrCancel` läser en tangent i taget med `Console.ReadKey`, så att Esc kan upptäckas. Esc kastar `UserCancelledException`, som fångas i huvudmenyn
- Fordonslistor skrivs ut som en tabell med fasta kolumnbredder via interpolerade strängar (`{värde,-11}`)

## Struktur

```
Ovning-9/
├── Garaget/
│   ├── Garages/        Garage<T>, själva samlingen av fordon
│   ├── Handlers/       GarageHandler, IHandler och TypeCount
│   ├── Storage/        IGarageStorage, JsonGarageStorage och GarageData
│   ├── UI/             MainMenu, IMenuInterface, ConsoleInput, ConsoleOutput och UserCancelledException
│   ├── Validation/     RegNumberValidation
│   └── Vehicles/       IVehicle, Vehicle, subklasserna och enums
└── Garaget.Tests/      Tester för garage, handler, lagring, fordon och validering
```

## Kom igång

```bash
git clone https://github.com/irlmonster/Ovning-9
cd Ovning-9
dotnet run --project Garaget
```
Det sparade garaget hamnar i `garage.json` i samma mapp som programmet körs från (t.ex. `Garaget/bin/Debug/net10.0/`).

Köra testerna:

```bash
dotnet test
```

## Tester

- **Garage:** parkera, fullt garage, dubbletter, ta bort, `foreach` över garaget, hitta på regnummer oavsett stora och små bokstäver, och att `Garage<Car>` ger tillbaka en `Car` utan cast
- **GarageHandler:** sökning på egenskaper, gruppering per fordonstyp, spara och ladda via handlern, och att garaget finns kvar när filen saknas
- **JsonGarageStorage:** att fordon sparas och läses in som rätt typ
- **Fordon och validering:** regnummerformat, normalisering och fritextsökning

## Möjliga förbättringar

- Olika stora platser, så att t.ex. en motorcykel tar en tredjedels plats och ett flygplan tre
- Spara och ladda som CSV, som en ny klass bakom `IGarageStorage`
- Bekräftelse (J/N) innan något går förlorat, t.ex. vid Esc mitt i en parkering eller innan ett sparat garage skriver över det nuvarande