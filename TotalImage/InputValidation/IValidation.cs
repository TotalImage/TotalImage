using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface for various forms of input validation in TotalImage.
/// </summary>
namespace TotalImage.Validation
{
    public interface IValidation
    {
        object Validate(object Data); // Can return modified data. For example, upper-to-lower case.
        void OnFail();
        void OnSuccess();

    }
}
