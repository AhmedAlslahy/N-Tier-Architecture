namespace N_Tier.Application.Helper;

public static class HelperMethod
{
    public static class ValidationHelper
    {
        public static Error? Validate(FluentValidation.Results.ValidationResult result)
        {
            if (result.IsValid)
                return null;

            return new Error(
                "ValidationError",
                string.Join(", ", result.Errors.Select(x => x.ErrorMessage)),
                ErrorType.BadRequest);
        }
    }
}