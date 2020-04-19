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
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");
            
            // Join Method with Query Syntax

            var query = from car in cars
                        join manufacturer in manufacturers 
                        on new { car.Manufacturer, car.Year } 
                        equals 
                        new { Manufacturer = manufacturer.Name, manufacturer.Year }
                        orderby car.Combined descending, car.Name ascending
                        select new {
                            car.Combined,
                            car.Name,
                            car.Year,
                            manufacturer.HeadQuarters
                        };

            //Join Method With extension method syntax

            var query2 = cars.Join(manufacturers,
                              c => new { c.Manufacturer, c.Year },
                              m => new { Manufacturer = m.Name, m.Year }, (c, m) =>
                               new
                               {
                                   Car = c,
                                   Manufacturer = m

                        //Creating Personalized anonymous object
                                /* c.Combined,
                                   c.Name,
                                   c.Year,
                                   m.HeadQuarters*/
                               })
                             .OrderByDescending(c => c.Car.Combined)
                             .ThenBy(c => c.Car.Name);


            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer.HeadQuarters} : {car.Car.Name} : {car.Car.Year} : {car.Car.Combined}");
            }           
        }

        
        private static List<Car> ProcessCars(string path)
        {
            var query = File.ReadAllLines(path)
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .ToCar();

            return query.ToList();
        }


        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .Select(l => { 
                                    var collumns = l.Split(',');
                                    return new Manufacturer
                                     {
                                          Name = collumns[0],
                                          HeadQuarters = collumns[1],
                                          Year = int.Parse(collumns[2])
                                      };
                            });

            return query.ToList();
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
