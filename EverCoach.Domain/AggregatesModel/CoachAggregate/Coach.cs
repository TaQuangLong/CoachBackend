using EverCoach.Domain.AggregatesModel.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverCoach.Domain.AggregatesModel.CoachAggregate
{
    public class Coach
        :Entity,IAggregateRoot
    {
        //public string CoachId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int Age { get; private set; }
        public string PhoneNum { get; private set; }
        //public DateTime Dob { get; private set; }

        private readonly List<Coach> _coaches;
        public IReadOnlyCollection<Coach> Coaches => _coaches;

        public Coach(string name, string email, int age, string phoneNum)
        {
            Name = name;
            Email = email;
            Age = age;
            PhoneNum =phoneNum;
            //Dob = dob;
        }

       
        public void Update(string id, string name, string email, int age, string phoneNum, DateTime dob)
        {
            Name = name;
            Email = email;
            Age = age;
            PhoneNum = phoneNum;
            //Dob = dob;
        }
        public void Delete()
        {

        }
        public void GetCoach()
        {

        }
    }
}
