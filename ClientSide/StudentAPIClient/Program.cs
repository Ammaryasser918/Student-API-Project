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
            //await GetAllStudents();

            //await GetPassedStudents();

            //await GetAverageGrade();

            await GetStudentByID(1);

        }

        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n_________________________________");
                Console.WriteLine("\nFetching all students...\n");
                var students = await httpClient.GetFromJsonAsync<List<Student>>("All");

                if (students != null)
                {
                    foreach (var std in students)
                    {
                        Console.WriteLine($"ID: {std.ID}, Name: {std.Name}, Age: {std.Age}, Grage: {std.Grage}\n");
                    }
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
                var students = await httpClient.GetFromJsonAsync<List<Student>>("Passed");
                if (students != null)
                {
                    foreach (var std in students)
                    {
                        Console.WriteLine($"ID: {std.ID}, Name: {std.Name}, Age: {std.Age}, Grade: {std.Grage}");
                    }
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

                double? AvgGrade = await httpClient.GetFromJsonAsync<double>("AverageGrade");
                if ((AvgGrade != null) || (AvgGrade != 0))
                {
                    Console.WriteLine($"The Average is : {AvgGrade}");
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
                var student = await httpClient.GetFromJsonAsync<Student>($"StudentID/{StudentID}");
                if (student != null)
                {
                    Console.WriteLine($"StudentID : {student.ID}, StudentName : {student.Name}, Age : {student.Age}, Grade : {student.Grage}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occurred: {ex.Message}");
            }
        }



    }
}