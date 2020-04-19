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

            var query = from car in cars
                        group car by car.Manufacturer.ToUpper() into manufacturer
                        orderby manufacturer.Key
                        select manufacturer;

            var query2 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                             .OrderBy(g => g.Key);
                         
                

            foreach (var group in query2)
            {
                Console.WriteLine(group.Key);
                foreach (var car in group.OrderByDescending(x=>x.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
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
