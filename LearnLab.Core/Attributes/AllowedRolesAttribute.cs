using System.ComponentModel.DataAnnotations;
using LearnLab.Core.Constants;

namespace LearnLab.Core.Attributes;
public class AllowedRolesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var role = value as string;
        if (role != null && (role == RoleNames.Student || role == RoleNames.Teacher))
            return ValidationResult.Success;

        return new ValidationResult($"Invalid role. Allowed values are '{RoleNames.Student}' and '{RoleNames.Teacher}'.");
    }
}
