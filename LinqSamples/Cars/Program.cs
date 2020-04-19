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
            QueryXml();

            
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"http://pluralsignt.com/cars/2016";
            var es = (XNamespace)"http://pluralsignt.com/cars/2016/es";
            var document = XDocument.Load("fuel.xml");
            var query = from element in document.Element(ns + "Cars").Elements( es + "Car")
            where element.Attribute("Manufacturer").Value == "BMW"
            select element.Attribute("Name").Value;


            foreach (var name in query)
            {
                Console.WriteLine(name);
            }


        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var ns = (XNamespace)"http://pluralsignt.com/cars/2016";
            var es = (XNamespace)"http://pluralsignt.com/cars/2016/es";
            var document = new XDocument();
            var cars = new XElement( ns + "Cars",
                                from record in records
                                select new XElement( es+"Car",
                                    new XAttribute("Name", record.Name),
                                    new XAttribute("Combined", record.Combined),
                                    new XAttribute("Manufacturer", record.Manufacturer))

            );

            cars.Add(new XAttribute(XNamespace.Xmlns + "es", es));
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
