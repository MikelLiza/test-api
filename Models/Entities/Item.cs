namespace APITest.Models.Entities;

using System.ComponentModel.DataAnnotations;

public class Item
{
    [Required] public Guid Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public bool For_Sale { get; set; }
}