using EvrazRacing.Models;
using ReactiveUI;
using System;

namespace EvrazRacing.ViewModels
{
    internal class CarViewModel : ReactiveObject
    {
        public readonly Car _carModel;
 


        public CarViewModel(Car car)
        {
            _carModel = car;
            CarName = _carModel.Name;
            CarSpeed = _carModel.Speed;
            RepairTime = _carModel.RepairTime;
            _carPassed = _carModel
                .WhenAnyValue(x => x.Passed)
                .ToProperty(this, p => p.CarPassed);

               

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

        
       
        
        
        private ObservableAsPropertyHelper<float> _carPassed;

        private string _carName;
        public string CarName
        {
            get => _carName;
            set => this.RaiseAndSetIfChanged(ref _carName, value);
        }

        private float _carSpeed;
        public float CarSpeed
        {
            get => _carSpeed;
            set => this.RaiseAndSetIfChanged(ref _carSpeed, value);
        }

        private uint _repairTime;
        public uint RepairTime
        {
            get => _repairTime;
            set => this.RaiseAndSetIfChanged(ref _repairTime, value);
        }

        private string _carType;
        public string CarType
        {
            get => _carType;
            set => this.RaiseAndSetIfChanged(ref _carType, value);
        }
        public float CarPassed => _carPassed.Value;

        private string _special;
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