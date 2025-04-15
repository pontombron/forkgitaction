using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Model;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.Utils.Validators;
using System.Windows.Input;

namespace person_wpf_demo.ViewModel
{
    public class NewAddressViewModel : BaseViewModel, INavigationParameterReceiver
    {
        private readonly INavigationService _navigationService;
        private readonly IAddressService _addressService;
        private readonly IAddressValidator _addressValidator;
        private Person _selectedPerson;

        public NewAddressViewModel(INavigationService navigationService, IAddressService addressService, IAddressValidator addressValidator)
        {
            _navigationService = navigationService;
            _addressService = addressService;
            _addressValidator = addressValidator;
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public void ApplyNavigationParameters(params object[] parameters)
        {
            if (parameters?.Length > 0)
            {
                _selectedPerson = parameters[0] as Person;
            }
        }

        public IAddressValidator AddressValidator
        {
            get => _addressValidator;
        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                if (_street != value)
                {
                    _street = value;
                    OnPropertyChanged(nameof(Street));
                    _addressValidator.ValidateProperty(nameof(Street), value);
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                    _addressValidator.ValidateProperty(nameof(City), value);
                }
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode != value)
                {
                    _postalCode = value;
                    OnPropertyChanged(nameof(PostalCode));
                    _addressValidator.ValidateProperty(nameof(PostalCode), value);
                }
            }
        }

        public ICommand SaveCommand { get; set; }
        private void Save()
        {
            var newAddress = new Address
            {
                Street = Street,
                City = City,
                PostalCode = PostalCode,
                PersonId = _selectedPerson.Id
            };

            _addressService.Add(_selectedPerson, newAddress);
            _navigationService.NavigateTo<PersonsViewModel>();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Street) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(PostalCode);
        }
    }
}
