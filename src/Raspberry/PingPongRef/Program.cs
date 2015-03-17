using System;
using System.Threading;
using AlertSense.PingPong.Raspberry.IO;

namespace AlertSense.PingPong.Raspberry
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Resources.Banner);

            ConnectionFactory.AddConnection<ITableConnection, TableConnection>();

            using (var game = new GameController()) 
            {
                game.Start();
                Console.WriteLine(Resources.Instructions);
                Console.ReadLine();
            }
        }
    }
}
