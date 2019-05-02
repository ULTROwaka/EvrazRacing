using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EvrazRacing.Models
{
    class Track
    {

        public readonly List<Car> CarsOnTrack;
        public readonly SourceList<Car> Leaderboard;
        private Timer RaceTimer;

        private int _finishedCarsCount;
        private uint _interval;
        private float _distance;
        private bool _isStarted;
        private bool _isFinished;

        public float Delta
        {
            get => (float)Interval / 1000;
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
            Leaderboard = new SourceList<Car>();
        }

        private void RaceTimerTick(Object source, ElapsedEventArgs e)
        {        
            Update();
            if (FinishedCarsCount >= CarsOnTrack.Count)
            {
                RaceTimer.Enabled = false;
                Debug.Print("race end");
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
                        Debug.Print($"{car.Name} on finish");
                        Leaderboard.Add(car);
                    }
                }
                Debug.Print("race tick");
            }       
        }

        public void Start()
        {
            if (IsStarted) return;
            Debug.Print("race start");
            RaceTimer = new Timer(Interval);
            RaceTimer.Elapsed += new ElapsedEventHandler(RaceTimerTick);
            IsStarted = true;
            RaceTimer.Enabled = true;
        }  

        public void AddCar(Car car)
        {
            if (IsStarted) return;
            car.OnBreaking += Car_OnBreaking;
            CarsOnTrack.Add(car);
        }

        private void Car_OnBreaking(object sender, EventArgs e)
        {
            Debug.Print($"{(sender as Car).Name} OnPitStop");
        }
    }
}
