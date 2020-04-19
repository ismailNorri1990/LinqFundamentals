using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();

            //Working with ForEach


            /*foreach (var record in records)
            {
                var car = new XElement("Car",
                                new XAttribute("Name", record.Name),
                                new XAttribute("Combined", record.Combined),
                                new XAttribute("Manufacturer",record.Manufacturer));
                                

                *//*car.Add(name);
                 * 
                car.Add(combined);*//*
                cars.Add(car);
            }*/



            /*
             * **********************
             * Agregate Linq  implementation method 
             * ************************
             * 
             * var manufacturers = ProcessManufacturers("manufacturers.csv");

            var query = from car in cars
                        group car by car.Manufacturer into carGroup
                        select new
                        {
                            Name = carGroup.Key,
                            Max = carGroup.Max(c => c.Combined),
                            Min = carGroup.Min(c => c.Combined),
                            Avg = carGroup.Average(c=>c.Combined)
                        }
                        into result
                        orderby result.Max descending
                        select result;

            var query2 = cars.GroupBy(c => c.Manufacturer)
                             .Select(g=>
                             {
                                 var results = g.Aggregate(new CarStatistics(),
                                                               (acc, c) => acc.Accumulate(c),
                                                               acc => acc.Compute());
                                 return new
                                 {
                                     Name = g.Key,
                                     Max = results.Max,
                                     Min = results.Min,
                                     Average = results.Average
                                 };
                             })
                             .OrderByDescending(r=>r.Max);


            foreach (var result in query2)
            {
                Console.WriteLine(result.Name);
                Console.WriteLine($"\t Max: {result.Max} \n\t Min: {result.Min} \n\t Avr:{result.Average :N1}");
            }
*/

            /*
             * *********************************************
             * Selecting top 3 best performance car by countries
             * *********************************************
             * 
             * var query = from manufacturer in manufacturers
                        join car in cars on manufacturer.Name equals car.Manufacturer
                        into carGroup
                        select new
                        {
                            Manufacturer = manufacturer,
                            Car = carGroup
                        } 
                        into result
                        group result by result.Manufacturer.HeadQuarters;


            var query2 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) => new { Manufacturer = m, Car = g }).GroupBy(m => m.Manufacturer.HeadQuarters);

            foreach (var group in query2)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var car in group.SelectMany(g=>g.Car).OrderByDescending(c => c.Combined).Take(3))
                {
                    Console.WriteLine($"\t {car.Manufacturer} : {car.Name} : {car.Combined}");
                }
            }*/



            /*
             * *******************
             * Selecting Two  Best performance car by manufacturer
             * ********************
             * 
             * 
             * var query = from manufacturer in manufacturers
                        join car in cars on manufacturer.Name equals car.Manufacturer
                        into carGroup
                        orderby manufacturer.Name
                        select new
                        {
                            Manufacturer = manufacturer,
                            Car = carGroup
                        };

            var query2 = manufacturers.GroupJoin(cars,m=>m.Name,c=>c.Manufacturer, (m,g) => new {Manufacturer=m,Car=g}).OrderByDescending(m=>m.Manufacturer.Name);



            foreach (var group in query2)
            {
                Console.WriteLine($" {group.Manufacturer.Name} {group.Manufacturer.HeadQuarters} ");
                foreach (var car in group.Car.OrderByDescending(x=>x.Combined).Take(3))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }*/


        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var document = new XDocument();
            var cars = new XElement("Cars",
                                from record in records
                                select new XElement("Car",
                                    new XAttribute("Name", record.Name),
                                    new XAttribute("Combined", record.Combined),
                                    new XAttribute("Manufacturer", record.Manufacturer))

            );

            document.Add(cars);
            document.Save("fuel.xml");
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

    public class CarStatistics
    {

        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        public CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }

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
