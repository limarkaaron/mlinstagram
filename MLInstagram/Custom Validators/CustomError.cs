using Microsoft.AspNetCore.Identity;

namespace MLInstagram.Custom_Validators
{
	public class CustomError : IdentityErrorDescriber
	{
		public override IdentityError DuplicateUserName(string userName)
		{
			var error = base.DuplicateUserName(userName);
			error.Description = $"{userName} has already been registered. Please enter another email address.";
			return error;
		}
	}
}
