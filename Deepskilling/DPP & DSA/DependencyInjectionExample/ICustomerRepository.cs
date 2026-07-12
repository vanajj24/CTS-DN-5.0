namespace DependencyInjectionExample
{
    public interface ICustomerRepository
    {
        Customer? FindCustomerById(int id);
        Customer? FindCustomerByEmail(string email);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        Customer?[] GetAllCustomers();
    }
}