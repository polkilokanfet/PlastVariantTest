namespace PlastVariantTest.Data.DB;

public class DataBase
{
    IEnumerable<Company> _companies;
    IEnumerable<Product> _products;
    IEnumerable<Order> _orders;
    IEnumerable<PlanItem> _planItems;

    public DataBase()
    {
        _products = new List<Product>()
        {
            new Product() {Name = "Марка 0", Price = 80},
            new Product() {Name = "Марка 1", Price = 80},
            new Product() {Name = "Марка 2", Price = 90},
            new Product() {Name = "Марка 3", Price = 100},
            new Product() {Name = "Марка 4", Price = 110},
            new Product() {Name = "Марка 5", Price = 120},
            new Product() {Name = "Марка 6", Price = 130},
            new Product() {Name = "Марка 7", Price = 140},
            new Product() {Name = "Марка 8", Price = 150},
            new Product() {Name = "Марка 9", Price = 160},
        };
        _companies = new CompanyGenerator().GenerateCompanies(20);
        _orders = OrderGenerator.GetOrders(_products, _companies).ToList();
        _planItems = PlanGenerator.GeneratePlan(_orders).ToList();
    }

    public IEnumerable<Product> Products => _products;
    public IEnumerable<Company> Companies => _companies;
    public IEnumerable<Order> Orders(DateOnly start, DateOnly end) => _orders.Where(x => x.ShippingDate >= start && x.ShippingDate <= end);
    public IEnumerable<PlanItem> PlanItems => _planItems;

}