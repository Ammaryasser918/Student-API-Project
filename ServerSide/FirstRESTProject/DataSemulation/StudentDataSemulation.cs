using FirstRESTProject.Model;

namespace FirstRESTProject.DataSemulation
{
    public class StudentDataSemulation
    {

        public static readonly List<Student> StudentsList = new List<Student>
        {
            new Student {ID = 1, Name = "Ammar Yasser", Age = 20, Grade = 88},
            new Student {ID = 2, Name = "Kareem Yasser", Age = 18, Grade = 77 },
            new Student {ID = 3, Name = "Rehab Reda", Age = 20, Grade = 66 },
            new Student {ID = 4, Name = "Reham Yasser", Age = 44, Grade = 44}
        };

    }
}
