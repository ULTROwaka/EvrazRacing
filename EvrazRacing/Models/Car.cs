using ReactiveUI;
using System;

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

        private int _onPitstopTime;
        public int OnPitstopTime
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

        public virtual event EventHandler OnBreaking;
        public abstract string StartMessage
        {
            get;
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

        public virtual void Update(float delta)
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
            if (breaking < BreakChance)
            {
                IsOnPitstop = true;
                OnBreaking(this, new EventArgs());
            }

            Passed += Speed * delta;
        }

        public virtual void SetOnStart()
        {
            Passed = 0;
            IsOnFinish = false;
        }
    }
}
