using Garaget.Storage;
using Garaget.Vehicles;

namespace Garaget.Tests
{
    public class JsonGarageStorageTests
    {

        [Fact]
        public async Task SaveAndLoad_WithVehicles_ReturnsSameData()
        {

            // Arrange
            var filePath = Path.Combine(Path.GetTempPath(), $"garage-{Guid.NewGuid()}.json");
            var storage = new JsonGarageStorage(filePath);
            var garageData = new GarageData
            {
                Capacity = 2,
                Vehicles = new List<IVehicle>
                {
                    new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi),
                    new Bus("XYZ789", "Blå", 6, "Scania", 50)
                }
            };
            // Act
            await storage.SaveAsync(garageData);
            var loadedData = await storage.LoadAsync();
            // Assert
            Assert.Equal(garageData.Capacity, loadedData.Capacity);
            Assert.Equal(garageData.Vehicles.Count, loadedData.Vehicles.Count);
            Assert.IsType<Car>(loadedData.Vehicles[0]);
            Assert.IsType<Bus>(loadedData.Vehicles[1]);
            File.Delete(filePath);
        }


    }
}
