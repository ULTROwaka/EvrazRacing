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
        private bool isStarted;
        private float Distance;
        private float Delta;
        private List<Car> CarsOnTrack;
        private int FinishedCarsCount;
        Timer RaceTimer;
        public event EventHandler OnRaceEnd;
        public readonly List<Car> LeaderBoard;

        public Track(float distance, int interval)
        {
            Distance = distance;
            Delta = interval / 1000;
            isStarted = false;
            CarsOnTrack = new List<Car>();
            FinishedCarsCount = 0;
            RaceTimer = new Timer(interval);
            RaceTimer.Elapsed += new ElapsedEventHandler(RaceTimerTick);
            LeaderBoard = new List<Car>();
        }

        private void RaceTimerTick(Object source, ElapsedEventArgs e)
        {        
            Update();
            if (FinishedCarsCount >= CarsOnTrack.Count)
            {
                RaceTimer.Enabled = false;
                OnRaceEnd(this, new EventArgs());
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
                        FinishedCarsCount++;
                        car.IsOnFinish = true;
                        LeaderBoard.Add(car);
                    }
                }                         
            }       
        }

        public void Start()
        {
            if (isStarted) return;
            isStarted = true;
            RaceTimer.Enabled = true;
        }  

        public void AddCar(Car car)
        {
            CarsOnTrack.Add(car);
        } 
    }
}
