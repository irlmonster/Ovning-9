using System.Text.RegularExpressions;

namespace Garaget.Validation
{
    // Denna klass svarar bara på frågor och ska inte kunna instansieras
    public static partial class RegNumberValidation
    {
        [GeneratedRegex(@"^[A-Z]{3}[0-9]{2}[A-Z0-9]$")] // Mönster: tre bokstäver, två siffror, sista tecknet siffra eller bokstav
        private static partial Regex RegNumberRegex();
        
        public static string Normalize(string regNumber) // Tar bort mellanslag runt och gör versaler, så alla regnummer har samma format
        {
            return regNumber.Trim().ToUpperInvariant();
        }

        
        public static bool IsValid(string? regNumber) // Returnerar true om regnumret följer formatet, annars false
        {
            if (string.IsNullOrWhiteSpace(regNumber))
            {
                return false;
            }

            string normalized = Normalize(regNumber);
            return RegNumberRegex().IsMatch(normalized);       
        }

    }
}
