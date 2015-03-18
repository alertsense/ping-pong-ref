using System;
using System.Threading;
using AlertSense.PingPong.Raspberry.IO;

namespace AlertSense.PingPong.Raspberry
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory.AddConnection<ITableConnection, TableConnection>();

            using (var game = GameFactory.CreateGame()) 
            {
                game.Start();
                Console.ReadLine();
            }
        }
    }
}
