using System;
using EventSourcing.Hospital.Storage;

namespace EventSourcing.Hospital.App.Snapshots
{
    internal class Program
    {
        private static readonly Store Store;

        static Program()
        {
            Store = new Store();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("TOOO: Seed tons of events!");
        }
    }
}
