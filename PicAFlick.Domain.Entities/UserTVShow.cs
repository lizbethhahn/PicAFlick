namespace PicAFlick.Domain.Entities
{
    public class UserTVShow
    {              
        public string? BackdropPath { get; set; }
        public DateTime? DateWatched { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public List<int>? GenreIds { get; set; }
        public bool IsWatched { get; set; } = false;
        public string? Name{ get; set; }   
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public int TmdbTvShowId { get; set; }
        public int UserTvShowId { get; set; }
        public float? UserRating { get; set; }
    }
}