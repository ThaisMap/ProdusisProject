using System.Globalization;
using System.Windows.Controls;

namespace Produsis.Validacoes
{
    internal class ValidacaoCampoPreenchido : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString() == "")
            {
                return new ValidationResult(false, "Favor preencher o campo.");
            }
            else
            {
                return new ValidationResult(true, "");
            }
        }
    }
}