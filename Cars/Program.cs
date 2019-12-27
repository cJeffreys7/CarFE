using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml("BMW");

            Console.Read();

            #region QueryPractice
            //foreach (var record in records)
            //{
            //    //var car = new XElement("Car");
            //    //var name = new XElement("Name", record.Name);
            //    //var combined = new XElement("Combined", record.Combined);

            //    //car.Add(name);
            //    //car.Add(combined);

            //    //Attribute oriented XML
            //    var name = new XAttribute("Name", record.Name);
            //    var combined = new XAttribute("Combined", record.Combined);
            //    //Pass attributes through with "Functional Construction"
            //    var car = new XElement("Car", name, combined);

            //    cars.Add(car);
            //}

            //XML format
            // <Cars>
            //      <Car>
            //          <Name>abc</Name>
            //          <Combined>21</Combined>
            //          ....
            //      </Car>
            //      ... more cars
            // </Cars>

            //document.Add(cars);
            //document.Save("fuel.xml");

            //Access file in bin/Debug/fuel.xml
            //Choose "Show All Files" near top of Solution Explorer

            //var query = cars.OrderByDescending(c => c.Combined)
            //                .ThenBy(c => c.Name); // Use thenby to create secondary sort in order

            //var query2 =
            //    from car in cars
            //    orderby car.Combined ascending, car.Name ascending
            //    select new //car; instead of using the car var, can create a simplified anonmymous var of car
            //    {
            //        car.Manufacturer,
            //        car.Name,
            //        car.Combined
            //    };

            //var query3 =
            //    from car in cars
            //    where car.Manufacturer == "BMW" && car.Year == 2016
            //    orderby car.Combined ascending, car.Name ascending
            //    select car;

            //var query4 =
            //    cars //.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            //    .OrderByDescending(c => c.Combined)
            //    .ThenBy(c => c.Name)
            //    .Select(c => c)
            //    .First(c => c.Manufacturer == "BMW" && c.Year == 2016); // Not Deferred, must immediately execute query
            //                                                            // .FirstOrDefault prevents a query exception, but returns a null object

            //var query5 = cars.Any(c => c.Manufacturer == "Ford");
            ////.Any, .All, .Contains ask if the dataset matches the predicate

            //// Anonymous variable, can create a test class
            //var anon = new
            //{
            //    Name = "Scott"
            //};

            //var query6 = cars.Select(c => c.Name);
            ////foreach (var name in query6)
            ////{
            ////    foreach (var character in name)
            ////    {
            ////        Console.WriteLine(character);
            ////    }
            ////}

            //// SelectMany combines collections together into one large collection (Lists, Arrays, etc.)
            //var query7 = cars.SelectMany(c => c.Name);
            ////foreach (var character in query7)
            ////{
            ////    Console.WriteLine(character);
            ////}


            //foreach (var car in query2.Take(10))
            //{
            //    Console.WriteLine($"{car.Name} : {car.Combined}");

            //}
            //Console.ReadKey();
            #endregion QueryPractice
        }

        private static void QueryXml(string manufacturer)
        {
            var ns = (XNamespace)"http://pluralsight.com/cars/2016"; //Cast to XNamespace
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = XDocument.Load("fuel.xml");

            var query =
                //?.Element does work but the where operator cannot run against a null, so coalescing ?? to default to empty XElement
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car") ?? Enumerable.Empty<XElement>()
                //?.Value returns null if the Attribute does not exist, preventing an exception
                where element.Attribute("Manufacturer")?.Value == manufacturer
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessFile("fuel.csv");

            var ns = (XNamespace)"http://pluralsight.com/cars/2016"; //Cast to XNamespace
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Cars",

                from record in records
                select new XElement(ex + "Car",
                                new XAttribute("Name", record.Name),
                                new XAttribute("Combined", record.Combined),
                                new XAttribute("Manufacturer", record.Manufacturer))
                );

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex)); //Nesting namespaces

            document.Add(cars);
            document.Save("fuel.xml");
        }

        private static List<Car> ProcessFile(string path)
        {
            var query =

                File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                .Select(l => Car.ParseFromCsv(l));
                //.ToCar();

            return query.ToList();

                //File.ReadAllLines(path)
                //    .Skip(1) // Skips the top line of the fuel.csv
                //    .Where(line => line.Length > 1)
                //    .Select(Car.ParseFromCsv)
                //    .ToList();
        }
    }

    //public static class CarExtensions
    //{
    //    public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
    //    {
    //        foreach (var line in source)
    //        {
    //            var columns = line.Split(',');

    //            yield return new Car
    //            {
    //                Year = int.Parse(columns[0]),
    //                Manufacturer = columns[1],
    //                Name = columns[2],
    //                Displacement = double.Parse(columns[3]),
    //                Cylinders = int.Parse(columns[4]),
    //                City = int.Parse(columns[5]),
    //                Highway = int.Parse(columns[6]),
    //                Combined = int.Parse(columns[7])
    //            };
    //        }
            
    //    }
    //}
}
