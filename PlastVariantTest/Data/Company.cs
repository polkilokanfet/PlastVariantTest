namespace PlastVariantTest.Data;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    public List<Order> Orders { get; set; } = new();
}