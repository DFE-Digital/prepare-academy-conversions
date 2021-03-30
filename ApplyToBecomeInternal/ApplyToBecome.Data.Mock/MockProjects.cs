using ApplyToBecome.Data.Models;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Mock
{
	public class MockProjects:IProjects
	{
		public IEnumerable<Project> GetAllProjects()
		{
			return new[] {
				new Project {
					Id = 0,
					School = new School 
					{
						Id = 100,
						Name = "St. Wilfrid's Primary School",
						URN = "AS_102062",
						LocalAuthority = "Warrington",
						ProjectId = 0
					},
					Trust = new Trust
					{
						Id = 200,
						Name = "Dynamics Academy Trust",
						ProjectId = 0
					}
				},
				new Project {
					Id = 1,
					School = new School
					{
						Id = 101,
						Name = "Bolton Spa Primary",
						URN = "AS_675091",
						LocalAuthority = "Warrington",
						ProjectId = 1
					},
					Trust = new Trust
					{
						Id = 201,
						Name = "Oak Hill Trust",
						ProjectId = 1
					}
				},
				new Project {
					Id = 2,
					School = new School
					{
						Id = 102,
						Name = "Blessed John Henry Newman Roman Catholic College",
						URN = "AS_123920",
						LocalAuthority = "Warrington",
						ProjectId = 1
					},
					Trust = new Trust
					{
						Id = 202,
						Name = "Kingfisher learning trust",
						ProjectId = 2
					}
				},
				new Project {
					Id = 3,
					School = new School
					{
						Id = 103,
						Name = "Bolton Spa Primary",
						URN = "AS_112456",
						LocalAuthority = "Bolton",
						ProjectId = 3
					},
					Trust = new Trust
					{
						Id = 203,
						Name = "Oak Hill Trust",
						ProjectId = 3
					}
				},
				new Project {
					Id = 4,
					School = new School
					{
						Id = 104,
						Name = "Ancoats Free School",
						URN = "AS_102045",
						LocalAuthority = "Greater Manchester",
						ProjectId = 4
					},
					Trust = new Trust
					{
						Id = 204,
						Name = "United Learning Trust",
						ProjectId = 4
					}
				},
			};
		}

		public Project GetProjectById(int id)
		{
			return new Project {School = new School {Name = "Mock school"}};
		}
	}
}