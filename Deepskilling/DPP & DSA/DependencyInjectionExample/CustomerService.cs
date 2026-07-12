using System;

namespace DependencyInjectionExample
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        // Constructor Injection - Repository is injected from outside
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            Console.WriteLine("CustomerService initialized with CustomerRepository");
        }

        public Customer? GetCustomerById(int id)
        {
            Console.WriteLine($"Retrieving customer with ID: {id}");
            Customer? customer = _customerRepository.FindCustomerById(id);
            
            if (customer != null)
            {
                Console.WriteLine($"Customer retrieved: {customer.GetCustomerDetails()}");
            }
            else
            {
                Console.WriteLine($"Customer with ID {id} not found");
            }
            
            return customer;
        }

        public Customer? GetCustomerByEmail(string email)
        {
            Console.WriteLine($"Retrieving customer with Email: {email}");
            Customer? customer = _customerRepository.FindCustomerByEmail(email);
            
            if (customer != null)
            {
                Console.WriteLine($"Customer retrieved: {customer.GetCustomerDetails()}");
            }
            else
            {
                Console.WriteLine($"Customer with email {email} not found");
            }
            
            return customer;
        }

        public void RegisterCustomer(string name, string email, string phone)
        {
            int newId = GenerateNewId();
            Customer newCustomer = new Customer(newId, name, email, phone);
            
            Console.WriteLine($"Registering new customer: {name}");
            _customerRepository.AddCustomer(newCustomer);
            Console.WriteLine($"Customer registered successfully with ID: {newId}");
        }

        public void UpdateCustomer(int id, string name, string email, string phone)
        {
            Customer? existingCustomer = _customerRepository.FindCustomerById(id);
            
            if (existingCustomer != null)
            {
                existingCustomer.Name = name;
                existingCustomer.Email = email;
                existingCustomer.Phone = phone;
                
                Console.WriteLine($"Updating customer ID: {id}");
                _customerRepository.UpdateCustomer(existingCustomer);
                Console.WriteLine($"Customer updated successfully");
            }
            else
            {
                Console.WriteLine($"Customer with ID {id} not found for update");
            }
        }

        public void RemoveCustomer(int id)
        {
            Console.WriteLine($"Removing customer with ID: {id}");
            _customerRepository.DeleteCustomer(id);
            Console.WriteLine($"Customer removed successfully");
        }

        public void DisplayAllCustomers()
        {
            Console.WriteLine("\n=== All Customers ===");
            Customer?[] customers = _customerRepository.GetAllCustomers();
            
            foreach (Customer? customer in customers)
            {
                if (customer != null)
                {
                    Console.WriteLine(customer.GetCustomerDetails());
                }
            }
            Console.WriteLine("===================\n");
        }

        private int GenerateNewId()
        {
            Customer?[] allCustomers = _customerRepository.GetAllCustomers();
            int maxId = 0;
            
            foreach (Customer? customer in allCustomers)
            {
                if (customer != null && customer.Id > maxId)
                {
                    maxId = customer.Id;
                }
            }
            
            return maxId + 1;
        }
    }
}