namespace PicAFlick.WebApi.Models
{
    public class MediaChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public int? TmdbId { get; set; }
        public string? MediaType { get; set; }
    }
}
