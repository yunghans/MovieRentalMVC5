using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using MovieRental.Externals;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Areas.Admin.Models;

namespace MovieRental.Areas.Admin.Controllers
{
    public class BulkLoadController : Controller
    {
        // GET: Admin/BulkLoad
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string movieList,
            double defaultRentalRate, int defaultQty)
        {
            string[] titles = titles = Regex.Split(movieList, "\r\n|\r|\n");

            List<string> unresolvedTitles = new List<string>();
            List<Movie> resolvedMovie = new List<Movie>();

            foreach (string title in titles)
            {
                Movie m = OMDBService.GetMoviesByTitle(title);
                if (m != null)
                {
                    m.RentalRate = defaultRentalRate;
                    m.NoInStock = defaultQty;
                    resolvedMovie.Add(m);
                }
                else
                {
                    if (!string.IsNullOrEmpty(title))
                        unresolvedTitles.Add(title);
                }
            }

            BulkLoadOutput blo =
                new BulkLoadOutput(unresolvedTitles, resolvedMovie);

            TempData["blo"] = blo;
            return View("BulkOutput", blo);
        }

        [HttpPost]
        public ActionResult Save(string save)
        {
            List<string> messages = new List<string>();
            if (save == "Yes")
            {
                BulkLoadOutput blo = (BulkLoadOutput)TempData["blo"];
                List<Movie> movies = blo == null ?
                    new List<Movie>() : blo.ResolvedMovie;
                MovieRepository rep = new MovieRepository();

                foreach (Movie m in movies)
                {
                    try
                    {
                        rep.Add(m);
                        rep.Save();
                        messages.Add("Successfully add movie " + m.Title);
                    }
                    catch
                    {
                        messages.Add("Unable to add movie " + m.Title
                            + ". Perhaps it is already in the database.");
                    }
                }
            }
            else
            {
                messages.Add("Bulk Load operation is successfully cancelled.");
            }
            return View(messages);
        }

    }
}