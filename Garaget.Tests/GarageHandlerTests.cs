using Garaget.Handlers;
using Garaget.Vehicles;
using Garaget.Storage;

namespace Garaget.Tests
{
    public class GarageHandlerTests
    {
        // Hjälpmetod: skapar en handler med samma fordon till alla tester,
        // så vi slipper upprepa parkeringen i varje test
        private static GarageHandler CreateHandlerWithVehicles()
        {

            var filePath = Path.Combine(Path.GetTempPath(), $"garage-{Guid.NewGuid()}.json");
            var storage = new JsonGarageStorage(filePath);
            var handler = new GarageHandler(10, storage);

            handler.Park(new Car("AAA111", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));
            handler.Park(new Car("BBB222", "Röd", 4, "Saab", "Diesel", CarType.Sedan));
            handler.Park(new Car("CCC333", "Svart", 4, "Volvo", "El", CarType.Suv));
            handler.Park(new Bus("DDD444", "Röd", 6, "Scania", 50));
            handler.Park(new Bus("EEE555", "Gul", 6, "Volvo", 40));
            handler.Park(new Motorcycle("FFF666", "Svart", 2, "Husaberg", 500, MotorcycleType.Enduro));

            return handler;
        }

        [Fact]
        public void SearchByProperties_RedCar_ReturnsOnlyRedCars()
        {
            var handler = CreateHandlerWithVehicles();

            var result = handler.SearchByProperties(color: "Röd", type: "Car");

            // Två röda bilar, den röda bussen ska inte komma med
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void SearchByProperties_BlackWithFourWheels_ReturnsOnlyBlackCar()
        {
            var handler = CreateHandlerWithVehicles();

            var result = handler.SearchByProperties(color: "Svart", wheels: 4);

            // Svarta bilen ska med, svarta motorcykeln har 2 hjul och ska inte med
            Assert.Single(result);
        }

        [Fact]
        public void SearchByProperties_OnlyType_ReturnsAllOfThatType()
        {
            var handler = CreateHandlerWithVehicles();

            var result = handler.SearchByProperties(type: "bus");

            // Båda bussarna, oavsett färg. Små bokstäver ska också fungera
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void SearchByProperties_NoCriteria_ReturnsAllVehicles()
        {
            var handler = CreateHandlerWithVehicles();

            var result = handler.SearchByProperties();

            // Inga kriterier betyder att allt matchar
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public void SearchByProperties_NoMatch_ReturnsEmpty()
        {
            var handler = CreateHandlerWithVehicles();

            var result = handler.SearchByProperties(color: "Rosa");

            Assert.Empty(result);
        }



        [Fact]
        public void GetTypeCounts_MixedVehicles_ReturnsCorrectCountPerType()
        {
            var handler = CreateHandlerWithVehicles();

            var result = handler.GetTypeCounts();

            // Tre olika typer i garaget
            Assert.Equal(3, result.Count);

            // Ordningen följer när typen först parkerades: bilar, bussar, motorcykel
            Assert.Equal(new TypeCount("Car", 3), result[0]);
            Assert.Equal(new TypeCount("Bus", 2), result[1]);
            Assert.Equal(new TypeCount("Motorcycle", 1), result[2]);
        }

        [Fact]
        public async Task SaveAndLoad_WithVehicles_RestoresGarage()
        {
            // Arrange
            var filePath = Path.Combine(Path.GetTempPath(), $"garage-{Guid.NewGuid()}.json");
            var storage = new JsonGarageStorage(filePath);
            var handler = new GarageHandler(10, storage);
            handler.Park(new Car("AAA111", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));
            handler.Park(new Bus("DDD444", "Röd", 6, "Scania", 50));
            
            // Act
            await handler.SaveAsync();
            var loadedHandler = new GarageHandler(1, storage);
            await loadedHandler.LoadAsync();
            
            // Assert
            Assert.Equal(2, loadedHandler.Count);
            Assert.Equal(10, loadedHandler.Capacity);
            File.Delete(filePath);
        }


        [Fact]
        public async Task Load_FileMissing_KeepsCurrentGarage()
        {
            // Arrange
            var filePath = Path.Combine(Path.GetTempPath(), $"garage-{Guid.NewGuid()}.json");
            var handler = new GarageHandler(5, new JsonGarageStorage(filePath));
            handler.Park(new Car("AAA111", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            // Act
            await Assert.ThrowsAsync<FileNotFoundException>(async () => await handler.LoadAsync());

            // Assert
            Assert.Equal(1, handler.Count);
            Assert.Equal(5, handler.Capacity);  

        }
    }
}