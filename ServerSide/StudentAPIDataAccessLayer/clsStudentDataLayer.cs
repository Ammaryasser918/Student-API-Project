using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


namespace StudentAPIDataAccessLayer
{
    public class StudentDTO
    {
        public StudentDTO(int id, string name, int age, int grade)
        {
            this.ID = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }

    public class clsStudentDataLayer
    {

        static string _ConnectionString = "Server=localhost;Database=StudentsDB;User ID = sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<StudentDTO> GetAllStudents()
        {
            var StudentList = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentList.Add(new StudentDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Age")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))
                                    ));
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        StudentList = new List<StudentDTO>();
                        // Do Something Here
                    }
                }
            }
            return StudentList;
        }
        

        public static List<StudentDTO> GetPassedStudents()
        {
            var StudentList = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentList.Add(new StudentDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetInt32(reader.GetOrdinal("Age")),
                                    reader.GetInt32(reader.GetOrdinal("Grade"))
                                    ));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StudentList = new List<StudentDTO>();
                        // Do Something Here
                    }
                }
            }
            return StudentList;
        }


        public static double GetAverageGrade()
        {
            double Average = 0;
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        object res = cmd.ExecuteScalar();
                        if (res != DBNull.Value && double.TryParse(res.ToString(), out double ans))
                        {
                            Average = Convert.ToDouble(ans);
                        }
                    }
                    catch(Exception ex)
                    {
                        // Do Something Here
                    }
                }
            }
            return Average;
        }


        public static StudentDTO GetStudentByID(int ID)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetStudentByID", connection))
                {
                    cmd.Parameters.AddWithValue("@StudentID", ID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new StudentDTO(
                                                         reader.GetInt32(reader.GetOrdinal("StudentID")),
                                                         reader.GetString(reader.GetOrdinal("Name")),
                                                         reader.GetInt32(reader.GetOrdinal("Age")),
                                                         reader.GetInt32(reader.GetOrdinal("Grade"))
                                                         );
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        // Do something here
                    }
                }
            }
            return null;
        }


        public static int AddNewStudent(StudentDTO student)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_AddStudent", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Grade", student.Grade);
                    SqlParameter outputParam = new SqlParameter("@NewStudentID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);
                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        // Do Something Here
                    }
                    return Convert.ToInt32(outputParam.Value);
                }
            }
        }

        public static bool UpdateStudent(StudentDTO updatedStudent)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_UpdateStudent", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", updatedStudent.Name);
                    cmd.Parameters.AddWithValue("@Age", updatedStudent.Age);
                    cmd.Parameters.AddWithValue("@Grade", updatedStudent.Grade);
                    cmd.Parameters.AddWithValue("@StudentID", updatedStudent.ID);
                    try
                    {
                        connection.Open();
                        AffectedRows = cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        // Do something here
                    }
                }
            }
            return (AffectedRows > 0);
        }

        public static bool DeleteStudent(int ID)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DeleteStudent", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", ID);
                    var outputParam = new SqlParameter(@"AffectedRows", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);
                    try
                    {
                        connection.Open();
                        AffectedRows = cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        // Do Something Here
                    }
                }
            }
            return (AffectedRows == 1);
        }



    }

}
