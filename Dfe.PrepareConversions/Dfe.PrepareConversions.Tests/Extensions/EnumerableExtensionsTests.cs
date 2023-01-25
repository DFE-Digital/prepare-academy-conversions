using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions
{
	public class EnumerableExtensionsTests
	{

		[Fact]
		public void Should_remove_non_distinct_elements_when_they_are_present()
		{
			DataWithDuplicatesForEnumerableTests.DistinctBy(p => p.Property1).Should().HaveCount(3);
		}
		[Fact]
		public void Should_remove_no_elements_when_all_unique()
		{
			DataForEnumerableTests.DistinctBy(p => p.Property1).Should().HaveCount(4);
		}
		[Fact]
		public void Should_still_return_all_elements_if_other_properties_are_not_distinct()
		{
			DataWithDuplicatesForEnumerableTests.DistinctBy(p => p.Property2).Should().HaveCount(4);
		}

		public class TestClass
		{
			public string Property1 { get; set; }
			public string Property2 { get; set; }
		}

		public static IEnumerable<TestClass> DataWithDuplicatesForEnumerableTests
		{
			get
			{
				List<TestClass> listWithDuplicate = new List<TestClass>
				{
						new TestClass() { Property1 = "Test1", Property2 = "Test2" },
						new TestClass() { Property1 = "Test1", Property2 = "Test1" },
						new TestClass() { Property1 = "Test2", Property2 = "Test3" },
						new TestClass() { Property1 = "Test3", Property2 = "Test4" }
					};
				return listWithDuplicate;
			}
		}
		public static IEnumerable<TestClass> DataForEnumerableTests
		{
			get
			{
				List<TestClass> listWithDuplicate = new List<TestClass>
				{
					new TestClass() { Property1 = "Test1", Property2 = "Test2" },
					new TestClass() { Property1 = "Test2", Property2 = "Test3" },
					new TestClass() { Property1 = "Test4", Property2 = "Test5" },
					new TestClass() { Property1 = "Test6", Property2 = "Test7" }
				};
				return listWithDuplicate;
			}
		}
	}
}
