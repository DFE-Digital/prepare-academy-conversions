## NEXT

* Added ability to filter filters (Example being; to refine delivery officer list)
## 1.5.0

* Changed Feedback Banner: The feedback banner has been updated to better capture user insights and improve engagement.
* Accessibility Adjustments: Removed phone numbers from the Accessibility section.
* Updated Project Filters: Filters on Conversions now include Ministry of Justice component (MoJ) and Trust search.
* Content Replacement in Trust Template: Replaced all content in the trust template with new guidance, removing references to KIM.
* Bug Fixes:
    * Resolved a bug with School Search.
    * Fixed a pagination issue.
    * Addressed a bug with clearing filters, ensuring filters reset correctly for new searches.
* Added New Start a Conversion Journeys:
    * Enabled creation of Special Educational Needs (SEN) Projects.
    * Enabled creation of Pupil Referral Unit (PRU) projects.
    * Allowed any Conversion project to be initiated without an application form, broadening the scope for project initiation.
* New Task Lists:
    * Introduced a new task list specifically for PRU schools.
    * Introduced a new task list for SEN schools, tailored to the unique requirements of SEN conversions.
* Grant Amount Logic: Implemented logic for calculating grant amounts for SEN and PRU conversions.
* New Tab for Form a MAT Projects: Added a dedicated tab for Form a Multi-Academy Trust (MAT) projects, improving organization and accessibility of these projects.
* Data Export Functionality: Facilitated the export of data into spreadsheets for reporting purposes.
* Project Enhancements:
    * Made Read Only Form a MAT projects editable using the voluntary tasklist.
    * Projects showing in the new Form a MAT tab for better visibility and organization.
    * Grouped Form a MAT projects within the Application by the proposed trust.
    * Displayed conversion type on the Project.
* Record Decision Enhancements:
    * Added a Withdrawn Status to accurately record a decision when a project is withdrawn.
    * Introduced AO (Advisory Officer) date to record decisions, enhancing decision tracking.
    * Added DAO (Designated Advisory Officer) and AO date to the Export spreadsheet.
* Ability to add Conversion projects within the application.
* SEN Specific additions to the school overview.

## 1.4.0

* Ability to add Conversion projects within the application
* SEN Specific additions to the school overview

## 1.3.0

* Sponsored Conversions inital MVP prepared
* Sponsored grants now take into account the different tracks and school phase
* User Story 136870 : hyperlink fixes 
* User Story 137325 : Remove signed by field
* User Story 137350 : Project details tab fixed
* User Story 137527 : Back button labelling updated for consistency
* Removed the 'UseAcademisation' feature flag and defaulted the behaviour to use the Academies Api
* User Story 132744 : Renaming of school information pages in task list.
* User Story 132316 : New 'Performance data' section added to task list.
* User Story 132317 : Renaming of LA info template sent date section and link in task list.
* User Story 132314 : New 'school information' section added to task list.
* User Story 132744 : Add School information - rename pages
* Fixed Bug 133919 : Styling on rationale page fixed to reflect other pages
* User Story 134597 : Added Route to top of task list for voluntary and sponsored journeys
* User Story 134936 : Updated wording of delivery officer to Project owner
* User Story 133831 : Added Application insights 
* User Story 135435 : Updated School Overview page hint text to be relevant to the route 
* User Story 135121 : Added Project Notes to the Form a MAT journey
* Updated the bottom of the preview document page to reflect changes to task list regarding 'Generate template'
* Moved reference data on the task list to it's own dedicated section
* 'Prepare your template' wording updated and moved up in the task list
* Updated Preview Document page to reflect the ordering in the task list and the hew headings


---

## 1.2.0
* Fixed Bug 132060 : The 'proposed academy opening date' field will be populated with the 'ProposedAcademyOpeningDate' value to match changes made in the academisation API. On creation of projects the proposed opening date will no longer be populated with a six month arbitrary date.  
* The term 'Involuntary Conversion' has been replaced with 'Sponsored Conversion'. This includes all Api endpoints. Note this requires a new release of the academies api with corresponding changes.
* User Story 120665 : Updated PreviewProjectTemplate to hide `Legal requirements` and `rationale-for-project` when project is an involuntary conversion
* User Story 129594 : The Local Authority and Region for a school is now passed up to the Academisation Api during involuntary project creation. This avoids the delay in populating the two values within the Academisation Api as they're already known during the conversion process.
* User Story 129560 : Hides the row for Local Authority on the project list if the value is null.
* Loans and leases on application form now split into discrete packages when multiple are presented
* Generated document for a given project has been refactored to be generated programatically. This enables a reduction in white-space, improves support for conditional elements based on Project ype (Voluntary/Sponsored etc) and allows for easier extension in the future.
* User Story 121797 : Create generate project template for an involuntary conversion.
* Fixed bug 132079 : Record Local Authority dates page flow is incorrect
* Fixed bug 132058 : Accessibility issues on New PFI Scheme Page


---
## 1.1.1
* Error messages relating to date now formatted with sentence case
* Error page tab title now displaying
* Dates now maintained on validation error - RecordLocalAuthorityInformationTemplateDates
* Telephone numbers for contact details on the school application form page have been removed inline with removing them from the data returned by the academisation API.
* Fixed bug 129478 : Conversions approved with conditions filter now displays as 'Approved with Conditions'
* Fixed bug 120010 : Navigation to links content is not working on 'Prepare you trust' page
* Fixed bug 126187 : Added Notes not showing correct time (Time now being set to UK time regardless of server)
* Irrelevant rationale for project field removed for the sponsored journey. (In favour of 'Rationale for the trust or Sponsor')
* Sponsored journey pages for changing or adding additional information have had sidebars updated with a link to upload their Annex B form (Excluding budget and legal section)
* Integration tests now utilising most recent feature flag configuration

___
## 1.1.0 
* Flexible Advisory Board Dates: Users can now set advisory board dates in either the future or the past for better planning and tracking.
* Project listing page now states Project route: Voluntary/Form a MAT and Involuntary (Involuntary is due to change to 'Sponsored' and won't appear in live until the feature is complete)
* Status tags on Key Stage performance data - Provisional, Revised and Final: This provides much needed context, allowing users to make any informed decisions with the knowledge of whether the data is still subject to change.
* New domains introduced:
   * https://www.prepare-conversions.education.gov.uk (Production)
   * https://dev.prepare-conversions.education.gov.uk/ (Development)
   * https://test.prepare-conversions.education.gov.uk/ (Test/Staging)
* Redirect now enabled for the Prepare Conversions application to begin the transition over to the above domain for users.
* Form a MAT Workflow:
   * Projects following the Form a MAT journey can now be viewed in the project listing.
   * Form a MAT applications can be accessed for each school within the Form a MAT applications perspective.
   * "Other schools in this MAT" can be viewed to see schools within the same Form a MAT application.
   * Key people and Form a MAT rationale can be viewed within the Form a MAT application.
* Updated several page titles for better user experience and navigation.
* Resolved an issue where project creation dates were displaying as 0001-01-01
* Project listing page now states Project route: Voluntary/Form a MAT and Involuntary (Involuntary is due to change to Sponsored and won't appear in live until the feature is complete)
* Status tags on Key Stage performance data
   * Provisional, Revised and Final: This provides much needed context, allowing users to make any informed decisions with the knowledge of whether the data is still subject to change.


---

# 1.0.0 - Initial release of Prepare Conversions
Initial Release of the Prepare-conversions system into production.