

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BussinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Context;
using Microsoft.Extensions.Logging;
using RepositoryLayer;
using RepositoryLayer.Entity;



namespace BussinessLayer.Service
{
    public class GreetingService : IGreetingService
    {
        private readonly IGreetingRL _greetingRL;
        private readonly ILogger<GreetingService> _logger;

        public GreetingService(IGreetingRL greetingRL, ILogger<GreetingService> logger)
        {
            _greetingRL = greetingRL;

            _logger = logger;
        }


        public string GetGreetingMessage(string firstName = "", string lastName = "")
        {
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                return $"Hello {firstName} {lastName}!";
            }
            else if (!string.IsNullOrWhiteSpace(firstName))
            {
                return $"Hello {firstName}!";
            }
            else if (!string.IsNullOrWhiteSpace(lastName))
            {
                return $"Hello {lastName}!";
            }
            else
            {
                return "Hello World!";
            }
        }
        public void SaveGreeting(string message)
        {
            var greeting = new GreetingEntity { Message = message };
            _greetingRL.SaveGreeting(greeting);
            _logger.LogInformation("Greeting message processed: {Message}", message);
        }

        public GreetingEntity? GetGreetingById(int id)
        { 
            _logger.LogInformation($"Service: Searching for greeting with ID: {id}");
            return _greetingRL.GetGreetingById(id);
        }

      
    }
}

