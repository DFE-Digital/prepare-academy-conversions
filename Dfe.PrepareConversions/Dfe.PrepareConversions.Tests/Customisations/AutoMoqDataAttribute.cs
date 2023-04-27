using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Dfe.PrepareConversions.Tests.Customisations;

public class AutoMoqDataAttribute : AutoDataAttribute
{
   public AutoMoqDataAttribute()
      : base(() => new Fixture().Customize(new CompositeCustomization(new AutoMoqCustomization())))
   {
   }
}
