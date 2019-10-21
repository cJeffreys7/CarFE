using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var query = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name); // Use thenby to create secondary sort in order

            var query2 =
                from car in cars
                orderby car.Combined ascending, car.Name ascending
                select car;


            var query3 =
                from car in cars
                where car.Manufacturer == "BMW" && car.Year == 2016
                orderby car.Combined ascending, car.Name ascending
                select car;

            var query4 =
                cars //.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name)
                .Select(c => c)
                .First(c => c.Manufacturer == "BMW" && c.Year == 2016); // Not Deferred, must immediately execute query
                                                                        // .FirstOrDefault prevents a query exception, but returns a null object

            var query5 = cars.Any(c => c.Manufacturer == "Ford");
                         //.Any, .All, .Contains ask if the dataset matches the predicate



            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
                
            }
            Console.ReadKey();
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
