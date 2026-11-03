using Garaget.UI;
using System.Text.Json;

namespace Garaget.Storage
{
    public class JsonGarageStorage : IGarageStorage
    {
        private readonly string _filePath;
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        public JsonGarageStorage(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<GarageData> LoadAsync()
        {

            string json = await File.ReadAllTextAsync(_filePath);
            var data = JsonSerializer.Deserialize<GarageData>(json, _options);

            if (data is null)
            {
                throw new InvalidDataException("Kunde inte läsa in garage-data från filen.");
            }

            return data;
        }
    
        public async Task SaveAsync(GarageData garageData)
        {
            string json = JsonSerializer.Serialize(garageData, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
    