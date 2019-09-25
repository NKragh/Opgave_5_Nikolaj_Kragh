using System;

namespace Opgave_5_Nikolaj_Kragh
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Opgave 5 - TCP Server";
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            ServerWorker server = new ServerWorker();
            server.Start();

            Console.ReadLine();
        }
    }
}
