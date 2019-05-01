using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EvrazRacing.Models
{
    class Track
    {

        public readonly List<Car> CarsOnTrack;
        public readonly List<Car> LeaderBoard;
        private Timer RaceTimer;

        private int _finishedCarsCount;
        private uint _interval;
        private float _distance;
        private bool _isStarted;
        private bool _isFinished;

        public float Delta
        {
            get => Interval / 1000;
        }
        public bool IsStarted
        {
            get => _isStarted;
            set => _isStarted = value;
        }
        public float Distance
        {
            get => _distance;
            set => _distance = value;
        }
        public uint Interval
        {
            get => _interval;
            set => _interval = value;
        }
        public int FinishedCarsCount
        {
            get => _finishedCarsCount;
        }
        public bool IsFinished
        {
            get => _isFinished;
        }

        public Track()
        {
            _interval = 1000;
            _finishedCarsCount = 0;
            _isStarted = false;
            _isFinished = false;
            CarsOnTrack = new List<Car>();
            LeaderBoard = new List<Car>();
        }

        private void RaceTimerTick(Object source, ElapsedEventArgs e)
        {        
            Update();
            if (FinishedCarsCount >= CarsOnTrack.Count)
            {
                RaceTimer.Enabled = false;
            }
        }

        private void Update()
        {
            foreach (var car in CarsOnTrack)
            {
                if (!car.IsOnFinish)
                {
                    car.Update(Delta);
                    if(car.Passed >= Distance)
                    {
                        _finishedCarsCount++;
                        car.IsOnFinish = true;
                        LeaderBoard.Add(car);
                    }
                }                         
            }       
        }

        public void Start()
        {
            if (IsStarted) return;
            RaceTimer = new Timer(Interval);
            RaceTimer.Elapsed += new ElapsedEventHandler(RaceTimerTick);
            IsStarted = true;
            RaceTimer.Enabled = true;
        }  

        public void AddCar(Car car)
        {
            if (IsStarted) return;
            CarsOnTrack.Add(car);
        } 
    }
}
