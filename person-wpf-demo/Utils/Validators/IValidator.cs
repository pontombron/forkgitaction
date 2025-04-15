using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace person_wpf_demo.Utils.Validators
{
    public interface IValidator
    {
        public void ValidateProperty(string propertyName, string value);
    }
}
