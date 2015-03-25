using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieRental.Models;

namespace MovieRental.Areas.Admin.Models
{
    public class BulkLoadOutput
    {
        public BulkLoadOutput(List<string> unresolvedTitles, List<Movie> resolvedMovie)
        {
            UnresolvedTitles = unresolvedTitles;
            ResolvedMovie = resolvedMovie;
        }
        public List<string> UnresolvedTitles { get; set; }
        public List<Movie> ResolvedMovie { get; set; }
    }
}