using System;

namespace MVCPatternExample
{
    public class StudentController
    {
        private Student _student;
        private StudentView _view;

        public StudentController(Student student, StudentView view)
        {
            _student = student;
            _view = view;
        }

        public void SetStudent(Student student)
        {
            _student = student;
        }

        public Student GetStudent()
        {
            return _student;
        }

        public void UpdateStudentDetails(int id, string name, string grade)
        {
            if (string.IsNullOrEmpty(name))
            {
                _view.DisplayErrorMessage("Student name cannot be empty");
                return;
            }

            if (grade != "A" && grade != "B" && grade != "C" && grade != "D" && grade != "F")
            {
                _view.DisplayErrorMessage("Grade must be A, B, C, D, or F");
                return;
            }

            _student.SetDetails(id, name, grade);
            _view.DisplaySuccessMessage("Student details updated successfully");
        }

        public void DisplayStudent()
        {
            _view.DisplayStudentDetails(_student);
        }

        public void UpdateGrade(string newGrade)
        {
            if (newGrade != "A" && newGrade != "B" && newGrade != "C" && newGrade != "D" && newGrade != "F")
            {
                _view.DisplayErrorMessage("Invalid grade. Must be A, B, C, D, or F");
                return;
            }

            _student.Grade = newGrade;
            _view.DisplaySuccessMessage($"Grade updated to {newGrade}");
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                _view.DisplayErrorMessage("Name cannot be empty");
                return;
            }

            _student.Name = newName;
            _view.DisplaySuccessMessage($"Name updated to {newName}");
        }
    }
}