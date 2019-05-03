using DynamicData;
using EvrazRacing.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace EvrazRacing.Models
{
    class Track
    {

        public readonly List<Car> CarsOnTrack;
        public readonly SourceList<Car> Leaderboard;
        private Timer RaceTimer;

        public float Delta
        {
            get => (float)Interval / 1000;
        }
        public bool IsStarted { get; set; }
        public float Distance { get; set; }
        public uint Interval { get; set; }
        public int FinishedCarsCount { get; private set; }
        public bool IsFinished { get; }

        public Track()
        {
            Interval = 1000;
            FinishedCarsCount = 0;
            IsStarted = false;
            IsFinished = false;
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
                    if (car.Passed >= Distance)
                    {
                        FinishedCarsCount++;
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
            Leaderboard.Clear();
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
