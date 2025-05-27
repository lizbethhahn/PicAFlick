using Domain.DTOs;
using DotNetEnv;
using PicAFlick.Data;
using PicAFlick.Domain.Entities;
using System.Data;
using System.Text.Json;

namespace PicAFlick.UI
{
    public class Program
    {
        private static UserContext _context = new UserContext();
 
        public static async Task Main(string[] args)
        {         
            Env.Load();
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("API Key not set.");
                return;
            }

            _context.Database.EnsureCreated();

            bool continueRunning = true;

            while (continueRunning)
            {
                // Display the menu options
                Console.WriteLine("=================================");
                Console.WriteLine("Welcome to Pic-A-Flick");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Search for a movie to add to your watch list.");
                Console.WriteLine("2. Search for a TV show to add to your watch list.");
                Console.WriteLine("3. Add a movie to the watch list.");
                Console.WriteLine("4. Remove a movie from the watchlist.");
                Console.WriteLine("5. View All movies.");
                Console.WriteLine("6. Exit");
                Console.WriteLine("=================================");
                Console.Write("Please select an option (1-6): ");

                // Get user input and handle it
                    string input = Console.ReadLine();

                // Process user input using a switch statement
                switch (input)
                {
                    case "1":
                        // Call method to search for a movie
                        Console.Clear();
                        await MovieTitleSearch();
                        break;
                    case "2":
                        // Call method to search for a TV show
                        await TvShowTitleSearch();
                        break;
                    case "3":
                        // Call method to add a movie 
                        AddMovie();
                        break;
                    case "4":
                        // Call method to delete a movie
                        RemoveMovieById();
                        break;
                    case "5":
                        // Call method to view all movies in the database
                        GetAllMovies();
                        break; ;
                    case "6":
                        continueRunning = false;
                        break;
                    default:
                        // Handle invalid input
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                        break;
                }
            }
            // Exit message
            Console.WriteLine("=================================");
            Console.WriteLine("Laters!");
            Console.WriteLine("=================================");
        }

        private static async Task MovieTitleSearch()
        {
            HttpClient httpClient = new HttpClient();
            var tmdbClient = new TmdbApiClient(httpClient);

            bool keepSearching = true;

            while (keepSearching)
            {
                Console.Write("Enter a movie title or type 'exit' to return to the main menu: ");
                string title = Console.ReadLine();

                if (title.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    break;
                }
                
                if (!string.IsNullOrWhiteSpace(title))
                {
                    string movieData = await tmdbClient.GetMovieByTitleAsync(title);
                    TmdbMovieSearchResponse searchResult = JsonSerializer.Deserialize<TmdbMovieSearchResponse>(movieData);

                    if (searchResult?.Results != null && searchResult.Results.Count > 0)
                    {
                        int lineCount = 0;
                        foreach (var movie in searchResult.Results)
                        {
                            Console.WriteLine($"\nTitle: {movie.Title}");
                            lineCount++;
                            Console.WriteLine($"Overview: {movie.Overview}");
                            lineCount++;
                            Console.WriteLine($"Release Date: {movie.ReleaseDate}");
                            lineCount++;
                            Console.WriteLine($"Rating: {movie.VoteAverage} ({movie.VoteCount} votes)");
                            lineCount++;
                            Console.WriteLine("-----------------------------\n");
                            lineCount++;

                            PauseIfNeeded(lineCount);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No results found.");
                        //break;
                    }
                }                
                else
                {
                    Console.WriteLine("\nNo title entered.");
                }
            }
            Console.WriteLine("\nPress any key to return to the main menu");
            Console.ReadKey();
        }
        private static async Task TvShowTitleSearch()
        {
            HttpClient httpClient = new HttpClient();
            var tmdbClient = new TmdbApiClient(httpClient);

            bool keepSearching = true;

            while (keepSearching)
            {
                Console.Write("Enter a tv title or type 'exit' to return to the main menu: ");
                string title = Console.ReadLine();

                if (title.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    break;
                }

                if (!string.IsNullOrWhiteSpace(title))
                {
                    string tvData = await tmdbClient.GetTvShowByTitleAsync(title);
                    TmdbMovieSearchResponse searchResult = JsonSerializer.Deserialize<TmdbMovieSearchResponse>(tvData);

                    if (searchResult?.Results != null && searchResult.Results.Count > 0)
                    {
                        int lineCount = 0;
                        foreach (var tvShow in searchResult.Results)
                        {
                            Console.WriteLine($"\nTitle: {tvShow.Title}");
                            lineCount++;
                            Console.WriteLine($"Overview: {tvShow.Overview}");
                            lineCount++;
                            Console.WriteLine($"Release Date: {tvShow.ReleaseDate}");
                            lineCount++;
                            Console.WriteLine($"Rating: {tvShow.VoteAverage} ({tvShow.VoteCount} votes)");
                            lineCount++;
                            Console.WriteLine("-----------------------------\n");
                            lineCount++;

                            PauseIfNeeded(lineCount);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No results found.");
                        //break;
                    }
                }
                else
                {
                    Console.WriteLine("\nNo title entered.");
                }
            }
            Console.WriteLine("\nPress any key to return to the main menu");
            Console.ReadKey();
        }
        private static void PauseIfNeeded(int lineCount, int maxLinesBeforePause = 20)
        {
            if (lineCount > 0 && lineCount % maxLinesBeforePause == 0)
            {
                Console.WriteLine("-- Press any key to continue --");
                Console.ReadKey(true); // 'true' means it won’t display the key pressed
            }
        }

        private static void AddMovie()
        {
            Console.WriteLine("Enter the title of the movie you want to add to the watch list:");
            string title = Console.ReadLine();
            var movie = new UserMovie { Title = title };
            _context.UserMovies.Add(movie);
            _context.SaveChanges();
        }

        private static void GetAllMovies()
        {
            var movies = _context.UserMovies.ToList();

            foreach (var movie in movies)
            {
                Console.WriteLine($"Id:{movie.Id}  Title: {movie.Title}");
            }
        }

        private static void RemoveMovieById()
        {
            Console.WriteLine("Enter the id of a movie to delete");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var idToDelete = _context.UserMovies.Where(m => m.Id == id);

                if (idToDelete != null)
                {
                    _context.UserMovies.RemoveRange(idToDelete);
                    _context.SaveChanges();
                    Console.WriteLine($"Movie with id {id} removed from the database");
                }
                else
                {
                    Console.WriteLine($"No movie found with ID {id}.");
                }
            }               
        }
    }
}