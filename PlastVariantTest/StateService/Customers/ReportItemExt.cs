using PlastVariantTest.Data;
using PlastVariantTest.StateService.Customers;

namespace PlastVariantTest.StateService.Customers;

public static class ReportItemExt
{
    public static IEnumerable<CustomerReportItem> ToCustomerReportItems(this IEnumerable<Order> orders)
    {
        return orders
            .GroupBy(x => x.Company)
            .Select(x => new CustomerReportItem(x))
            .OrderByDescending(x => x.Sum)
            .ToList();
    }

    public static IEnumerable<ProductsReportItem> ToProductsReportItems(this IEnumerable<Order> orders)
    {
        return orders
            .GroupBy(x => x.Product)
            .Select(x => new ProductsReportItem(x))
            .OrderByDescending(x => x.Sum)
            .ToList();
    }
}
