using ReactiveUI;
namespace EvrazRacing.ViewModels
{
    internal class CarViewModel : ReactiveObject
    {
        private string _carName;
        private string _carSpeed;
        private uint _repairTime;
        private string _carType;
        private float _weight;
        private uint _passangerCount;
        private bool _sidecar;

        public CarViewModel(string carName, string carSpeed, uint repairTime, string carType, object specialize)
        {
            CarName = carName;
            CarSpeed = carSpeed;
            RepairTime = repairTime;
            CarType = carType;
            switch (carType)
            {
                case "Truck": _weight = (float)specialize; break;
                case "Motorcycle": _sidecar = (bool)specialize; break;
                case "Automobile": _passangerCount = (uint)specialize; break;
            }
        }

        public string CarName
        {
            get => _carName;
            set => this.RaiseAndSetIfChanged(ref _carName, value);
        }
        public string CarSpeed
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