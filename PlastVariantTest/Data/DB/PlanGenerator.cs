namespace PlastVariantTest.Data.DB;

static class PlanGenerator
{
    public static IEnumerable<PlanItem> GeneratePlan(IEnumerable<Order> orders)
    {
        Random rnd = new Random();

        var byYear = orders.GroupBy(x => x.ShippingDate.Year);
        foreach(var by in byYear)
        {
            var year = by.Key;
            var byMonth = by.GroupBy(x => x.ShippingDate.Month);
            foreach(var bm in byMonth)
            {
                var month = bm.Key;
                var byProduct = bm.GroupBy(x => x.Product);
                foreach(var bp  in byProduct)
                {
                    var product = bp.Key;
                    yield return new PlanItem
                    {
                        Year = year,
                        Month = month,
                        Product = product,
                        Amount = bm.Sum(x => x.Amount) * (1 + rnd.Next(-10, 10) / 100)
                    };
                }
            }
        }
    }
}
