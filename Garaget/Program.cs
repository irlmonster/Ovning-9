using Garaget.Handlers;
using Garaget.UI;


namespace Garaget
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int capacity = ConsoleInput.ReadInt("Hur många platser ska garaget ha? ", 1, 1000);
            IHandler handler = new GarageHandler(capacity);

            //Skapa ett menyobjekt och sen kör vi metoden Run 
            IMenuInterface menu = new MainMenu(handler);
            menu.Run();

            
        }
    }
}
