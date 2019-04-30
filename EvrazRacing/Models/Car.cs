using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvrazRacing.Models
{
    internal abstract class Car
    {
        protected string Name;
        protected float Speed;
        protected float Passed;
        protected uint BreakChance;
        protected bool IsOnPitstop;
        protected uint OnPitstopTime;
        protected uint RepairTime;
        protected Random rand;

        protected Car(string name, float speed, uint breakChance, uint repairTime)
        {
            Name = name;
            Speed = speed;
            BreakChance = breakChance;
            RepairTime = repairTime;

            rand = new Random();
        }

        public abstract void Update(float delta);
        public abstract string StartMessage();
        public abstract event EventHandler OnBreaking;
    }
}
