using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IGreetingRL
    {
        void SaveGreeting(GreetingEntity greeting);
        public GreetingEntity? GetGreetingById(int id);

        public List<GreetingEntity> GetAllGreeting();

       
        bool EditGreeting(int id, string newMessage);

        bool deleteGreeting(int id);
    }
}