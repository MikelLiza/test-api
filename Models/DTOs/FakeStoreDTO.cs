namespace APITest.Models.DTOs;

using System.Text.Json.Serialization;

public class FakeStoreDTO
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("category")] public string Category { get; set; }
    [JsonPropertyName("image")] public string ImageUrl { get; set; }
    [JsonPropertyName("rating")] public FakeStoreRating Rating { get; set; }
}

public class FakeStoreRating
{
    [JsonPropertyName("rate")] public decimal Rate { get; set; }
    [JsonPropertyName("count")] public int Count { get; set; }
}