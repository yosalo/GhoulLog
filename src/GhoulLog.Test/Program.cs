
namespace GhoulLog.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            Log.Info<Program>("Hello");
            Log.Debug("Program", "Debug");

            Log.Info<Program>("HHHH< ", fileName: "test");
            Log.Debug<Program>("DDe< ", fileName: "test");

            //Console.ReadKey();
        }
    }
}
