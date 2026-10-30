using Garaget.Validation;

namespace Garaget.Tests
{
    public class RegNumberValidationTests
    {
        [Theory] // betyder att det ska köras flera gånger och dessa rader under
        [InlineData("ABC123")]
        [InlineData("ABC12D")]
        [InlineData("abc123")]
        [InlineData("  abc123  ")]
        public void IsValid_ValidFormat_ReturnsTrue(string regNumber)
        {
            var result = RegNumberValidation.IsValid(regNumber);

            Assert.True(result);
        }



        [Theory]
        [InlineData("ABC1a3")]
        [InlineData("1BC12D")]
        [InlineData("")]
        [InlineData("ABD13232123")]
        [InlineData("123ASD")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData(".")]
        [InlineData("AB12")]
        [InlineData("ÅBC123")]
        public void IsValid_InvalidFormat_ReturnsFalse(string? regNumber)
        {
            var result = RegNumberValidation.IsValid(regNumber);

            Assert.False(result);
        }



    }
}