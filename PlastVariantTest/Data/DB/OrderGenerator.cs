namespace PlastVariantTest.Data.DB;

static class OrderGenerator
{
    public static IEnumerable<Order> GetOrders(IEnumerable<Product> products, IEnumerable<Company> companies)
    {
        var _companies = companies.ToArray();
        var _products = products.ToArray();

        Random rnd = new Random();

        int number = 100;
        DateOnly dateStart = new DateOnly(DateTime.Today.Year - 1, DateTime.Today.Month, 1);
        DateOnly dateEnd = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        DateOnly date = dateStart;
        while (date < dateEnd)
        {
            for (int i = 1; i < rnd.Next(5); i++)
            {
                Company company = _companies[rnd.Next(_companies.Length)];
                Product product = _products[rnd.Next(_products.Length)];
                var order = new Order()
                {
                    Number = number++,
                    Company = company,
                    Product = product,
                    Amount = rnd.Next(500, 15000),
                    Price = product.Price * ((decimal)1 + (decimal)rnd.Next(20, 50) / (decimal)100),
                    ShippingDate = date
                };
                company.Orders.Add(order);
                yield return order;
            }

            date = date.AddDays(1);
        }

    }
}
