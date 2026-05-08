using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using StudentApiClient;
using StudentAPIClient;
using System.Diagnostics;


namespace StudentApiClient
{
    class Programm
    {
        static readonly HttpClient httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7175/api/Students/");
            await GetAllStudents();

            await GetPassedStudents();

            await GetAverageGrade();

            await GetStudentByID(1);

            await AddNewStudent(new Student { Name = "Ahmed", Age = 43, Grade = 55, ID = 7 });

            await GetAllStudents();

            await DeleteStudent(1);

            await GetAllStudents();

            await UpdateStudent(2, new Student { Name = "Omar", Age = 8, Grade = 75 });

        }

        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("\nFetching all students...\n");
                var response = await httpClient.GetAsync("All");
                if (response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                    if (students != null && students.Count > 0)
                    {
                        foreach (var std in students)
                        {
                            Console.WriteLine($"ID: {std.ID}, Name: {std.Name}, Age: {std.Age}, Grage: {std.Grade}\n");
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No Students Found.");
                }

                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }

        }


        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("Fetching Passed Students..\n");
                var response = await httpClient.GetAsync("Passed");
                if (response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                    if (students != null && students.Count > 0)
                    {
                        foreach (var std in students)
                        {
                            Console.WriteLine($"ID: {std.ID}, Name: {std.Name}, Age: {std.Age}, Grade: {std.Grade}");
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No Students Found");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error Occurred : {ex.Message}");
            }
        }

        static async Task GetAverageGrade()
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("Calculating Average Grade..\n");

                var response = await httpClient.GetAsync("AverageGrade");
                if (response.IsSuccessStatusCode)
                {
                    double? AvgGrade = await response.Content.ReadFromJsonAsync<double>();
                    if ((AvgGrade != null) || (AvgGrade != 0))
                    {
                        Console.WriteLine($"The Average is : {AvgGrade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No Sudents Found");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }


        }


        static async Task GetStudentByID(int StudentID)
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("Fetching Student Data..\n");
                var response = await httpClient.GetAsync($"StudentID/{StudentID}");
                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    if (student != null)
                    {
                        Console.WriteLine($"StudentID : {student.ID}, StudentName : {student.Name}, Age : {student.Age}, Grade : {student.Grade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request : Not Accepted ID {StudentID}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found : Student with ID {StudentID} Not Found");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }
        }

        static async Task AddNewStudent(Student student)
        {
            try
            {
                
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("Adding The Student..\n");
                var response = await httpClient.PostAsJsonAsync("NewStudent", student);
                if (response.IsSuccessStatusCode)
                {
                    var AddedStudent = await response.Content.ReadFromJsonAsync<Student>();
                    if (AddedStudent != null)
                    {
                        Console.WriteLine($"Added Student - ID: {AddedStudent.ID}, Name: {AddedStudent.Name}, Age: {AddedStudent.Age}, Grade: {AddedStudent.Grade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Invalid Student Data");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }
        }

        static async Task DeleteStudent(int StudentID)
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("Deleting The Student..\n");
                var response = await httpClient.DeleteAsync($"DeleteStudent/{StudentID}");
                if (response.IsSuccessStatusCode)
                {
                    var ans = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Success: {ans}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Bad Request: Invalid ID");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Studnet with ID {StudentID} Not Found");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }
        }

        static async Task UpdateStudent(int StudentID, Student updatedStudent)
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("Updating The Student..\n");
                var response = await httpClient.PutAsJsonAsync($"UpdateStudent/{StudentID}", updatedStudent);
                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    if (student != null)
                    {
                        Console.WriteLine($"Updated Student with ID {StudentID} - Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {StudentID} Not Found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Invalid Student Data");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }
        }

    }
}