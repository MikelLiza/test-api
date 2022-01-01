namespace APITest.Models.DTOs;

public class ItemDTO
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public bool For_Sale { get; set; }
}