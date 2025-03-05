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
    public class GreetingRL: IGreetingRL
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

    }
}