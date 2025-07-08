namespace PicAFlick.Domain.Entities
{
    public class UserMovie
    {
        public int Id { get; set; }
        public string? BackdropPath { get; set; }
        public DateTime? DateWatched { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public bool IsWatched { get; set; }        
        public string? OriginalLanguage { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Title { get; set; }
        public int TmdbMovieId { get; set; } // id assigned by Tmdb API for a given Movie
        public decimal? UserRating { get; set; }            
    }
}