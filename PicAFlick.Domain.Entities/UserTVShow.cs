namespace PicAFlick.Domain.Entities
{
    public class UserTVShow
    {
        public int Id { get; set; }
        public string? Title { get; set; }   
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? OriginalLanguage { get; set; }
        public int Season { get; set; }
        public int Episode { get; set; }
        public float? UserRating { get; set; }
        public bool IsWatched { get; set; } = false;
        public DateTime? DateWatched { get; set; }
    }
}