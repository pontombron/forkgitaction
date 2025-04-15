using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace person_wpf_demo.Utils.Validators
{
    public class AddressValidator : BaseViewModel, IAddressValidator
    {
        public void ValidateProperty(string propertyName, string value)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case "Street":
                    if (value.Empty())
                    {
                        AddError(propertyName, "La rue est requise.");
                    }
                    break;
                case "City":
                    if (value.Empty())
                    {
                        AddError(propertyName, "La ville est requise.");
                    }
                    break;
                case "PostalCode":
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le code postal est requis.");
                    }
                    break;
            }
            OnPropertyChanged(nameof(ErrorMessages));
        }
    }
}
