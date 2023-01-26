using AutoFixture;
using AutoFixture.Kernel;
using System;

namespace Dfe.PrepareConversions.Tests.Customisations
{
	internal class RandomDateBuilder : ISpecimenBuilder
	{
		private readonly ISpecimenBuilder _innerGenerator;

		internal RandomDateBuilder(DateTime min, DateTime max)
		{
			_innerGenerator = new RandomDateTimeSequenceGenerator(min, max);
		}

		public object Create(object request, ISpecimenContext context)
		{
			var result = _innerGenerator.Create(request, context);
			if (result is NoSpecimen)
				return result;

			return ((DateTime)result).Date;
		}
	}
}
