using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvrazRacing.Models
{
    class Automobile : Car
    {
        public uint PassangersCount;
        public Automobile(string name, float speed, uint breakChance, uint repairTime, uint passangersCount) : base(name, speed, breakChance, repairTime)
        {
            PassangersCount = passangersCount;
        }

        public override event EventHandler OnBreaking;

        public override string StartMessage() => $"Automobile {Name}, speed: {Speed}, wheel breaking chance: {BreakChance}" +
                                                 $"with {PassangersCount} passangers on start";

        public override void Update(float delta)
        {
            if (IsOnPitstop)
            {
                OnPitstopTime--;
                if (OnPitstopTime < 0)
                {
                    IsOnPitstop = false;
                    OnPitstopTime = (int)RepairTime;
                }
                else
                {
                    return;
                }
            }

            int breaking = rand.Next(100);
            if (breaking >=  BreakChance)
            {
                IsOnPitstop = true;
                OnBreaking(this, new EventArgs());
            }

            Passed += Speed * delta;
        }
    }
}
