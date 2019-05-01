using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace EvrazRacing.Models
{
    internal abstract class Car : ReactiveObject
    {
        protected Random rand;
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = this.RaiseAndSetIfChanged(ref _name, value);
        }

        private float _speed;
        public float Speed
        {
            get => _speed;
            set => this.RaiseAndSetIfChanged(ref _speed, value);
        }

        private float _passed;
        public float Passed
        {
            get => _passed;
            set => this.RaiseAndSetIfChanged(ref _passed, value);
        }

        private uint _breakChance;
        public uint BreakChance
        {
            get => _breakChance;
            set => this.RaiseAndSetIfChanged(ref _breakChance, value);
        }

        private bool _isOnPitstop;
        public bool IsOnPitstop
        {
            get => _isOnPitstop;
            set => this.RaiseAndSetIfChanged(ref _isOnPitstop, value);
        }

        private uint _onPitstopTime;
        public uint OnPitstopTime
        {
            get => _onPitstopTime;
            set => this.RaiseAndSetIfChanged(ref _onPitstopTime, value);
        }

        private uint _repairTime;
        public uint RepairTime
        {
            get => _repairTime;
            set => this.RaiseAndSetIfChanged(ref _repairTime, value);
        }

        private bool _isOnFinish;
        public bool IsOnFinish
        {
            get => _isOnFinish;
            set => this.RaiseAndSetIfChanged(ref _isOnFinish, value);
        }

        protected Car(string name, float speed, uint breakChance, uint repairTime)
        {
            Name = name;
            Speed = speed;
            BreakChance = breakChance;
            RepairTime = repairTime;

            Passed = 0;
            IsOnPitstop = false;
            IsOnFinish = false;

            rand = new Random();
        }

        public abstract void Update(float delta);
        public abstract string StartMessage();
        public abstract event EventHandler OnBreaking;
    }
}
