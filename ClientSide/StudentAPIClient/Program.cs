using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using StudentApiClient;
using StudentAPIClient;


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
        

        


    }
}