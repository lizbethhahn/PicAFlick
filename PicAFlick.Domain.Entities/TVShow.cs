
namespace PicAFlick.Domain.Entities
{
    public class TVShow
    {
        public int Id { get; set; }
        public string? Title { get; set; }   
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public int Season { get; set; }
        public int Episode { get; set; }
        public bool IsWatched { get; set; } = false;
    }
}
