using System;

namespace PW1.Model
{
    public class Gnome : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }         
        public string Clan { get; set; }         
        public string Profession { get; set; }   
        public int Age { get; set; }             
        public int BeardLength { get; set; }     

        public override string ToString()
        {
            return $"[{Id}] {Name} из клана {Clan} | {Profession} | Возраст: {Age} лет | Борода: {BeardLength} см";
        }
    }
}