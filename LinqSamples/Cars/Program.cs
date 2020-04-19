using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var query = from car in cars
                        where car.Manufacturer == "BMW" && car.Year == 2016
                        orderby car.Combined descending, car.Name ascending
                        select new {
                            car.Combined,
                            car.Name,
                            car.Year,
                            car.Manufacturer
                        };


            //Using SelectMany -- Flattening Data - Find All persons from different businessUnit

            var query2 = cars.SelectMany(c => c.Name)
                             .OrderBy(c => c); 

            
                foreach (var @char in query2 )
                {
                    Console.WriteLine(@char);
                }


            //Return a Bool like All() , Contains()

            var result = cars.Any(c => c.Manufacturer == "Ford");

            Console.WriteLine(result);

            //Working with Last() -  First() - FirstOrDefault - LastOrDefault
            var top = cars.OrderByDescending(c => c.Combined)
                             .ThenBy(c => c.Name)
                             .Select(c=>c)
                             .First(c => c.Manufacturer == "BMW" && c.Year == 2016);

            Console.WriteLine(top.Name);
                /*cars.OrderByDescending(x => x.Combined)
                            .ThenBy(x => x.Name);*/    

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer} : {car.Name} : {car.Year} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            var query = File.ReadAllLines(path)
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .ToCar();
                
                /*from line in File.ReadAllLines(path).Skip(1)
                        where line.Length > 1
                        select Car.ParseFromCsv(line);*/

            return query.ToList();


            /*return File.ReadAllLines(path)
                       .Skip(1)
                       .Where(line => line.Length > 1)
                       .Select(Car.ParseFromCsv)
                       .ToList();*/
        }
    }

    public static class CarExtentions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var collumn = line.Split(';');
                yield return new Car()
                {
                    Year = int.Parse(collumn[0]),
                    Manufacturer = collumn[1],
                    Name = collumn[2],
                    Displacement = double.Parse(collumn[3]),
                    Cylinders = int.Parse(collumn[4]),
                    City = int.Parse(collumn[5]),
                    HighWay = int.Parse(collumn[6]),
                    Combined = int.Parse(collumn[7])
                };
            }
        }
    }
}
