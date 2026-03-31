using System.Text.Json.Serialization;

namespace PicTmdb.Models;

public class TmdbMovieCreditsResponseDto
{
    [JsonPropertyName("cast")]
    public List<TmdbCastMemberDto> Cast { get; set; } = [];
}

public class TmdbCastMemberDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("character")]
    public string Character { get; set; } = string.Empty;
}
