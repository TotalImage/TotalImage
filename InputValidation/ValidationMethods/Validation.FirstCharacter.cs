using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Properties;
using TotalImage.Validation;

/// <summary>
/// Validates the first character of a DOS 8.3 filename (in the case it has been overwritten with 0xE5) (2020/08/01)
/// </summary>
namespace TotalImage.Validation.ValidationMethods
{
    public class FirstCharacterValidation : IValidation
    {
        public object Validate(object Data) 
        {
            // Return if not char
            Data = ValidationCore(Data);

            if (Data == null)
            {
                OnFail();
            }
            else
            {
                return Data;
            }

            //blah
            return null; 

        }

        private object ValidationCore(object Data)
        {
            if (!VerifyDataType(Data))
            {
                return null;
            }
            else
            {
                // Convert to character. It must be a char by this point. 
                char D = (char)Data;

                // Control characters not allowed.
                if (D <= 0x1F) return null;

                // Other invalid characters. (what to do about space?)
                if (D == '#'
                    || D == '*'
                    || D == '+'
                    || D == ','
                    || D == '/'
                    || D == ':'
                    || D == ';'
                    || D == '<'
                    || D == '='
                    || D == '>'
                    || D == '?'
                    || D == '\\' // requires escaping 
                    || D == '['
                    || D == ']'
                    || D == '|') return null;

                // Lowercase to uppercase. 
                if (D >= 0x61 || D <= 0x7A)
                {
                    // why isn't (D -= 0x20) valid? 

                    // Alternatively you could use a string and ToLowerInvariant to do this.
                    byte _ = (byte)D;
                    _ -= 0x20;
                    D = (char)_;
                }

                return D;
            }
        }

        public void OnFail()
        {
            MessageBox.Show(Resources.TI_Validate_FirstChar_OnFail, Resources.TI_Generic_Warning, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Just temporarily not implemented for now, since undeletion itself seems to not be implemented
        public void OnSuccess()
        {
            throw new NotImplementedException();
        }

        // ca1822 suppressed
        private bool VerifyDataType(object Data)
        {
            return Data is Char;
        }
    }
}
