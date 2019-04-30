using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvrazRacing.Models
{
    class Automobile : Car
    {
        uint PassangersCount;
        protected Automobile(string name, float speed, uint breakChance, uint repairTime, uint passangersCount) : base(name, speed, breakChance, repairTime)
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
                    OnPitstopTime = RepairTime;
                }
                else
                {
                    return;
                }
            }

            int breaking = rand.Next((int)BreakChance);
            if (breaking == BreakChance / 2)
            {
                IsOnPitstop = true;
                OnBreaking(this, new EventArgs());
            }

            Passed += Speed * delta;
        }
    }
}
