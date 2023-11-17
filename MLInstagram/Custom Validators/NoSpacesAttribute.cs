using System.ComponentModel.DataAnnotations;

namespace MLInstagram.Validations
{
    //    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    //AllowMultiple = false)]
    public class NoSpacesAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string? x = value as string;
            if (x.Contains(" "))
            {
                return false;
            }
            return true;
        }
    }
}
