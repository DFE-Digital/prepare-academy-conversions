using ApplyToBecome.Data.Models;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Mock
{
	public class MockProjects:IProjects
	{
		public IEnumerable<Project> GetAllProjects()
		{
			return new[] {new Project {School = new School {Name = "Mock school"}}};
		}

		public Project GetProjectById(int id)
		{
			return new Project {School = new School {Name = "Mock school"}};
		}
	}
}