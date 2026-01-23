# release notes 

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/). To see an example from a mature product in the program [see the Complete products changelog that follows the same methodology](https://github.com/DFE-Digital/dfe-complete-conversions-transfers-and-changes/blob/main/CHANGELOG.md).


## [Unreleased](https://github.com/DFE-Digital/prepare-academy-conversions/compare/production-2026-01-20.1023...main)
NOTE: remember to update unreleased link when making a new release

### Changed

- updated hidden text for "change PFI scheme" link on school overview


### Added

- 257437 - duplicated apply filters button to the bottom of conversion and transfer project list filters

## [1.26.0][1.26.0] - 2026-01-20

### Changed

- 255792 - update accessibility statement link

---

## [1.25.0][1.25.0] - 2026-01-16

### Added

- 235884 - OKR 1 - Pre-populate MP Name
- 251757 - OKR 1 - MP Political Party
- 236589 - OKR 1 - Pre-populate 'Viability issues' under 'School Overview'

### Changed

- update EAT api client to use latest version

---

## [1.24.0][1.24.0] - 2026-01-07

- rollback to 1.22.0 due to person api env variables missing in production

---

## [1.23.0][1.23.0] - 2026-01-07

### Added

- 235884 - OKR 1 - Pre-populate MP Name

--- 

## [1.22.0][1.22.0] - 2026-01-06

### Added

- 237053 - 'Approve with conditions' with recommendation notes under 'Conversion details'

---

## [1.21.0][1.21.0] - 2025-12-09

### Added

- 236761 - prepopulate the field 'Is the school linked to a diocese'
- 236756 - Prepopulate the field 'Financial deficit' under 'School Overview'

---

## [1.20.0][1.20.0] - 2025-12-01

### Fixed

- 209924 - BUG accessibility - Missing Labels And Broken ARIA refs
- 213008 - BUG accessibility - cant create conversion when js turned off

---

## [1.19.0][1.19.0] - 2025-11-28

### Changed
- Updated EAT API client Auth scope

---

## [1.18.0][1.18.0] - 2025-11-28

### Changed
- update EAT client setting
- EAT client error handling

---

## [1.17.0][1.17.0] - 2025-11-05

### Added
- docker for local development

### Security
- 240987 - xss vulnerability

---

## [1.16.0][1.16.0] - 2025-10-22

### Fixed
- 209885 - BUG accessibility aria labels

---

## [1.15.0][1.15.0] - 2025-10-22

### Changed
- use new govuk design

---

## [1.14.0][1.14.0] - 2025-09-25

### Fixed
- 235754 - Performance data references

---

## [1.13.0][1.13.0] - 2025-09-23

- update release notes

---

## [1.12.0][1.12.0] - 2025-09-23

### Changed
- Transfer of Projects into Complete AB date future date

### Fixed
- 235754 - Performance data references TRAMS incorrectly - Two other instances
- 233570 - Transfer of Projects into Complete - Return url missing
- 226293 - Performance data references TRAMS not Ofsted

---

## [1.11.0][1.11.0] - 2025-08-27

### Changed
- Advisory board date rename to Proposed decision date #1379

---

## [1.10.0][1.10.0] - 2025-07-03

### Deprecated
- 1899562 - Remove conversion support grant task
- 210853 - Conversion Support Grant Removal - School Application Form

### Fixed
- 219657 - SAT-Transfer/FAM-Transfer Minor: Visual: Inset Text Looks Quirky
- Hide Voluntary Conversion Support Grant when post deadline

---

## [1.9.0][1.9.0] - 2025-05-15

### Fixed
- 212811 - Change links on CheckYourAnswers page for Transfers project are not working

---

## [1.8.0][1.8.0] - 2025-05-07

### Fixed
- 211057 - Advisory board date page discrepancy on Conversion and Transfer project

### Changed
- Publish Artifact Attestation for Docker images
 
---

## [1.7.0][1.7.0] - 2025-04-28

### Added
* 200233 - Add PSED task to Prepare
* 200234 - Add Equalities Duty section to pre-populated Project Template Word Document
* 201243 - Add PSED section to 'Preview project document' screen


## 1.6.0

* Added ability to filter filters (Example being; to refine delivery officer list)
* Added ability to have 'DAO Revoked' decision recorded against a Sponsored conversion project
* Decision flow changed to ensure consistency with Complete
* Content updated on decision flow to be more concise.
* Introduction of New Decision for DAO Revoked:
    * Introduces precursor to explain the process
    * Adds a 'Before You Start' page to ensure user readiness and confirmation of prerequisites
    * Enforces decision was made by Minister 


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

[1.26.0]: 
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2026-01-20.1023
[1.25.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2026-01-16.1018
[1.24.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2026-01-07.1010
[1.23.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2026-01-07.1009
[1.22.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2026-01-06.1001
[1.21.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-12-09.990
[1.20.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-12-01.979
[1.19.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-11-28.973
[1.18.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/edit/production-2025-11-28.969
[1.17.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-11-05.955
[1.16.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-10-22.945
[1.15.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/edit/production-2025-10-22.938
[1.14.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-09-25.923
[1.13.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-09-23.916
[1.12.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-09-23.913
[1.11.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-08-27.893
[1.10.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-07-03.872
[1.9.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-05-15.855
[1.8.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-05-07.846
[1.7.0]:
   https://github.com/DFE-Digital/prepare-academy-conversions/releases/tag/production-2025-04-28.837
   