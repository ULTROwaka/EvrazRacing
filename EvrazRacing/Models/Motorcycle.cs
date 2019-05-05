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

        public Motorcycle(string name, float speed, uint breakChance, uint repairTime, bool sidecar) : base(name, speed, breakChance, repairTime)
        {
            Sidecar = sidecar;
        }
        public override string StartMessage => $"Motorcycle {Name}, speed: {Speed}, wheel breaking chance: {BreakChance} " +
                                                 $"{(Sidecar?"with":"without" )} sidecar on start";
    }
}
