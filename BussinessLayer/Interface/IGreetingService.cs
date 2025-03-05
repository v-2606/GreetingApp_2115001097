using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;


namespace BussinessLayer.Interface
{
    public interface IGreetingService
    {
        //string GetGreetingMessage();

        string GetGreetingMessage(string firstName = "", string lastName = "");
        void SaveGreeting(string message);
        GreetingEntity? GetGreetingById(int id);

       


    }


}
