using Garaget.Garages;
using Garaget.Vehicles;

namespace Garaget.Tests
{
    public class GarageTests
    {
       /* [Fact]
        public void Park_WhenSpaceAvailable_ReturnsTrue()
        {
            var garage = new Garage(2);

            var result = garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            Assert.True(result);
            Assert.Equal(1, garage.Count);
      
        }

        [Fact]
        public void Park_WhenFull_ReturnsFalse()
        {
            var garage = new Garage(1);
            garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            var result = garage.Park(new Car("XYZ789", "Blå", 4, "Tesla", "El", CarType.Halvkombi));

            Assert.False(result);
            Assert.Equal(1, garage.Count);
        }

        [Fact]
        public void Park_DuplicateRegNumber_ReturnsFalse()
        {
            var garage = new Garage(2);
            garage.Park(new Car("ABC123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));

            var result = garage.Park(new Bus("abc123", "Gul", 6, "Scania", 50));


            Assert.False(result);
            Assert.Equal(1, garage.Count);
        }


        [Fact]
        public void Remove_ExistingVehicle_FreeSpace()
        {
            var garage = new Garage(1);
            garage.Park (new Car("ABC123", "Gul", 4, "Tesla", "El", CarType.Halvkombi));

            var result = garage.Remove("abc123");

            Assert.True(result);
            Assert.Equal(0, garage.Count);

        }

        [Fact]
        public void Constructor_ZeroCapacity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Garage(0));
        }



        */
    }
}
