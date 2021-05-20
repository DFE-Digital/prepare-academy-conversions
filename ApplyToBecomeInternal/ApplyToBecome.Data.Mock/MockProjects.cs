using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.GenerateHTBTemplate;
using System;
using System.Collections.Generic;

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
				ProjectDocuments = new []
				{
					new DocumentDetails
					{
						Name = "Wilfreds-Dynamics-HTB-temp-13-March-2021.docx",
						Type = "Word document",
						Size = "267kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-temp-13March2021.docx",
						Type = "Word document",
						Size = "112kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-update-May-2021.docx",
						Type = "Word document",
						Size = "854kb"
					}
				}
			},
			new Project
			{
				Id = 1,
				School = new School {Id = 101, Name = "Bolton Spa Primary", URN = "AS_675091", LocalAuthority = "Warrington"},
				Trust = new Trust {Id = 201, Name = "Oak Hill Trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
				ProjectDocuments = new[]
				{
					new DocumentDetails
					{
						Name = "Wilfreds-Dynamics-HTB-temp-13-March-2021.docx",
						Type = "Word document",
						Size = "267kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-temp-13March2021.docx",
						Type = "Word document",
						Size = "112kb"
					}
				}
			},
			new Project
			{
				Id = 2,
				School = new School {Id = 102, Name = "Blessed John Henry Newman Roman Catholic College", URN = "AS_123920", LocalAuthority = "Warrington"},
				Trust = new Trust {Id = 202, Name = "Kingfisher learning trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
				ProjectDocuments = new []
				{
					new DocumentDetails
					{
						Name = "Wilfreds-Dynamics-HTB-temp-13-March-2021.docx",
						Type = "Word document",
						Size = "267kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-temp-13March2021.docx",
						Type = "Word document",
						Size = "112kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-update-May-2021.docx",
						Type = "Word document",
						Size = "854kb"
					}
				}
			},
			new Project
			{
				Id = 3,
				School = new School {Id = 103, Name = "Bolton Spa Primary", URN = "AS_112456", LocalAuthority = "Bolton"},
				Trust = new Trust {Id = 203, Name = "Oak Hill Trust"},
				ApplicationReceivedDate = new DateTime(2021, 3, 1),
				AssignedDate = new DateTime(2021, 3, 3),
				ProjectDocuments = new List<DocumentDetails>()
			},
			new Project
			{
				Id = 4,
				School = new School {Id = 104, Name = "Ancoats Free School", URN = "AS_102045", LocalAuthority = "Greater Manchester"},
				Trust = new Trust {Id = 204, Name = "United Learning Trust"},
				ProjectDocuments = new List<DocumentDetails>()
			},
		};
			
		public IEnumerable<Project> GetAllProjects() => _projects;

		public Project GetProjectById(int id) => _projects[id] ?? throw new Exception();
	}
}