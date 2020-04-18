using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queries.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);

            foreach (var number in numbers)
            {
                Console.WriteLine(number);

            }
            
            
            var movies = new List<Movie>(){
                new Movie() { Title = "The Dark knight", Year = 2008 , Rating = 8.9F },
                new Movie() { Title = "The king's speech", Year = 2010, Rating = 8.0F },
                new Movie() { Title = "Casablanca", Year = 1942, Rating = 8.5F },
                new Movie() { Title = "StarWar V", Year = 1980, Rating = 8.7F }
            };

            var query = movies.Filter(x => x.Year > 2000)/*.ToList()
                               .Take(1)Where(m => m.Year > 2000)*/
                               .OrderByDescending(m=>m.Rating) ;

            //It's more efficient to Always filter before sorting

            //Where query Operator have differed excecution

            //ToList and Count query Operators forces immediate excecution

            var enumerator = query.GetEnumerator();

            /*Console.WriteLine(query.Count());*/
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

            /*foreach (var movie in query)
            {
                Console.WriteLine(movie.Title);
           }*/
        }
    }
}
 