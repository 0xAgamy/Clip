using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Clips.Application.Validation;

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
