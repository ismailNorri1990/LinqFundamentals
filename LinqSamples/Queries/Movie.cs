﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queries.Linq;

namespace Queries
{
    class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }
        int _year;
        public int Year
        {
            get {
                Console.WriteLine($"Returning {_year} for {Title}");
                return _year;
            }
            set
            {
                _year = value;
            }
        }
    }
}
