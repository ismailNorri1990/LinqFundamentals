using System;
using System.Collections.Generic;
using System.Text;

namespace Features.Linq
{
    public static class myLinq
    {
        public static int Count<T>(this IEnumerable<T> sequence)
        {
            int count = 0;
            foreach (var item in sequence)
            {
                count++;
            }
            return count;
        }
       
    }
}
