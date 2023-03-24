using FluentValidation;
#nullable disable
namespace UrlShortener.Application.ValidationRules
{
    public static class RulesExtensions
    {
        public static IRuleBuilderOptions<T, string> MustUrlAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                      || url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                      || url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Not Url.");
        }
    }
}
