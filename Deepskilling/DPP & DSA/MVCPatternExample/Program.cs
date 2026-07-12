using System;

namespace MVCPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MVC Pattern Demo - Student Record Management ===\n");

            // Test 1: Create initial student
            Console.WriteLine("1. Creating Initial Student:");
            Student student = new Student(1, "John Doe", "A");
            StudentView view = new StudentView();
            StudentController controller = new StudentController(student, view);

            controller.DisplayStudent();

            // Test 2: Update student details
            Console.WriteLine("2. Updating Student Details:");
            controller.UpdateStudentDetails(1, "Jane Smith", "B");
            controller.DisplayStudent();

            // Test 3: Update only grade
            Console.WriteLine("3. Updating Only Grade:");
            controller.UpdateGrade("A");
            controller.DisplayStudent();

            // Test 4: Update only name
            Console.WriteLine("4. Updating Only Name:");
            controller.UpdateName("Robert Johnson");
            controller.DisplayStudent();

            // Test 5: Invalid grade
            Console.WriteLine("5. Testing Invalid Grade:");
            controller.UpdateGrade("X");

            // Test 6: Empty name
            Console.WriteLine("6. Testing Empty Name:");
            controller.UpdateName("");

            // Test 7: Create new student
            Console.WriteLine("7. Creating New Student:");
            Student newStudent = new Student(2, "Emily Davis", "C");
            controller.SetStudent(newStudent);
            controller.DisplayStudent();

            // Test 8: Multiple students
            Console.WriteLine("8. Managing Multiple Students:");
            Student[] students = new Student[3];
            students[0] = new Student(1, "Alice Brown", "A");
            students[1] = new Student(2, "Bob Wilson", "B");
            students[2] = new Student(3, "Carol White", "C");

            view.DisplayStudentList(students);

            // Test 9: Update grade for first student
            Console.WriteLine("9. Updating First Student's Grade:");
            StudentController controller1 = new StudentController(students[0], view);
            controller1.UpdateGrade("A");
            controller1.DisplayStudent();

            Console.WriteLine("\n=== MVC Pattern Demo Complete ===");
            Console.WriteLine("Key Benefits:");
            Console.WriteLine("  Separation of Concerns: Model, View, Controller are separate");
            Console.WriteLine("  Easy Maintenance: Changes to UI don't affect data logic");
            Console.WriteLine("  Reusability: View can be reused with different controllers");
            Console.WriteLine("  Parallel Development: Different teams can work on MVC components");
        }
    }
}