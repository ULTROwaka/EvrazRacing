﻿using DynamicData;
using DynamicData.Binding;
using EvrazRacing.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace EvrazRacing.ViewModels
{
    class RaceViewModel : ReactiveObject
    {
        #region new car
        string _carName;
        public string CarName
        {
            get => _carName;
            set => this.RaiseAndSetIfChanged(ref _carName, value);
        }
        uint _carSpeed;
        public uint CarSpeed
        {
            get => _carSpeed;
            set => this.RaiseAndSetIfChanged(ref _carSpeed, value);
        }
        uint _repairTime;
        public uint RepairTime
        {
            get => _repairTime;
            set => this.RaiseAndSetIfChanged(ref _repairTime, value);
        }
        uint _breakChance;
        public uint BreakChance
        {
            get => _breakChance;
            set => this.RaiseAndSetIfChanged(ref _breakChance, value);
        }
        bool _isTruck;
        public bool IsTruck
        {
            get => _isTruck;
            set => this.RaiseAndSetIfChanged(ref _isTruck, value);
        }
        bool _isMotorcycle;
        public bool IsMotorcycle
        {
            get => _isMotorcycle;
            set => this.RaiseAndSetIfChanged(ref _isMotorcycle, value);
        }
        bool _isAutomobile;
        public bool IsAutomobile
        {
            get => _isAutomobile;
            set => this.RaiseAndSetIfChanged(ref _isAutomobile, value);
        }
        bool _sidecar;
        public bool Sidecar
        {
            get => _sidecar;
            set => this.RaiseAndSetIfChanged(ref _sidecar, value);
        }
        float _carWeight;
        public float CarWeight
        {
            get => _carWeight;
            set => this.RaiseAndSetIfChanged(ref _carWeight, value);
        }
        uint _carPassanger;
        public uint CarPassanger
        {
            get => _carPassanger;
            set => this.RaiseAndSetIfChanged(ref _carPassanger, value);
        }

        readonly ObservableAsPropertyHelper<bool> _gestureVisibility;
        public bool GestureVisibility => _gestureVisibility.Value;

        readonly ObservableAsPropertyHelper<string> _gesture;
        public string Gesture => _gesture.Value;

        readonly ObservableAsPropertyHelper<bool> _carWeightVisibility;
        public bool CarWeightVisibility => _carWeightVisibility.Value;

        readonly ObservableAsPropertyHelper<bool> _carPassangerVisibility;
        public bool CarPassangerVisibility => _carPassangerVisibility.Value;

        readonly ObservableAsPropertyHelper<bool> _sidecarVisibility;
        public bool SidecarVisibility => _sidecarVisibility.Value;

        readonly ObservableAsPropertyHelper<bool> _addNewCarAvailability;
        public bool AddNewCarAvailability => _addNewCarAvailability.Value;
        #endregion

        #region track
        Track _track;
        uint _trackDistance;
        public uint TrackDistance
        {
            get => _trackDistance;
            set => this.RaiseAndSetIfChanged(ref _trackDistance, value);
        }
        uint _trackInterval;
        public uint TrackInterval
        {
            get => _trackInterval;
            set => this.RaiseAndSetIfChanged(ref _trackInterval, value);
        }
        #endregion

        private SourceList<CarViewModel> carList;
        private ReadOnlyObservableCollection<CarViewModel> _carList;
        public ReadOnlyObservableCollection<CarViewModel> CarList
        {
            get => _carList;
            set => this.RaiseAndSetIfChanged(ref _carList, value);
        }

        private ReadOnlyObservableCollection<CarViewModel> _leaderboard;
        public ReadOnlyObservableCollection<CarViewModel> Leaderboard
        {
            get => _leaderboard;
            set => this.RaiseAndSetIfChanged(ref _leaderboard, value);
        }

        private IObservable<bool> _startCanExecute;

        #region commands
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Start { get; }
        #endregion

        public RaceViewModel()
        {
            _track = new Track();
            _isTruck = true;
            carList = new SourceList<CarViewModel>();
            carList
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _carList)
                .Subscribe();

            _addNewCarAvailability = this
                .WhenAnyValue(v => v.CarName)
                .Select(s => s != null && !s.Trim().Equals(string.Empty))
                .ToProperty(this, x => x.AddNewCarAvailability);

            _sidecarVisibility = this
                .WhenAnyValue(v => v.IsMotorcycle)
                .ToProperty(this, x => x.SidecarVisibility);

            _carWeightVisibility = this
                .WhenAnyValue(v => v.IsTruck)
                .ToProperty(this, x => x.CarWeightVisibility);

            _carPassangerVisibility = this
                .WhenAnyValue(v => v.IsAutomobile)
                .ToProperty(this, x => x.CarPassangerVisibility);

            _gestureVisibility = this
                .WhenAnyValue(v => v.IsMotorcycle)
                .Select(s => !s)
                .ToProperty(this, x => x.GestureVisibility);

            _gesture = this
                .WhenAnyValue(v => v.IsTruck, v => v.IsAutomobile, v => v.IsMotorcycle)
                .Select(s => s.Item1 ? "Вес грузовика" : "Кол-во пассажиров")
                .ToProperty(this, x => x.Gesture);

            _track.Leaderboard
                  .Connect()         
                  .AutoRefresh(r => r.Passed)
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .Sort(SortExpressionComparer<Car>.Descending(t => t.Passed))
                  .Transform(s => new CarViewModel(s))                 
                  .Bind(out _leaderboard)
                  .Subscribe();


            Add = ReactiveCommand.CreateFromObservable(AddCar);

            _startCanExecute = this
                .WhenAnyValue(v => v.TrackDistance, v => v.TrackInterval, v => v.CarList.Count,
                (distance, interval, carlist) => distance > 0 && interval > 0 && carList.Count > 0);
            Start = ReactiveCommand.CreateFromObservable(StartRace, _startCanExecute);
        }

        public IObservable<Unit> AddCar()
        {
            return Observable.Start(() =>
             {
                 Car newcar;
                 if (_isTruck)
                 {
                      newcar = new Truck(_carName, _carSpeed, _breakChance, _repairTime, _carWeight);
                 }
                 else if (_isMotorcycle)
                 {
                      newcar = new Motorcycle(_carName, _carSpeed, _breakChance, _repairTime, _sidecar);

                 } else
                 {
                      newcar = new Automobile(_carName, _carSpeed, _breakChance, _repairTime, _carPassanger);
                 }
                 carList.Add(new CarViewModel(newcar));
                 return;
             });
        }

        public IObservable<Unit> StartRace()
        {
            return Observable.Start(() =>
            {
                _track.Distance = _trackDistance;
                _track.Interval = _trackInterval;
                foreach (var car in _carList)
                {
                    _track.AddCar(car.ExtractModel());
                }
                _track.Start();
            });
        }
    }
}
