namespace DependencyInjectionExample
{
    public class Customer
    {
        private int _id;
        private string _name;
        private string _email;
        private string _phone;

        public Customer(int id, string name, string email, string phone)
        {
            _id = id;
            _name = name;
            _email = email;
            _phone = phone;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string GetCustomerDetails()
        {
            return $"ID: {_id}, Name: {_name}, Email: {_email}, Phone: {_phone}";
        }
    }
}