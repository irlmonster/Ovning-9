using Garaget.Vehicles;

namespace Garaget.Tests
{
    public class VehicleTests
    {
        [Fact] // kolla så det kastar på invalid regnummer
        public void Constructor_InvalidRegNumber_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Car("123ABC", "Röd", 4, "Volvo", "Bensin", CarType.Kombi));
        }


        [Fact]
        public void Constructor_LowercaseWithSpaces_NormalizesRegNumber()
        {
            // Arrange + Act: konstruktorn är det vi testar, så att skapa bilen är själva "görandet"
            var car = new Car("  abc123  ", "Röd", 4, "Volvo", "Bensin", CarType.Kombi);

            // Assert: förväntat värde först, faktiskt värde sist
            Assert.Equal("ABC123", car.RegNumber);
        }


        // kollar så att Stor/liten bokstav fungerar på färg
        [Fact]
        public void Matches_ColorDifferentCase_ReturnsTrue()
        {
            var car = new Car("abc123", "Röd", 4, "Volvo", "Bensin", CarType.Kombi);
            var result = car.Matches("röd");

            Assert.True(result);
        }


        // Här tittar vi så att siffran läses in som en sträng och sedan fungerar mot volymen på motorcykel. 
        [Fact]
        public void Matches_MotorcycleCylinderVolume_ReturnsTrue()
        {
            var motorcycle = new Motorcycle("abc123", "Vit", 2, "Husaberg", 500, MotorcycleType.Enduro);
            var result = motorcycle.Matches("500");

            Assert.True(result);
        }


        [Theory] // betyder att det ska köras flera gånger och dessa rader under
        [InlineData("12,5")]
        [InlineData("12.5")]
        public void Matches_BoatLengthWithCommaOrDot_ReturnsTrue(string searchTerm)
        {
            var boat = new Boat("abc123", "Vit", 0, "Flipper", 12.5);
            var result = boat.Matches(searchTerm);

            Assert.True(result);
        }


    }
}
