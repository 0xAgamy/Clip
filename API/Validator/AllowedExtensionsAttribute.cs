using System;
using System.ComponentModel.DataAnnotations;

namespace Clips_api.Validator;

public class AllowedExtensionsAttribute:ValidationAttribute
{

    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(params string[] extensions) => _extensions = extensions;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is not IFormFile file) return ValidationResult.Success;
        var ext= Path.GetExtension(file.FileName).ToLowerInvariant();
        return _extensions.Contains(ext)
                ? ValidationResult.Success:
                 new ValidationResult($"File type {ext} not allowed");
    }

}
