namespace PicAFlick.Domain.Entities
{
    public class UserTVShow
    {
        public int Id { get; set; }
        public string? BackdropPath { get; set; }
        public DateTime? DateWatched { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public bool IsWatched { get; set; } = false;
        public string? Name{ get; set; }   
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public int TmdbTvShowId { get; set; } // id assigned by Tmdb API for a given Tv Show
        public decimal? UserRating { get; set; }
    }
}