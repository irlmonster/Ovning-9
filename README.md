# Garage 3.0

En konsolapplikation i C# som simulerar ett parkeringsgarage. Övning 5 i .NET-utbildningen hos Lexicon.

Nu är det `version 3`, jag använder nu generics och LINQ.

## Funktioner

- Skapa ett garage med valfri storlek vid start, eller ett nytt via menyn
- Parkera fordon via en undermeny för varje fordonstyp
- Hämta ut fordon via registreringsnummer
- Lista alla parkerade fordon
- Lista vilka fordonstyper som står i garaget och hur många av varje
- Hitta ett fordon via registreringsnummer (ABC123, abc123 och AbC123 fungerar)
- Sök med fritext i alla egenskaper, även fordonsspecifika (t.ex. "500" hittar en motorcykel med 500 cc)
- Sök på en eller flera egenskaper i kombination, t.ex. alla svarta fordon med fyra hjul
- Fyll garaget med testfordon
- Färgad feedback: grönt när något gick bra, rött vid fel, gult vid varningar
- Robust inmatning: programmet frågar igen vid felaktig input istället för att krascha

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

- **.NET 10 / C#**, **xUnit** för enhetstester
- Garaget lagrar fordonen i en privat List<IVehicle>. 
- Abstrakt basklass `Vehicle` med gemensamma egenskaper, en subklass per fordonstyp
- Fritextsökningen använder polymorfism: varje fordon har en egen `Matches`-metod som anropar basklassens och lägger till sina egna fält
- Registreringsnummer valideras med regex (tre bokstäver, två siffror, en siffra eller bokstav) och normaliseras till versaler
- `GarageHandler` är mellanlagret mellan menyn och garaget, så menyn aldrig pratar direkt med garaget
- All färgad utskrift och all inmatning är samlad i `ConsoleOutput` och `ConsoleInput`

## Struktur

```
Garaget/
├── Garaget/
│   ├── Garages/        Garage, själva samlingen av fordon
│   ├── Handlers/       GarageHandler IHandler och TypeCount
│   ├── UI/             MainMenu, ConsoleInput, ConsoleOutput och IMenuInterface
│   ├── Validation/     RegNumberValidation
│   └── Vehicles/       IVehicle, Vehicle, subklasserna och enums
└── Garaget.Tests/
                        Tester för garage, handler, fordon och validering
```

## Kom igång

```bash
git clone https://github.com/irlmonster/Ovning-9
cd Garaget
dotnet run --project Garaget
```

Köra testerna:

```bash
dotnet test
```
