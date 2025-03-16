
using PicAFlick.Data;
using PicAFlick.Domain.Entities;

namespace PicAFlick.UI
{
    public class Program
    { 
        private static MovieContext _context = new MovieContext();
        public static void Main(string[] args)
        { 
            _context.Database.EnsureCreated();
            AddMovie();
            GetMovies("Title added:");

            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddMovie()
        {
            var movie = new Movie { Title = "The Godfather" };
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        private static void GetMovies(string text)
        {
            var movies = _context.Movies.ToList();

            foreach (var movie in movies)
            { 
                Console.WriteLine(movie.Title);
            }
        }
    }
}
