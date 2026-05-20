using StudentAPIDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentAPIBusinessLayer
{
    public class clsStudentBusinessLayer
    {
        public int ID { get; set; }
        public string Name { get; set; } 
        public int Age { get; set; }
        public int Grade { get; set; }

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public StudentDTO SDTO
        {
            get
            {
                return new StudentDTO(this.ID, this.Name, this.Age, this.Grade);
            }
        }


        public clsStudentBusinessLayer(StudentDTO SDTO, enMode cMode = enMode.AddNew)
        {
            this.ID = SDTO.ID;
            this.Name = SDTO.Name;
            this.Age = SDTO.Age;
            this.Grade = SDTO.Grade;
            this.Mode = cMode;
        }

        private bool _AddNewStudent()
        {
            this.ID = clsStudentDataLayer.AddNewStudent(SDTO);
            return (this.ID != -1);
        }

        private bool _UpdateStudent()
        {
            return clsStudentDataLayer.UpdateStudent(SDTO);
        }

        static public List<StudentDTO> GetAllStduents()
        {
            return clsStudentDataLayer.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return clsStudentDataLayer.GetPassedStudents();
        }

        public static double GetAverageGrade()
        {
            return clsStudentDataLayer.GetAverageGrade();
        }

        public static clsStudentBusinessLayer GetStudentByID(int ID)
        {
            StudentDTO SDTO = clsStudentDataLayer.GetStudentByID(ID);
            if (SDTO == null)
            {
                return null;
            }
            return new clsStudentBusinessLayer(SDTO, enMode.Update);
        }

        public static bool DeleteStudent(int ID)
        {
            return clsStudentDataLayer.DeleteStudent(ID);
        }

        public bool Save()
        {
            if (this.Mode == enMode.AddNew)
            {
                if (_AddNewStudent())
                {
                    this.Mode = enMode.Update;
                    return true;
                }
                return false;
            }
            return _UpdateStudent();
        }

    }
}
