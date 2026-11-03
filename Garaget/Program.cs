using Garaget.Handlers;
using Garaget.Storage;
using Garaget.UI;


namespace Garaget
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            int capacity;

            try
            {
                capacity = ConsoleInput.ReadInt("Hur många platser ska garaget ha? ", 1, 1000);
            }
            catch (UserCancelledException)
            {
                // Esc redan vid start: avsluta programmet
                ConsoleOutput.WriteSuccess("Hej då!");
                return;
            }

            IGarageStorage storage = new JsonGarageStorage(Path.Combine(AppContext.BaseDirectory, "garage.json"));
            IHandler handler = new GarageHandler(capacity, storage);

            //Skapa ett menyobjekt och sen kör vi metoden Run 
            IMenuInterface menu = new MainMenu(handler);
            await menu.RunAsync();


        }
    }
}
