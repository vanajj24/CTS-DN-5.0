using System;

namespace DependencyInjectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Dependency Injection Demo - Customer Management ===\n");

            // Test 1: Create CustomerService with CustomerRepositoryImpl (Real Repository)
            Console.WriteLine("1. Creating CustomerService with Real Repository:");
            ICustomerRepository realRepository = new CustomerRepositoryImpl();
            CustomerService customerService = new CustomerService(realRepository);

            // Test 2: Find customer by ID
            Console.WriteLine("\n2. Finding Customer by ID:");
            Customer? customer1 = customerService.GetCustomerById(1);
            if (customer1 != null)
            {
                Console.WriteLine($"Found: {customer1.GetCustomerDetails()}");
            }

            // Test 3: Find customer by email
            Console.WriteLine("\n3. Finding Customer by Email:");
            Customer? customer2 = customerService.GetCustomerByEmail("jane@example.com");
            if (customer2 != null)
            {
                Console.WriteLine($"Found: {customer2.GetCustomerDetails()}");
            }

            // Test 4: Register new customer
            Console.WriteLine("\n4. Registering New Customer:");
            customerService.RegisterCustomer("Alice Brown", "alice@example.com", "456-789-0123");

            // Test 5: Display all customers
            Console.WriteLine("\n5. Displaying All Customers:");
            customerService.DisplayAllCustomers();

            // Test 6: Update customer
            Console.WriteLine("\n6. Updating Customer:");
            customerService.UpdateCustomer(1, "John D. Doe", "john.doe@example.com", "123-456-7890");

            // Test 7: Find updated customer
            Console.WriteLine("\n7. Finding Updated Customer:");
            Customer? customer3 = customerService.GetCustomerById(1);
            if (customer3 != null)
            {
                Console.WriteLine($"Updated: {customer3.GetCustomerDetails()}");
            }

            // Test 8: Delete customer
            Console.WriteLine("\n8. Deleting Customer:");
            customerService.RemoveCustomer(3);

            // Test 9: Create CustomerService with MockRepository (for testing)
            Console.WriteLine("\n9. Creating CustomerService with Mock Repository (for Testing):");
            ICustomerRepository mockRepository = new MockCustomerRepository();
            CustomerService testService = new CustomerService(mockRepository);

            // Test 10: Use mock service
            Console.WriteLine("\n10. Using Mock Service:");
            Customer? mockCustomer = testService.GetCustomerById(101);
            if (mockCustomer != null)
            {
                Console.WriteLine($"Mock Customer: {mockCustomer.GetCustomerDetails()}");
            }

            // Test 11: Register with mock service
            Console.WriteLine("\n11. Registering with Mock Service:");
            testService.RegisterCustomer("Test Customer", "test@example.com", "111-111-1111");

            // Test 12: Demonstrate dependency switching
            Console.WriteLine("\n12. Demonstrating Dependency Switching:");
            Console.WriteLine("Creating new CustomerService with different repository:");
            ICustomerRepository anotherRepository = new CustomerRepositoryImpl();
            CustomerService anotherService = new CustomerService(anotherRepository);
            anotherService.RegisterCustomer("New Customer", "new@example.com", "999-999-9999");

            Console.WriteLine("\n=== Dependency Injection Demo Complete ===");
            Console.WriteLine("Key Benefits:");
            Console.WriteLine("  Loose Coupling: CustomerService doesn't create repository");
            Console.WriteLine("  Easy Testing: Mock repository can be injected for testing");
            Console.WriteLine("  Flexibility: Repository implementation can be changed easily");
            Console.WriteLine("  Reusability: Same service works with different repositories");
            Console.WriteLine("  Maintainability: Changes to repository don't affect service");
        }
    }
}