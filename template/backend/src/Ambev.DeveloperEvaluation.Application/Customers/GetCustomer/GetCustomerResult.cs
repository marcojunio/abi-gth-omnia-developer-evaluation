namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}