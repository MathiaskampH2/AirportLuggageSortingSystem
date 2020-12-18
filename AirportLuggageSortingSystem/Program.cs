using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportLuggageSortingSystem
{
    class Program
    {
        // static ques for luggage creation and luggage sorting
        static Queue<Luggage> sortingLuggage = new Queue<Luggage>();
        static Queue<Luggage> luggageForGate01 = new Queue<Luggage>();
        static Queue<Luggage> luggageForGate02 = new Queue<Luggage>();
        static Queue<Luggage> luggageForGate03 = new Queue<Luggage>();
        // static lists of luggage that is sorted to gate 01 02 and 03
        static List<Luggage> loadinglistGate01 = new List<Luggage>();
        static List<Luggage> loadinglistGate02 = new List<Luggage>();
        static List<Luggage> loadinglistGate03 = new List<Luggage>();
        // Passenger count for each gate
        private static int passengerCount = 20;
        // gate01 gate 02 and gate 03 prefix for luggageNumber
        private static string gate01Id = "01";
        private static string gate02Id = "02";
        private static string gate03Id = "03";
        // counter for how many Luggage´s there has been made
        private static int counterGate01 = 0;
        private static int counterGate02 = 0;
        private static int counterGate03 = 0;
        // string builder for luggageNumber
        private static StringBuilder sb = new StringBuilder();
        // random number generator
        private static Random rand = new Random();

        // create LuggageNumber for gate01
        static string CreateLuggageIdForGate01(int counter)
        {
            sb = new StringBuilder();
            sb.Append(gate01Id);
            sb.Append("-");
            sb.Append(counter);

            return sb.ToString();
        }

        // create LuggageNumber for gate02
        static string CreateLuggageIdForGate02(int counter)
        {
            sb = new StringBuilder();
            sb.Append(gate02Id);
            sb.Append("-");
            sb.Append(counter);

            return sb.ToString();
        }

        // create LuggageNumber for gate03
        static string CreateLuggageIdForGate03(int counter)
        {
            sb = new StringBuilder();
            sb.Append(gate03Id);
            sb.Append("-");
            sb.Append(counter);

            return sb.ToString();
        }
        
        // Creates luggage for gate01
        static void CreateLuggageGate01()
        {
            while (true)
            {
                lock (sortingLuggage)
                {
                    if (counterGate01 != passengerCount)
                    {
                        counterGate01++;
                        Luggage tempLuggage = new Luggage();
                        tempLuggage = new Luggage()
                        {
                            LuggageNumber = CreateLuggageIdForGate01(counterGate01), DateIn = DateTime.Now,
                            DateOut = null
                        };
                        Console.WriteLine($"Luggage id: {tempLuggage.LuggageNumber} : check in : {tempLuggage.DateIn}");


                        sortingLuggage.Enqueue(tempLuggage);
                        Thread.Sleep(rand.Next(2000));
                        Monitor.PulseAll(sortingLuggage);
                    }
                }

                Thread.Sleep(3000);
            }
        }

        // Creates luggage for gate03
        static void CreateLuggageGate03()
        {
            while (true)
            {
                lock (sortingLuggage)
                {
                    if (counterGate03 != passengerCount)
                    {
                        counterGate03++;
                        Luggage tempLuggage = new Luggage();
                        tempLuggage = new Luggage()
                        {
                            LuggageNumber = CreateLuggageIdForGate03(counterGate03),
                            DateIn = DateTime.Now,
                            DateOut = null
                        };
                        Console.WriteLine($"Luggage id: {tempLuggage.LuggageNumber} : check in : {tempLuggage.DateIn}");


                        sortingLuggage.Enqueue(tempLuggage);
                        Thread.Sleep(rand.Next(2000));
                    }

                    Monitor.PulseAll(sortingLuggage);
                }

                Thread.Sleep(3000);
            }
        }

        // Creates luggage for gate02
        static void CreateLuggageGate02()
        {
            while (true)
            {
                lock (sortingLuggage)
                {
                    if (counterGate02 != passengerCount)
                    {
                        counterGate02++;
                        Luggage tempLuggage = new Luggage();
                        tempLuggage = new Luggage()
                        {
                            LuggageNumber = CreateLuggageIdForGate02(counterGate02),
                            DateIn = DateTime.Now,
                            DateOut = null
                        };
                        Console.WriteLine($"Luggage id: {tempLuggage.LuggageNumber} : check in : {tempLuggage.DateIn}");


                        sortingLuggage.Enqueue(tempLuggage);
                        Thread.Sleep(rand.Next(2000));
                    }

                    Monitor.PulseAll(sortingLuggage);
                }

                Thread.Sleep(3000);
            }
        }

        // Sorting the luggage into gate01 gate02 or gate03
        static void SortingLuggageForTerminals()
        {
            while (true)
            {
                lock (sortingLuggage)
                {
                    while (sortingLuggage.Count == 0)
                    {
                        Monitor.Wait(sortingLuggage);
                        Console.WriteLine("Waiting for luggage to arrive");
                    }

                    Luggage tempLuggage = sortingLuggage.Dequeue();

                    if (tempLuggage.LuggageNumber.Substring(0, 2) == "01")
                    {
                        lock (luggageForGate01)
                        {
                            tempLuggage.DateOut = DateTime.Now;
                            luggageForGate01.Enqueue(tempLuggage);
                            Console.WriteLine($"Luggage has been sorted  id: {tempLuggage.LuggageNumber} {tempLuggage.DateOut}");
                            Monitor.PulseAll(luggageForGate01);
                        }
                    }

                    else if (tempLuggage.LuggageNumber.Substring(0, 2) == "02")
                    {
                        lock (luggageForGate02)
                        {
                            tempLuggage.DateOut = DateTime.Now;
                            luggageForGate02.Enqueue(tempLuggage);
                            Console.WriteLine($"Luggage has been sorted  id: {tempLuggage.LuggageNumber} {tempLuggage.DateOut}");
                            Monitor.PulseAll(luggageForGate02);
                            
                        }
                    }

                    else if (tempLuggage.LuggageNumber.Substring(0, 2) == "03")
                    {
                        lock (luggageForGate03)
                        {
                            luggageForGate03.Enqueue(tempLuggage);
                            Monitor.PulseAll(luggageForGate03);
                        }
                    }
                }
                Thread.Sleep(rand.Next(1000));
            }
        }

        // Adding gate01 to loadingList
        static void AddGate01LuggageToLoadingList()
        {
            while (true)
            {
                lock (luggageForGate01)
                {
                    while (luggageForGate01.Count == 0)
                    {
                        Monitor.Wait(luggageForGate01);
                        Console.WriteLine("gate01 Waiting for luggage to arrive");
                    }

                    Luggage tempLuggage = luggageForGate01.Dequeue();

                    loadinglistGate01.Add(tempLuggage);


                }
                Thread.Sleep(1000);
            }
        }

        // Adding gate02 to loadingList
        static void AddGate02LuggageToLoadingList()
        {
            while (true)
            {
                lock (luggageForGate02)
                {
                    while (luggageForGate02.Count == 0)
                    {
                        Monitor.Wait(luggageForGate02);
                        Console.WriteLine("gate02 Waiting for luggage to arrive");
                    }

                    Luggage tempLuggage = luggageForGate02.Dequeue();

                    loadinglistGate02.Add(tempLuggage);


                }
                Thread.Sleep(1000);
            }
        }

        // Adding gate03 to loadingList
        static void AddGate03LuggageToLoadingList()
        {
            while (true)
            {
                lock (luggageForGate03)
                {
                    while (luggageForGate03.Count == 0)
                    {
                        Monitor.Wait(luggageForGate03);
                        Console.WriteLine("gate01 Waiting for luggage to arrive");
                    }

                    Luggage tempLuggage = luggageForGate03.Dequeue();

                    loadinglistGate03.Add(tempLuggage);


                }
                Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            // creating luggage for gate01, gate02 and gate03
            Thread gate01Luggage = new Thread(CreateLuggageGate01);
            Thread gate02Luggage = new Thread(CreateLuggageGate02);
            Thread gate03Luggage = new Thread(CreateLuggageGate03);
            // sorting the luggage
            Thread sortingLuggageThread = new Thread(SortingLuggageForTerminals);
            // adding gate01, gate02 and gate03 luggage to its loadingList
            Thread gate01AddLuggageToLoadingList = new Thread(AddGate01LuggageToLoadingList);
            Thread gate02AddLuggageToLoadingList = new Thread(AddGate02LuggageToLoadingList);
            Thread gate03AddLuggageToLoadingList = new Thread(AddGate03LuggageToLoadingList);
            // starting the threads
            gate01Luggage.Start();
            gate02Luggage.Start();
            gate03Luggage.Start();
            sortingLuggageThread.Start();
            gate01AddLuggageToLoadingList.Start();
            gate02AddLuggageToLoadingList.Start();
            gate03AddLuggageToLoadingList.Start();
        }
    }
}