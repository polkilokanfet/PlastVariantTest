using PlastVariantTest.Data;

namespace PlastVariantTest.StateService.Customers;

public class ProductsReportItem
{
    public Product Product { get; }
    public decimal Sum { get; }
    public IEnumerable<Order> Orders { get; }

    public ProductsReportItem(IEnumerable<Order> orders1)
    {
        var orders = orders1.ToList();
        Orders = orders;
        Product = orders.First().Product;
        Sum = orders.Sum(x => (decimal)x.Amount * x.Price);
    }
}

