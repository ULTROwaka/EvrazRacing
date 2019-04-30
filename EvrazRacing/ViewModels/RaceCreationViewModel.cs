using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace EvrazRacing.ViewModels
{
    class RaceCreationViewModel : ReactiveObject
    {
        string _carName;
        public string CarName
        {
            get => _carName;
            set => this.RaiseAndSetIfChanged(ref _carName, value);
        }

        string _carSpeed;
        public string CarSpeed
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

        readonly ObservableAsPropertyHelper<bool> _carWeightPassangerVisibility;
        public bool CarWeightPassangerVisibility => _carWeightPassangerVisibility.Value;

        readonly ObservableAsPropertyHelper<bool> _sidecarVisibility;
        public bool SidecarVisibility => _sidecarVisibility.Value;
     

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

        List<CarViewModel> _carList;
        public List<CarViewModel> CarList
        {
            get => _carList;
            set => this.RaiseAndSetIfChanged(ref _carList, value);
        }

        public ReactiveCommand<Unit, Unit> AddCar { get; }

        public RaceCreationViewModel()
        {
            _isTruck = true;
            _carList = new List<CarViewModel>();
            _sidecarVisibility = this
                .WhenAnyValue(v => v.IsMotorcycle)
                .ToProperty(this, x => x.SidecarVisibility);

            _carWeightPassangerVisibility = this
                .WhenAnyValue(v => v.IsTruck, v => v.IsAutomobile)
                .Select(s => s.Item1 || s.Item2)
                .ToProperty(this, x => x.CarWeightPassangerVisibility);

            AddCar = ReactiveCommand.Create( () =>
            {
                CarList.Add(new CarViewModel(_carName, _carSpeed, _repairTime, "Truck", 12.3F));
            } );
        }
    }
}
