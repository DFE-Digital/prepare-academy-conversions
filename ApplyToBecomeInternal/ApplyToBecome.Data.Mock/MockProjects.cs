using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.ProjectNotes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecome.Data.Mock
{
	public class MockProjects:IProjects
	{
		private readonly Project[] _projects = {
			new Project
			{
				Id = 0,
				School = new School {Id = 100, Name = "St. Wilfrid's Primary School", URN = "AS_102062", LocalAuthority = "Warrington"},
				Trust = new Trust {Id = 200, Name = "Dynamics Academy Trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
			},
			new Project
			{
				Id = 1,
				School = new School {Id = 101, Name = "Bolton Spa Primary", URN = "AS_675091", LocalAuthority = "Warrington"},
				Trust = new Trust {Id = 201, Name = "Oak Hill Trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
			},
			new Project
			{
				Id = 2,
				School = new School {Id = 102, Name = "Blessed John Henry Newman Roman Catholic College", URN = "AS_123920", LocalAuthority = "Warrington"},
				Trust = new Trust {Id = 202, Name = "Kingfisher learning trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
			},
			new Project
			{
				Id = 3,
				School = new School {Id = 103, Name = "Bolton Spa Primary", URN = "AS_112456", LocalAuthority = "Bolton"},
				Trust = new Trust {Id = 203, Name = "Oak Hill Trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
			},
			new Project
			{
				Id = 4,
				School = new School {Id = 104, Name = "Ancoats Free School", URN = "AS_102045", LocalAuthority = "Greater Manchester"},
				Trust = new Trust {Id = 204, Name = "United Learning Trust"}
			},
		};
			
		public IEnumerable<Project> GetAllProjects() => _projects;

		public Project GetProjectById(int id) => _projects[id] ?? throw new Exception();

		public Project UpdateProjectWithNewNote(int id, ProjectNote note)
		{
			var project = _projects.Where(x => x.Id == id).FirstOrDefault();
			project.AddNote(note);

			return project;
		}
	}
}