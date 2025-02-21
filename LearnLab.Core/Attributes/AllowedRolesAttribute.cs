using LearnLab.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace LearnLab.Core.Attributes;
public class AllowedRolesAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var role = value as string;
        if(role != null && (role == RoleNames.Customer || role == RoleNames.Seller))
            return ValidationResult.Success;
        return new ValidationResult($"Invalid role. Allowed values are '{RoleNames.Customer}' and '{RoleNames.Seller}'.");
    }
}
