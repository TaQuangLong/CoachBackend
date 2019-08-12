using Dgm.Core.SeedWork;
using System;

namespace EverCoach.Domain.AggregatesModel.CoachAggregate
{
    public class Coach
        : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int Age { get; private set; }
        public string PhoneNum { get; private set; }
        //public string Address { get; private set; }
        //public DateTime Dob { get; private set; }

        //private readonly List<Coach> _coaches;
        //public IReadOnlyCollection<Coach> Coaches => _coaches;

        public Coach(string name, string email, int age, string phoneNum)
        {
            Name = name;
            Email = email;
            Age = age;
            PhoneNum =phoneNum;
            //Dob = dob;
        }

       
        public void Update(string name, string email, int age, string phoneNum)
        {
            Name = name;
            Email = email;
            Age = age;
            PhoneNum = phoneNum;
            //Dob = dob;
        }
        public void Update(string name)
        {
            Name = name;
        }
     
        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }

    }
}
