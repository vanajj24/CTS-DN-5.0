using System;

namespace MVCPatternExample
{
    public class StudentView
    {
        public void DisplayStudentDetails(Student student)
        {
            Console.WriteLine("\n=== Student Details ===");
            Console.WriteLine($"Student ID: {student.Id}");
            Console.WriteLine($"Student Name: {student.Name}");
            Console.WriteLine($"Student Grade: {student.Grade}");
            Console.WriteLine("======================\n");
        }

        public void DisplayStudentList(Student[] students)
        {
            Console.WriteLine("\n=== Student List ===");
            Console.WriteLine($"Total Students: {students.Length}");
            Console.WriteLine("--------------------");
            
            foreach (var student in students)
            {
                if (student != null)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Grade: {student.Grade}");
                }
            }
            Console.WriteLine("--------------------\n");
        }

        public void DisplayErrorMessage(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }

        public void DisplaySuccessMessage(string message)
        {
            Console.WriteLine($"SUCCESS: {message}");
        }
    }
}