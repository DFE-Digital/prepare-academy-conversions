﻿using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents
{
	public class GeneralInformationViewComponent : ViewComponent
	{
		private readonly GeneralInformationService _generalInformationService;
		private readonly IAcademyConversionProjectRepository _repository;

		public GeneralInformationViewComponent(GeneralInformationService generalInformationService, IAcademyConversionProjectRepository repository)
		{
			_generalInformationService = generalInformationService;
			_repository = repository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var id = int.Parse(ViewContext.RouteData.Values["id"].ToString());

			var response = await _repository.GetProjectById(id);
			if (!response.Success)
			{
				throw new InvalidOperationException();
			}

			var project = response.Body;
			var generalInformation = await _generalInformationService.GetGeneralInformationByUrn(project.Urn.ToString());

			var viewModel = new GeneralInformationViewModel
			{
				Id = project.Id.ToString(),
				SchoolPhase = generalInformation.SchoolPhase,
				AgeRange = !string.IsNullOrEmpty(generalInformation.AgeRangeLower) && !string.IsNullOrEmpty(generalInformation.AgeRangeUpper)
					? $"{generalInformation.AgeRangeLower} to {generalInformation.AgeRangeUpper}"
					: "",
				SchoolType = generalInformation.SchoolType,
				NumberOnRoll = generalInformation.NumberOnRoll?.ToString(),
				PercentageSchoolFull = generalInformation.NumberOnRoll.AsPercentageOf(generalInformation.SchoolCapacity),
				SchoolCapacity = generalInformation.SchoolCapacity?.ToString(),
				PublishedAdmissionNumber = project.PublishedAdmissionNumber,
				PercentageFreeSchoolMeals = !string.IsNullOrEmpty(generalInformation.PercentageFreeSchoolMeals) ? $"{generalInformation.PercentageFreeSchoolMeals}%" : "",
				PartOfPfiScheme = project.PartOfPfiScheme,
				ViabilityIssues = project.ViabilityIssues,
				FinancialDeficit = project.FinancialDeficit,
				IsSchoolLinkedToADiocese = generalInformation.IsSchoolLinkedToADiocese,
				DistanceFromSchoolToTrustHeadquarters = ViewData["Return"] == null ?
					project.DistanceFromSchoolToTrustHeadquarters.ToSafeString() : $"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()}",
				DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
				ParliamentaryConstituency = generalInformation.ParliamentaryConstituency,
				MemberOfParliamentName = project.MemberOfParliamentName,
				MemberOfParliamentParty = project.MemberOfParliamentParty
			};

			return View(viewModel);
		}
	}
}