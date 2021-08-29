using System;
using System.Globalization;
using System.Windows.Controls;

namespace SearchEngineInsightApp
{
    public class UrlValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool validUrl = Uri.TryCreate(value.ToString(), UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!validUrl)
            {
                return new ValidationResult(false, $"Url format invalid '{value}'");
            }

            return ValidationResult.ValidResult;
        }
    }
}
