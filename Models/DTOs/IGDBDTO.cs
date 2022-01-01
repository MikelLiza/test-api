namespace APITest.Models.DTOs;

using System.Text.Json.Serialization;

public class IGDBDTO
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("age_ratings")] public object?[] AgeRatingIds { get; set; }
    [JsonPropertyName("release_dates")] public object?[] ReleaseDateIds { get; set; }
    [JsonPropertyName("cover")] public object? CoverId { get; set; }
}

public enum AgeRatingCategory
{
    ESRB = 1,
    PEGI = 2,
    CERO = 3,
    USK = 4,
    GRAC = 5,
    CLASS_IND = 6,
    ACB = 7
}

public enum AgeRating
{
    Three = 1,
    Seven = 2,
    Twelve = 3,
    Sixteen = 4,
    Eighteen = 5,
    RP = 6,
    EC = 7,
    E = 8,
    E10 = 9,
    T = 10,
    M = 11
}