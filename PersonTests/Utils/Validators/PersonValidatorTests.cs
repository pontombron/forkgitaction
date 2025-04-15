using System.ComponentModel.DataAnnotations;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Validators;

namespace PersonTests.Utils.Validators
{
    public class PersonValidatorTests
    {
        private readonly PersonValidator _validator;

        public PersonValidatorTests()
        {
            _validator = new PersonValidator();
        }

        // --- Tests pour la propriété "Prenom" ---
        [Fact]
        public void ValidateProperty_PrenomEmpty_ShouldAddRequiredError()
        {
            // Arrange
            var validator = _validator;

            // Act
            validator.ValidateProperty("Prenom", "");

            // Assert
            Assert.True(!string.IsNullOrEmpty(validator.ErrorMessages));
            Assert.Contains("Le prénom est requis.", validator.ErrorMessages);
        }

        [Fact]
        public void ValidateProperty_PrenomTooShort_ShouldAddMinLengthError()
        {
            // Arrange
            var validator = _validator;

            // Act
            validator.ValidateProperty("Prenom", "A");

            // Assert
            Assert.True(!string.IsNullOrEmpty(validator.ErrorMessages));
            Assert.Contains("Le prénom doit contenir au moins 2 caractères.", validator.ErrorMessages);
        }

        [Fact]
        public void ValidateProperty_PrenomValid_ShouldNotAddError()
        {
            // Arrange
            var validator = _validator;

            // Act
            validator.ValidateProperty("Prenom", "Alexandre");

            // Assert
            // Lorsque la valeur est valide, ErrorMessages doit être vide ou null
            Assert.True(string.IsNullOrEmpty(validator.ErrorMessages));
        }

        // --- Tests pour la propriété "Nom" ---
        [Fact]
        public void ValidateProperty_NomEmpty_ShouldAddRequiredError()
        {
            // Arrange
            var validator = _validator;

            // Act
            validator.ValidateProperty("Nom", "");

            // Assert
            Assert.True(!string.IsNullOrEmpty(validator.ErrorMessages));
            Assert.Contains("Le nom est requis.", validator.ErrorMessages);
        }

        [Fact]
        public void ValidateProperty_NomTooShort_ShouldAddMinLengthError()
        {
            // Arrange
            var validator = _validator;

            // Act
            validator.ValidateProperty("Nom", "B");

            // Assert
            Assert.True(!string.IsNullOrEmpty(validator.ErrorMessages));
            Assert.Contains("Le nom doit contenir au moins 2 caractères.", validator.ErrorMessages);
        }

        [Fact]
        public void ValidateProperty_NomValid_ShouldNotAddError()
        {
            // Arrange
            var validator = _validator;

            // Act
            validator.ValidateProperty("Nom", "Dupont");

            // Assert
            Assert.True(string.IsNullOrEmpty(validator.ErrorMessages));
        }

        // --- Vérification que les erreurs sont effacées avant chaque validation ---
        [Fact]
        public void ValidateProperty_ErrorsClearedBeforeValidation()
        {
            // Arrange
            var validator = _validator;

            // Act : première validation erronée pour générer une erreur
            validator.ValidateProperty("Prenom", "");
            Assert.True(!string.IsNullOrEmpty(validator.ErrorMessages));
            Assert.Contains("Le prénom est requis.", validator.ErrorMessages);

            // Act : validation correcte qui doit effacer l'erreur précédente
            validator.ValidateProperty("Prenom", "John");

            // Assert : ErrorMessages doit être vide
            Assert.True(string.IsNullOrEmpty(validator.ErrorMessages));
        }
    }
}
