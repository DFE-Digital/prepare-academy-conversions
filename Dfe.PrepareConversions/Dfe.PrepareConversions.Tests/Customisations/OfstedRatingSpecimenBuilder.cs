using AutoFixture.Kernel;
using System;
using System.Reflection;

namespace Dfe.PrepareConversions.Tests.Customisations;

public class OfstedRatingSpecimenBuilder : ISpecimenBuilder
{
   public object Create(object request, ISpecimenContext context)
   {
      PropertyInfo pi = request as PropertyInfo;
      if (pi == null || pi.PropertyType != typeof(string))
         return new NoSpecimen();

      int i = new Random().Next();

      return "1234".Substring(i % 4, 1);
   }
}
