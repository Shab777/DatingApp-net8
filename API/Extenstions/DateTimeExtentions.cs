using System;

namespace API.Extenstions;

public static class DateTimeExtentions
{   
    public static int  CalculateAge (this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        
        var age = today.Year - dob.Year;

        if(dob > today.AddDays(-age)) age--;

        return age;
    }

}
