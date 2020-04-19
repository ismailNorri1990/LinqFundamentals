using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {

            // Anonymous Methos -- Delegate -- Lambda Expression
            Func<int, int> square = x => x * x ;
            Console.WriteLine(square(3));

            Func<int, int, int> sum = (x,y) => x + y;

            Action<int> Write = x => Console.WriteLine(x);
            Write(square(sum(3,5)));
            
            //***


            IEnumerable<Employee> developers = new Employee[]
           {
             new Employee {Id = 1, Name = "Ismail"},
             new Employee {Id = 2, Name = "Badr"}
           };

            IEnumerable<Employee> sales = new List<Employee>()
            {
            new Employee {Id = 3, Name = "Issam" }
            };


            /*IEnumerator<Employee> enumerable = sales.GetEnumerator();

            Console.WriteLine(developers.Count());

            while (enumerable.MoveNext())
            {
                Console.WriteLine(enumerable.Current.Name);
            }*/

            var query = developers.Where(e => e.Name.Length == 4)
                                               .OrderBy(e => e.Name).Select(e=>e);

            var query2 = from developer in developers
                         where developer.Name.Length == 4
                         orderby developer.Name
                         select developer;

            foreach (var employee in query)
            {
                Console.WriteLine(employee.Name);
            }

        }
    }
}

