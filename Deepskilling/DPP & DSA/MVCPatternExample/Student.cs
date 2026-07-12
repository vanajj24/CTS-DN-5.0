namespace MVCPatternExample
{
    public class Student
    {
        private int _id;
        private string _name;
        private string _grade;

        public Student(int id, string name, string grade)
        {
            _id = id;
            _name = name;
            _grade = grade;
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

        public string Grade
        {
            get { return _grade; }
            set { _grade = value; }
        }

        public void SetDetails(int id, string name, string grade)
        {
            _id = id;
            _name = name;
            _grade = grade;
        }

        public void GetDetails()
        {
            Console.WriteLine($"Student ID: {_id}");
            Console.WriteLine($"Student Name: {_name}");
            Console.WriteLine($"Student Grade: {_grade}");
        }
    }
}