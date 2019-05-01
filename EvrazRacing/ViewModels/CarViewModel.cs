using EvrazRacing.Models;
using ReactiveUI;
namespace EvrazRacing.ViewModels
{
    internal class CarViewModel : ReactiveObject
    {
        private Car _carModel;
        private string _carName;
        private float _carSpeed;
        private uint _repairTime;
        private string _carType;
        private float _weight;
        private uint _passangerCount;
        private bool _sidecar;

        public CarViewModel(Car car)
        {
            _carModel = car;
            _carName = _carModel.Name;
            _carSpeed = _carModel.Speed;
            _repairTime = _carModel.RepairTime;
            
            if (_carModel is Truck)
            {
                _weight = (_carModel as Truck).Weight;
                _carType = "Truck";
                return;
            }
            if (_carModel is Motorcycle)
            {
                _sidecar = (_carModel as Motorcycle).Sidecar;
                _carType = "Motorcycle";
                return;
            }
            if (_carModel is Automobile)
            {
                _passangerCount = (_carModel as Automobile).PassangersCount;
                _carType = "Automobile";
                return;
            }
        }

        public string CarName
        {
            get => _carName;
            set => this.RaiseAndSetIfChanged(ref _carName, value);
        }
        public float CarSpeed
        {
            get => _carSpeed;
            set => this.RaiseAndSetIfChanged(ref _carSpeed, value);
        }
        public uint RepairTime
        {
            get => _repairTime;
            set => this.RaiseAndSetIfChanged(ref _repairTime, value);
        }
        public string CarType
        {
            get => _carType;
            set => this.RaiseAndSetIfChanged(ref _carType, value);
        }
    }
}