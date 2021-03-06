﻿using System;
using System.Net.Http;

namespace Identity.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Call();
            Console.Read();
        }

        static async void Call()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:9964/api/values");
            request.Headers.Add("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiLkuIDkuKrkuLvpopgiLCJqdGkiOiJiYWM4NzBiZC1iOTkyLTRhZDItODlmZC01OWRhNGVmZjJlNTAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9nZW5kZXIiOiJtYWxlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoi5byg5LiJIiwibmJmIjoxNTQwMDI2MzQ4LCJleHAiOjE1NDA2MzExNDgsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6OTk2NC8iLCJhdWQiOiJjbGllbnRpZCJ9.cFCY2v3UM6JW1ssquw6-2VAXCjrJ_AiktFLXhMQbO28");
            var response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
                Console.WriteLine(response.StatusCode);
        }
    }
}
