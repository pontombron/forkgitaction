using System.Windows.Input;
using person_wpf_demo.Model;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.Utils.Validators;

namespace person_wpf_demo.ViewModel
{
    public class NewPersonViewModel : BaseViewModel
    {
        private readonly IPersonService _personService;
        private readonly INavigationService _navigationService;
        private readonly IPersonValidator _personValidator;
        private readonly IAddressValidator _addressValidator;


        public IPersonValidator PersonValidator
        {
            get => _personValidator;
        }
        public IAddressValidator AddressValidator
        {
            get => _addressValidator;
        }

        private string _prenom;
        public string Prenom
        {
            get => _prenom;
            set
            {
                if (_prenom != value)
                {
                    _prenom = value;
                    OnPropertyChanged(nameof(Prenom));
                    _personValidator.ValidateProperty(nameof(Prenom), value);
                }
            }
        }

        private string _nom;
        public string Nom
        {
            get => _nom;
            set
            {
                if (_nom != value)
                {
                    _nom = value;
                    OnPropertyChanged(nameof(Nom));
                    _personValidator.ValidateProperty(nameof(Nom), value);
                }
            }
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

        public NewPersonViewModel(IPersonService personService, INavigationService navigationService, IPersonValidator personValidator, IAddressValidator addressValidator)
        {
            _personService = personService;
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save, CanSave);
            _addressValidator = addressValidator;
            _personValidator = personValidator;
        }

        public ICommand SaveCommand { get; set; }
        private void Save()  
        {
            Person person = new Person()
            {
                Prenom = Prenom,
                Nom = Nom,
                Addresses = new List<Address>
                {
                    new Address()
                    {
                        Street = Street,
                        City = City,
                        PostalCode = PostalCode
                    }
                }
            };
            _personService.Add(person);
            _navigationService.NavigateTo<PersonsViewModel>();
        }
        private bool CanSave()
        {
            bool allRequiredFieldsAreEntered = Prenom.NotEmpty() && Nom.NotEmpty() && Street.NotEmpty() && City.NotEmpty() && PostalCode.NotEmpty();

            return !HasErrors && allRequiredFieldsAreEntered;
        }
    }
}
