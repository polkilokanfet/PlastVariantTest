namespace PlastVariantTest.Data;

public class Order
{
    public int Number { get; set; }
    public Product Product { get; set; }
    public double Amount { get; set; }
    public decimal Price { get; set; }
    public DateOnly ShippingDate { get; set; }

    public Company Company { get; set; }
}
