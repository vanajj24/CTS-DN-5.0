using System;
using System.Linq;

namespace DependencyInjectionExample
{
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        private Customer?[] _customers;
        private int _customerCount;

        public CustomerRepositoryImpl()
        {
            _customers = new Customer?[100];
            _customerCount = 0;

            InitializeSampleData();
        }

        public Customer? FindCustomerById(int id)
        {
            for (int i = 0; i < _customerCount; i++)
            {
                if (_customers[i] is Customer c && c.Id == id)
                {
                    Console.WriteLine($"Customer found with ID: {id}");
                    return c;
                }
            }

            Console.WriteLine($"Customer with ID: {id} not found");
            return null;
        }

        public Customer? FindCustomerByEmail(string email)
        {
            for (int i = 0; i < _customerCount; i++)
            {
                if (_customers[i] is Customer c && c.Email == email)
                {
                    Console.WriteLine($"Customer found with Email: {email}");
                    return c;
                }
            }

            Console.WriteLine($"Customer with Email: {email} not found");
            return null;
        }

        public void AddCustomer(Customer customer)
        {
            if (_customerCount < _customers.Length)
            {
                _customers[_customerCount] = customer;
                _customerCount++;
                Console.WriteLine($"Customer added successfully: {customer.Name}");
            }
            else
            {
                Console.WriteLine("Cannot add customer: Database full");
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            for (int i = 0; i < _customerCount; i++)
            {
                if (_customers[i] is Customer c && c.Id == customer.Id)
                {
                    _customers[i] = customer;
                    Console.WriteLine($"Customer updated successfully: {customer.Name}");
                    return;
                }
            }

            Console.WriteLine($"Customer with ID: {customer.Id} not found for update");
        }

        public void DeleteCustomer(int id)
        {
            for (int i = 0; i < _customerCount; i++)
            {
                if (_customers[i] is Customer c && c.Id == id)
                {
                    _customers[i] = null;
                    _customerCount--;
                    Console.WriteLine($"Customer deleted successfully with ID: {id}");
                    return;
                }
            }

            Console.WriteLine($"Customer with ID: {id} not found for deletion");
        }

        public Customer?[] GetAllCustomers()
        {
            Customer?[] result = new Customer?[_customerCount];
            for (int i = 0; i < _customerCount; i++)
            {
                result[i] = _customers[i];
            }
            return result;
        }

        private void InitializeSampleData()
        {
            _customers[0] = new Customer(1, "John Doe", "john@example.com", "123-456-7890");
            _customers[1] = new Customer(2, "Jane Smith", "jane@example.com", "234-567-8901");
            _customers[2] = new Customer(3, "Bob Wilson", "bob@example.com", "345-678-9012");
            _customerCount = 3;

            Console.WriteLine("Sample customer data initialized");
        }
    }

    public class MockCustomerRepository : ICustomerRepository
    {
        private Customer?[] _mockCustomers;

        public MockCustomerRepository()
        {
            _mockCustomers = new Customer?[5];
            _mockCustomers[0] = new Customer(101, "Mock Customer 1", "mock1@test.com", "000-000-0001");
            _mockCustomers[1] = new Customer(102, "Mock Customer 2", "mock2@test.com", "000-000-0002");
            _mockCustomers[2] = new Customer(103, "Mock Customer 3", "mock3@test.com", "000-000-0003");
        }

        public Customer? FindCustomerById(int id)
        {
            for (int i = 0; i < _mockCustomers.Length; i++)
            {
                if (_mockCustomers[i] is Customer c && c.Id == id)
                {
                    return c;
                }
            }
            return null;
        }

        public Customer? FindCustomerByEmail(string email)
        {
            for (int i = 0; i < _mockCustomers.Length; i++)
            {
                if (_mockCustomers[i] is Customer c && c.Email == email)
                {
                    return c;
                }
            }
            return null;
        }

        public void AddCustomer(Customer customer)
        {
            Console.WriteLine($"[MOCK] Customer added: {customer.Name}");
        }

        public void UpdateCustomer(Customer customer)
        {
            Console.WriteLine($"[MOCK] Customer updated: {customer.Name}");
        }

        public void DeleteCustomer(int id)
        {
            Console.WriteLine($"[MOCK] Customer deleted with ID: {id}");
        }

        public Customer?[] GetAllCustomers()
        {
            return _mockCustomers;
        }
    }
}