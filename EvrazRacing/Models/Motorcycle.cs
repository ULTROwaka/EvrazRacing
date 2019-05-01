using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvrazRacing.Models
{
    class Motorcycle : Car
    {
        public bool Sidecar;

        protected Motorcycle(string name, float speed, uint breakChance, uint repairTime, bool sidecar) : base(name, speed, breakChance, repairTime)
        {
            Sidecar = sidecar;
        }

        public override event EventHandler OnBreaking;

        public override string StartMessage() => $"Motorcycle {Name}, speed: {Speed}, wheel breaking chance: {BreakChance}" +
                                                 $"{(Sidecar?"with":"without" )} sidecar on start";

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
