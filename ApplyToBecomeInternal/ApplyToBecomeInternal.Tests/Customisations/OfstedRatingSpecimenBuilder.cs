using AutoFixture.Kernel;
using System;
using System.Reflection;

namespace ApplyToBecomeInternal.Tests.Customisations
{
	public class OfstedRatingSpecimenBuilder : ISpecimenBuilder
	{
		public object Create(object request, ISpecimenContext context)
		{
			var pi = request as PropertyInfo;
			if (pi == null || pi.PropertyType != typeof(string))
				return new NoSpecimen();

			var i = new Random().Next();

			return "1234".Substring(i % 4, 1);
		}
	}
}
