using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace person_wpf_demo.Utils.Validators
{
    public class PersonValidator : BaseViewModel, IPersonValidator
    {
        public void ValidateProperty(string propertyName, string value)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case "Prenom":
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le prénom est requis.");
                    }
                    else if (value.Length < 2)
                    {
                        AddError(propertyName, "Le prénom doit contenir au moins 2 caractères.");
                    }
                    break;
                case "Nom":
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le nom est requis.");
                    }
                    else if (value.Length < 2)
                    {
                        AddError(propertyName, "Le nom doit contenir au moins 2 caractères.");
                    }
                    break;
            }
            OnPropertyChanged(nameof(ErrorMessages));
        }
    }
}
