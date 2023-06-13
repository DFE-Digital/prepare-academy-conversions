## NEXT
* The term 'Involuntary Conversion' has been replaced with 'Sponsored Conversion'. This includes all Api endpoints. Note this requires a new release of the academies api with corresponding changes.
* User Story 120665 : Updated PreviewProjectTemplate to hide `Legal requirements` and `rationale-for-project` when project is an involuntary conversion
* User Story 129594 : The Local Authority and Region for a school is now passed up to the Academisation Api during involuntary project creation. This avoids the delay in populating the two values within the Academisation Api as they're already known during the conversion process.
* User Story 129560 : Hides the row for Local Authority on the project list if the value is null.

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