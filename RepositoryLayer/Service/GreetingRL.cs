using System;
using ModelLayer.Model;
using RepositoryLayer.Context; 
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer
{
    public class GreetingRL : IGreetingRL
    {
        private readonly UserContext _context;
        private readonly ILogger<GreetingRL> _logger;

        public GreetingRL(UserContext context, ILogger<GreetingRL> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SaveGreeting(GreetingEntity greeting)
        {
            _context.Greetings.Add(greeting);
            _context.SaveChanges();
            _logger.LogInformation("Greeting saved successfully: {Message}", greeting.Message);
        }
        public GreetingEntity? GetGreetingById(int id)
        {
            return _context.Greetings.FirstOrDefault(g => g.Id == id);
        }
        public List<GreetingEntity> GetAllGreeting()
        {

            _logger.LogInformation("Retrieving all greetings from the database.");
            return _context.Greetings.ToList();

        }

        public bool EditGreeting(int id, string newMessage)
        {
            {

                var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id);
                if (greeting == null) return false;
                greeting.Message = newMessage;
                _context.SaveChanges();
                return true;
            }
        }

        public bool deleteGreeting(int id)
        {

            _logger.LogInformation($"DELETING request for greeting from the database of id:{id}");
            var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id);

            if (greeting == null) return false;

            _context.Greetings.Remove(greeting);
            _context.SaveChanges();
            return true;
        }
    }
}