using Microsoft.CodeAnalysis.CSharp.Syntax;
using MLInstagram.Data;
using System.ComponentModel.DataAnnotations;

namespace MLInstagram.Validations
{
	public class UniqueAttribute : ValidationAttribute
	{
		private readonly ApplicationDbContext _context;

		public UniqueAttribute(ApplicationDbContext context)
		{
			_context = context;
		}
		public override bool IsValid(object value)
		{
			string? x = value as string;
			foreach(var user in _context.Users)
			{
				if (user.IGHandle.ToLower() == x.ToString().ToLower())
				{
					return false;
				}
			}
			return true;
		}
	}
}
