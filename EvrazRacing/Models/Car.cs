using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvrazRacing.Models
{
    internal abstract class Car
    {
        protected Random rand;
        private string _name;
        public string Name
        {
            get => _name;
            protected set => _name = value;
        }

        private float _speed;
        public float Speed
        {
            get => _speed;
            protected set => _speed = value;
        }

        private float _passed;
        public float Passed
        {
            get => _passed;
            protected set => _passed = value;
        }

        private uint _breakChance;
        public uint BreakChance
        {
            get => _breakChance;
            protected set => _breakChance = value;
        }

        private bool _isOnPitstop;
        public bool IsOnPitstop
        {
            get => _isOnPitstop;
            protected set => _isOnPitstop = value;
        }

        private uint _onPitstopTime;
        public uint OnPitstopTime
        {
            get => _onPitstopTime;
            protected set => _onPitstopTime = value;
        }

        private uint _repairTime;
        public uint RepairTime
        {
            get => _repairTime;
            protected set => _repairTime = value;
        }

        private bool _isOnFinish;
        public bool IsOnFinish
        {
            get => _isOnFinish;
            set => _isOnFinish = value;
        }

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
