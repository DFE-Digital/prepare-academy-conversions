using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Dfe.PrepareConversions.Data.Tests.AutoFixture;

public class AutoMoqDataAttribute : AutoDataAttribute
{
   public AutoMoqDataAttribute()
      : base(() =>
      {
         return new Fixture().Customize(new CompositeCustomization(new AutoMoqCustomization()));
      })
   {
   }
}
