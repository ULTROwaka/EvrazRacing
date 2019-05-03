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
        private string _special;
        private float _carPassed;

        public CarViewModel(Car car)
        {
            _carModel = car;
            _carName = _carModel.Name;
            _carSpeed = _carModel.Speed;
            _repairTime = _carModel.RepairTime;
            _carPassed = _carModel.Passed;

            if (_carModel is Truck)
            {
                Special = (_carModel as Truck).Weight.ToString();
                _carType = "Truck";
                return;
            }
            if (_carModel is Motorcycle)
            {
                Special = (_carModel as Motorcycle).Sidecar.ToString();
                _carType = "Motorcycle";
                return;
            }
            if (_carModel is Automobile)
            {
                Special = (_carModel as Automobile).PassangersCount.ToString();
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
        public float CarPassed
        {
            get => _carPassed;
            set => this.RaiseAndSetIfChanged(ref _carPassed, value);
        }
        public string Special
        {
            get => _special;
            set => this.RaiseAndSetIfChanged(ref _special, value);
        }

        internal Car ExtractModel()
        {
            return _carModel;
        }
    }
}