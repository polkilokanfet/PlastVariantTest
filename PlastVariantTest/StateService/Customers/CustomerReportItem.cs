using PlastVariantTest.Data;

namespace PlastVariantTest.StateService.Customers;

public class CustomerReportItem
{
    public Company Customer { get; }
    public decimal Sum { get; }
    public IEnumerable<Order> Orders { get; }

    public CustomerReportItem(IEnumerable<Order> orders1)
    {
        var orders = orders1.ToList();
        Orders = orders;
        Customer = orders.First().Company;
        Sum = orders.Sum(x => (decimal)x.Amount * x.Price);
    }
}
