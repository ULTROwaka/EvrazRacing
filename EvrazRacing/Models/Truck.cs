using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvrazRacing.Models
{
    class Truck : Car
    {
        public float Weight;

        public Truck(string name, float speed, uint breakChance, uint repairTime, float weight) : base(name, speed, breakChance, repairTime)
        {
            if(weight < 0)
            {
                throw new ArgumentException("Argument is invalid, weight cant be negative", "weight");
            }
            Weight = weight;
        }

        public override string StartMessage() => $"Truck {Name}, speed: {Speed}, wheel breaking chance: {BreakChance}" +
                                                 $"with weight {Weight} units on start";   
    }
}
