using System;
using ModelLayer.Model;
using RepositoryLayer.Context; 
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;
using StackExchange.Redis;
using System.Text.Json;


namespace RepositoryLayer
{
    public class GreetingRL : IGreetingRL
    {
        private readonly UserContext _context;
        private readonly ILogger<GreetingRL> _logger;
        private readonly RedisCacheService _redisCache;

        public GreetingRL(UserContext context, ILogger<GreetingRL> logger , RedisCacheService redisCache)
        {
            _context = context;
            _logger = logger;
           _redisCache= redisCache;
        }




        public void SaveGreeting(GreetingEntity greeting)
        {
            _logger.LogInformation($"Saving new greeting: {greeting.Message}");

            _context.Greetings.Add(greeting);
            _context.SaveChanges();

            _logger.LogInformation("Greeting saved successfully.");

            // Cache Invalidate
            _redisCache.RemoveData("greeting_list");
            _logger.LogInformation("Cache invalidated for greeting_list.");
        }





        public GreetingEntity? GetGreetingById(int id)
        {
            string cacheKey = $"greeting_{id}";

            _logger.LogInformation($"Fetching Greeting ID {id}");

            var cachedData = _redisCache.GetData(cacheKey);//Redis Data
            if (cachedData != null) 
            {
                _logger.LogInformation($" Greeting ID {id} found in Redis Cache.");
                return JsonSerializer.Deserialize<GreetingEntity>(cachedData);


            }
            var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id); //fetch from database
            if (greeting == null)
            {
                _logger.LogWarning($" Greeting ID {id} not found in Database.");
                return null; ;

            }
            _logger.LogInformation($" Greeting ID {id} fetched from Database.");
            _redisCache.SetData(cacheKey, JsonSerializer.Serialize(greeting), TimeSpan.FromMinutes(10)); // redis strore

            return greeting;


        }


        public List<GreetingEntity> GetAllGreeting()
            
        {

            string cacheKey = "greeting_list";

            var cachedData = _redisCache.GetData(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {

                _logger.LogInformation("Fetched greetings from Redis Cache.");
                return JsonSerializer.Deserialize<List<GreetingEntity>>(cachedData)!;
            }

            _logger.LogWarning(" Redis Cache miss! Fetching greetings from Database..."); 


            var greetings = _context.Greetings.ToList();
            if (greetings.Count == 0) return new List<GreetingEntity>();

            _redisCache.SetData(cacheKey, JsonSerializer.Serialize(greetings), TimeSpan.FromMinutes(10));

          
            _logger.LogInformation(" No greetings found in Database.");
            return greetings;


        }


        public bool EditGreeting(int id, string newMessage)
        {
            _logger.LogInformation($" Edit request received for Greeting ID: {id}");

            var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id);

            if (greeting == null)
            {
                _logger.LogWarning($" Greeting ID {id} not found in Database.");


                    return false;
            }
                greeting.Message = newMessage;
                _context.SaveChanges();
            _logger.LogInformation($" Greeting ID {id} updated successfully in Database.");


            // Cache Invalidate 
            _redisCache.RemoveData("greeting_list");
                _redisCache.RemoveData($"greeting_{id}");
            _logger.LogInformation($" Cache invalidated for Greeting ID {id} and greeting_list.");


            return true;
            
        }



        public bool deleteGreeting(int id)
        {

            _logger.LogInformation($" Delete request received for Greeting ID: {id}");
            var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id);

            if (greeting == null) 
            { 
                _logger.LogWarning($" Greeting ID {id} not found in Database. Delete failed.");

            return false;
        }
            _context.Greetings.Remove(greeting);
            _context.SaveChanges();
            _logger.LogInformation($" Greeting ID {id} deleted successfully from Database.");

            _redisCache.RemoveData("greeting_list");
            _redisCache.RemoveData($"greeting_{id}");
            _logger.LogInformation($" Cache invalidated for Greeting ID {id} and greeting_list.");

            return true;
        }
    }
}