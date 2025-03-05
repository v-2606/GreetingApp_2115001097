using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IGreetingRL
    {
        void SaveGreeting(GreetingEntity greeting);
    }
}