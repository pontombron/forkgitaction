using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using person_wpf_demo.Model;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.Utils.Validators;
using person_wpf_demo.ViewModel;

namespace PersonTests.ViewModel
{
    public class NewAddressViewModelTest
    {
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly Mock<IAddressValidator> _addressValidatorMock;
        private readonly NewAddressViewModel _viewModel;
        private readonly Person _person;

        public NewAddressViewModelTest()
        {
            // Création des mocks pour les dépendances
            _navigationServiceMock = new Mock<INavigationService>();
            _addressServiceMock = new Mock<IAddressService>();
            _addressValidatorMock = new Mock<IAddressValidator>();

            // Création de l'instance du ViewModel avec les mocks
            _viewModel = new NewAddressViewModel(
                _navigationServiceMock.Object,
                _addressServiceMock.Object,
                _addressValidatorMock.Object);

            // Création d'une personne de test pour simuler la navigation
            _person = new Person { Id = 1 };
            _viewModel.ApplyNavigationParameters(_person);
        }

        [Fact]
        public void ApplyNavigationParameters_ShouldSetSelectedPerson()
        {
            // Arrange
            // (Déjà réalisé dans le constructeur via ApplyNavigationParameters)

            // Pour tester indirectement que _selectedPerson est bien utilisé,
            // on exécutera la commande Save et vérifiera que le service reçoit la bonne personne.
            _viewModel.Street = "123 Main St";
            _viewModel.City = "Paris";
            _viewModel.PostalCode = "75001";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert : vérification que l'adresse est ajoutée avec le bon PersonId
            _addressServiceMock.Verify(a => a.Add(
                _person,
                It.Is<Address>(addr =>
                    addr.Street == "123 Main St" &&
                    addr.City == "Paris" &&
                    addr.PostalCode == "75001" &&
                    addr.PersonId == _person.Id)),
                Times.Once);
        }

        [Fact]
        public void Street_Set_ShouldCallValidator()
        {
            // Act
            _viewModel.Street = "456 Elm St";

            // Assert : Vérifie que le validateur est appelé pour la propriété "Street"
            _addressValidatorMock.Verify(v => v.ValidateProperty(nameof(_viewModel.Street), "456 Elm St"), Times.Once);
        }

        [Fact]
        public void City_Set_ShouldCallValidator()
        {
            // Act
            _viewModel.City = "Lyon";

            // Assert : Vérifie que le validateur est appelé pour la propriété "City"
            _addressValidatorMock.Verify(v => v.ValidateProperty(nameof(_viewModel.City), "Lyon"), Times.Once);
        }

        [Fact]
        public void PostalCode_Set_ShouldCallValidator()
        {
            // Act
            _viewModel.PostalCode = "69000";

            // Assert : Vérifie que le validateur est appelé pour la propriété "PostalCode"
            _addressValidatorMock.Verify(v => v.ValidateProperty(nameof(_viewModel.PostalCode), "69000"), Times.Once);
        }

        [Fact]
        public void SaveCommand_CannotExecute_WhenAnyFieldIsEmpty()
        {
            // Arrange: aucune valeur n'est définie
            _viewModel.Street = "";
            _viewModel.City = "";
            _viewModel.PostalCode = "";

            // Act
            bool canSave = _viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canSave);
        }

        [Fact]
        public void SaveCommand_Execute_ShouldCallAddAndNavigate_WhenAllFieldsAreValid()
        {
            // Arrange : on définit toutes les valeurs nécessaires
            _viewModel.Street = "789 Oak St";
            _viewModel.City = "Marseille";
            _viewModel.PostalCode = "13000";

            // Vérification préalable que la commande peut s'exécuter
            Assert.True(_viewModel.SaveCommand.CanExecute(null));

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert : Vérifie que l'adresse a bien été ajoutée avec les données correctes
            _addressServiceMock.Verify(a => a.Add(
                _person,
                It.Is<Address>(addr =>
                    addr.Street == "789 Oak St" &&
                    addr.City == "Marseille" &&
                    addr.PostalCode == "13000" &&
                    addr.PersonId == _person.Id)),
                Times.Once);

            // Assert : Vérifie que la navigation vers PersonsViewModel est appelée
            _navigationServiceMock.Verify(n => n.NavigateTo<PersonsViewModel>(), Times.Once);
        }
    }
}
