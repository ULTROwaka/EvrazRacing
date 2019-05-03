using System;

namespace EvrazRacing.Models
{
    class Automobile : Car
    {
        public uint PassangersCount;
        public Automobile(string name, float speed, uint breakChance, uint repairTime, uint passangersCount) : base(name, speed, breakChance, repairTime)
        {
            PassangersCount = passangersCount;
        }

        public override string StartMessage() => $"Automobile {Name}, speed: {Speed}, wheel breaking chance: {BreakChance}" +
                                                 $"with {PassangersCount} passangers on start";
    }
}
