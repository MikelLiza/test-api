namespace APITest.Models.DTOs;

using System.Text.Json.Serialization;

public class FirebaseUserDTO
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("password")] public string Password { get; set; }
}