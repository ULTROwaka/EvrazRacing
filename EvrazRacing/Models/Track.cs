using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;

namespace EvrazRacing.Models
{
    class Track
    {

        public readonly ObservableCollectionExtended<Car> CarsOnTrack;
        public readonly SourceList<Car> Leaderboard;
        public readonly SourceList<string> EventLog;
        private Timer RaceTimer;

        public float Delta
        {
            get => (float)Interval / 1000;
        }
        public bool IsStarted
        {
            get; set;
        }
        public float Distance
        {
            get; set;
        }
        public uint Interval
        {
            get; set;
        }
        public int FinishedCarsCount
        {
            get; private set;
        }
        public bool IsFinished
        {
            get;
        }

        public Track()
        {
            Interval = 1000;
            FinishedCarsCount = 0;
            IsStarted = false;
            IsFinished = false;
            CarsOnTrack = new ObservableCollectionExtended<Car>();
            Leaderboard = new SourceList<Car>();
            EventLog = new SourceList<string>();
        }

        private void RaceTimerTick(Object source, ElapsedEventArgs e)
        {
            Update();
            if (FinishedCarsCount >= CarsOnTrack.Count)
            {
                RaceTimer.Enabled = false;
                IsStarted = false;
#if DEBUG
                Debug.Print("race end");
#endif
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
                        car.Passed = Distance;
                        Debug.Print($"{car.Name} on finish");
                        EventLog.Add($"{car.Name} on finish");
                    }
                }
#if DEBUG
                Debug.Print("race tick");
#endif
            }
        }

        public void Start()
        {
            if (IsStarted)
            {
                return;
            }              
            Leaderboard.Clear();
            EventLog.Clear();
            FinishedCarsCount = 0;
#if DEBUG
            Debug.Print("race start");
#endif
            EventLog.Add("В гонке участвуют:");
            foreach (var car in CarsOnTrack)
            {
                car.SetOnStart();
                Leaderboard.Add(car);
                EventLog.Add(car.StartMessage);
            }
            RaceTimer = new Timer(Interval);
            RaceTimer.Elapsed += new ElapsedEventHandler(RaceTimerTick);
            IsStarted = true;
            RaceTimer.Enabled = true;
        }

        public void AddCar(Car car)
        {
            if (IsStarted)
            {
                return;
            }             
            car.OnBreaking += Car_OnBreaking;
            CarsOnTrack.Add(car);
        }

        private void Car_OnBreaking(object sender, EventArgs e)
        {
#if DEBUG
            Debug.Print($"{(sender as Car).Name} OnPitStop");
#endif
            EventLog.Add($"{(sender as Car).Name} on pitstop");
        }

        internal void DeleteCar(Car car)
        {
            if (IsStarted)
            {
                return;
            }
            car.OnBreaking -= Car_OnBreaking;
            CarsOnTrack.Remove(car);
        }
    }
}
