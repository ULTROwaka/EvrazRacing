using DynamicData;
using DynamicData.Binding;
using EvrazRacing.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
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
        uint _carSpeed = 10;
        public uint CarSpeed
        {
            get => _carSpeed;
            set => this.RaiseAndSetIfChanged(ref _carSpeed, value);
        }
        uint _repairTime = 1;
        public uint RepairTime
        {
            get => _repairTime;
            set => this.RaiseAndSetIfChanged(ref _repairTime, value);
        }
        uint _breakChance = 15;
        public uint BreakChance
        {
            get => _breakChance;
            set => this.RaiseAndSetIfChanged(ref _breakChance, value);
        }
        bool _isTruck = true;
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
        float _carWeight = 5;
        public float CarWeight
        {
            get => _carWeight;
            set => this.RaiseAndSetIfChanged(ref _carWeight, value);
        }
        uint _carPassanger = 1;
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

        private CarViewModel _selectdCar;
        public CarViewModel SelectedCar
        {
            get => _selectdCar;
            set => this.RaiseAndSetIfChanged(ref _selectdCar, value);
        }

        private ReadOnlyObservableCollection<CarViewModel> _leaderboard;
        public ReadOnlyObservableCollection<CarViewModel> Leaderboard
        {
            get => _leaderboard;
            set => this.RaiseAndSetIfChanged(ref _leaderboard, value);
        }

        private ReadOnlyObservableCollection<string> _eventLog;
        public ReadOnlyObservableCollection<string> EventLog
        {
            get => _eventLog;
            set => this.RaiseAndSetIfChanged(ref _eventLog, value);
        }


        #region commands
        private IObservable<bool> _addCanExecute;
        public ReactiveCommand<Unit, Unit> Add
        {
            get;
        }
        private IObservable<bool> _deleteCanExecute;
        public ReactiveCommand<Unit, Unit> Delete
        {
            get;
        }
        private IObservable<bool> _startCanExecute;
        public ReactiveCommand<Unit, Unit> Start
        {
            get;
        }
        #endregion

        public RaceViewModel()
        {
            _track = new Track();
            carList = new SourceList<CarViewModel>();
            carList
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _carList)
                .Subscribe();

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

            _track.EventLog
                  .Connect()
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .Bind(out _eventLog)
                  .Subscribe();

            _track.CarsOnTrack
                  .ToObservableChangeSet()     
                  .Transform(s => new CarViewModel(s))
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .Bind(out _carList)
                  .Subscribe();

            _addCanExecute = this
                .WhenAnyValue(v => v.CarName, v => v.CarSpeed, v => v.RepairTime, v => v.BreakChance,
                (name, speed, repairTime, breakchance) => name != null && !name.Trim().Equals(string.Empty) && speed > 0 && breakchance <= 100 && breakchance >= 0 && repairTime > 0);
            Add = ReactiveCommand.CreateFromObservable(AddCar, _addCanExecute);

            _deleteCanExecute = this
                .WhenAnyValue(v => v.SelectedCar)
                .Select(s => s != null);
            Delete = ReactiveCommand.CreateFromObservable(DeleteCar, _deleteCanExecute);

            _startCanExecute = this
                .WhenAnyValue(v => v.TrackDistance, v => v.TrackInterval, v => v.CarList.Count,
                (distance, interval, carcount) => distance > 0 && interval > 0 && carcount > 0);
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

                 }
                 else
                 {
                     newcar = new Automobile(_carName, _carSpeed, _breakChance, _repairTime, _carPassanger);
                 }
                 foreach (var car in CarList)
                 {
                     if (car.Equals(newcar))
                     {
                         return;
                     }
                 }
                 _track.AddCar(newcar);
             });
        }
        public IObservable<Unit> DeleteCar()
        {
            return Observable.Start(() =>
            {
                _track.DeleteCar(_selectdCar.ExtractModel());
            });
        }

        public IObservable<Unit> StartRace()
        {
            return Observable.Start(() =>
            {
                _track.Distance = _trackDistance;
                _track.Interval = _trackInterval;
                _track.Start();
            });
        }
    }
}
