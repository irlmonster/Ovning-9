using Garaget.Garages;
using Garaget.Vehicles;

namespace Garaget.Tests
{
    public class GarageTests
    {
        [Fact]
        public void Park_WhenSpaceAvailable_ReturnsTrue()
        {
            var garage = new Garage<IVehicle>(2);

            var result = garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            Assert.True(result);
            Assert.Equal(1, garage.Count);
      
        }

        [Fact]
        public void Park_WhenFull_ReturnsFalse()
        {
            var garage = new Garage<IVehicle>(1);
            garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            var result = garage.Park(new Car("XYZ789", "Blå", 4, "Tesla", "El", CarType.Halvkombi));

            Assert.False(result);
            Assert.Equal(1, garage.Count);
        }

        [Fact]
        public void Park_DuplicateRegNumber_ReturnsFalse()
        {
            var garage = new Garage<IVehicle>(2);
            garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            var result = garage.Park(new Bus("abc123", "Gul", 6, "Scania", 50));


            Assert.False(result);
            Assert.Equal(1, garage.Count);
        }


        [Fact]
        public void Remove_ExistingVehicle_FreeSpace()
        {
            var garage = new Garage<IVehicle>(1);
            garage.Park (new Car("ABC123", "Gul", 4, "Tesla", "El", CarType.Halvkombi));

            var result = garage.Remove("abc123");

            Assert.True(result);
            Assert.Equal(0, garage.Count);

        }

        [Fact]
        public void Constructor_ZeroCapacity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<IVehicle>(0));
        }


        [Fact]
        public void Enumerate_WithParkedVehicles_ReturnsAllVehicles()
        {
            var garage = new Garage<IVehicle>(5);
            var v1 = new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi);
            var v2 = new Bus("XYZ789", "Blå", 6, "Scania", 50);
            var v3 = new Car("ABd124", "Gul", 4, "Kia", "El", CarType.Kombi);
            var v4 = new Bus("hYZ785", "Blå", 6, "MadMax", 50);
            var v5 = new Car("AdC543", "Vit", 4, "Mini", "Bensin", CarType.Kombi);

            
            foreach (var v in new IVehicle[] { v1, v2, v3, v4, v5 })
            {
                garage.Park(v);
            }

            var vehicles = garage.ToList();

            Assert.Equal(5, vehicles.Count);
            Assert.Contains(v1, vehicles);
            Assert.Contains(v2, vehicles);
            Assert.Contains(v3, vehicles);
            Assert.Contains(v4, vehicles);
            Assert.Contains(v5, vehicles);

        }

        [Fact]
        public void Enumerate_EmptyGarage_ReturnsNothing()
        {
            var garage = new Garage<IVehicle>(5);
            Assert.Empty(garage);
        }

        [Fact]
        public void Remove_NonExistingRegNumber_ReturnsFalse()
        {
            var garage = new Garage<IVehicle>(5);
            garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));
            var result = garage.Remove("XYZ789");
            Assert.False(result);
            Assert.Equal(1, garage.Count);
        }


        [Fact]
        public void Enumerate_AfterRemove_DoesNotIncludeRemovedVehicle()
        {
            var garage = new Garage<IVehicle>(5);
            var v1 = new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi);
            var v2 = new Bus("XYZ789", "Blå", 6, "Scania", 50);

            garage.Park(v1);
            garage.Park(v2);

            garage.Remove(v1.RegNumber);

            var vehicles = garage.ToList();

            Assert.Single(vehicles);
            Assert.DoesNotContain(v1, vehicles);
            Assert.Contains(v2, vehicles);
        }
    }
}
