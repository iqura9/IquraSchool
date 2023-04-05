using System;
using System.ComponentModel.DataAnnotations;

public class DateLessThanOrEqualToTodayAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true; 
        }

        DateTime date = (DateTime)value;
        return date <= DateTime.Now;
    }
}