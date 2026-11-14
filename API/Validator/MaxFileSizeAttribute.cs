using System;
using System.ComponentModel.DataAnnotations;

namespace Clips_api.Validator;

public class MaxFileSizeAttribute:ValidationAttribute
{
    private readonly long _maxSize;
    public MaxFileSizeAttribute(long maxSize)=> _maxSize=maxSize;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
        if (file.Length > _maxSize) return new ValidationResult("File size not allowed");
        else return ValidationResult.Success; 
        

    }
}
