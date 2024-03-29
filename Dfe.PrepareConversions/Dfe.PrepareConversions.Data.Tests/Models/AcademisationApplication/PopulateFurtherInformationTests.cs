﻿using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using System;
using Xunit;
using static Dfe.PrepareConversions.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication;

public class PopulateFurtherInformationTests
{
   [Fact]
   public void PopulateFurtherInformation_ShouldPopulateDataCorrectly()
   {
      // Arrange
      School academisationApplicationSchool = new()
      {
         TrustBenefitDetails = "Trust Benefit",
         OfstedInspectionDetails = "Ofsted Inspection",
         Safeguarding = true,
         //SafeguardingDetails = "Safeguarding",
         LocalAuthorityReorganisationDetails = "LA Reorganization",
         LocalAuthorityClosurePlanDetails = "LA Closure",
         DioceseName = "Diocese",
         PartOfFederation = true,
         FoundationTrustOrBodyName = "Foundation",
         ExemptionEndDate = DateTime.Today,
         MainFeederSchools = "Feeder Schools",
         ProtectedCharacteristics = "unlikely",
         FurtherInformation = "Additional Information"
      };
      ApplyingSchool academiesApplicationSchool = new();
      // Act
      PopulateFurtherInformation(academiesApplicationSchool, academisationApplicationSchool);
      // Assert
      Assert.Equal("Trust Benefit", academiesApplicationSchool.SchoolAdSchoolContributionToTrust);
      Assert.True(academiesApplicationSchool.SchoolAdInspectedButReportNotPublished);
      Assert.Equal("Ofsted Inspection", academiesApplicationSchool.SchoolAdInspectedButReportNotPublishedExplain);
      Assert.True(academiesApplicationSchool.SchoolOngoingSafeguardingInvestigations);
      //Assert.Equal("Safeguarding", academiesApplicationSchool.SchoolOngoingSafeguardingDetails);
      Assert.True(academiesApplicationSchool.SchoolPartOfLaReorganizationPlan);
      Assert.Equal("LA Reorganization", academiesApplicationSchool.SchoolLaReorganizationDetails);
      Assert.True(academiesApplicationSchool.SchoolPartOfLaClosurePlan);
      Assert.Equal("LA Closure", academiesApplicationSchool.SchoolLaClosurePlanDetails);
      Assert.True(academiesApplicationSchool.SchoolFaithSchool);
      Assert.Equal("Diocese", academiesApplicationSchool.SchoolFaithSchoolDioceseName);
      Assert.True(academiesApplicationSchool.SchoolIsPartOfFederation);
      Assert.True(academiesApplicationSchool.SchoolIsSupportedByFoundation);
      Assert.Equal("Foundation", academiesApplicationSchool.SchoolSupportedFoundationBodyName);
      Assert.True(academiesApplicationSchool.SchoolHasSACREException);
      Assert.Equal(DateTime.Today, academiesApplicationSchool.SchoolSACREExemptionEndDate);
      Assert.Equal("Feeder Schools", academiesApplicationSchool.SchoolAdFeederSchools);
      Assert.True(academiesApplicationSchool.SchoolAdEqualitiesImpactAssessmentCompleted);
      Assert.Equal("That the Secretary of State's decision is unlikely to disproportionately affect any particular person or group who share protected characteristics",
         academiesApplicationSchool.SchoolAdEqualitiesImpactAssessmentDetails);
      Assert.True(academiesApplicationSchool.SchoolAdditionalInformationAdded);
      Assert.Equal("Additional Information", academiesApplicationSchool.SchoolAdditionalInformation);
   }
}
