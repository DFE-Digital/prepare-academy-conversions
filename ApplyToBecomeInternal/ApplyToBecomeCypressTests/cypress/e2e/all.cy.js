/// <reference types ='Cypress'/>

// Run all tests NOTE: this is a work around as cypress 10.0 GUI does not allow to run all tests


// 84457_TL_school-and-trust-information-proj-dates
import '../e2e/84457_TL_school-and-trust-information-proj-dates/84596_error-handling_incorrect-url-tests.cy'
import '../e2e/84457_TL_school-and-trust-information-proj-dates/86296_error-handling_checkmark.cy'
import '../e2e/84457_TL_school-and-trust-information-proj-dates/86342_error-handling_error-routes.cy'

// 84468_TL_general-Information
import '../e2e/84468_TL_general-Information/86316_happy-path_mp-details.cy'
import '../e2e/84468_TL_general-Information/86858_modify-viability-field.cy'
import '../e2e/84468_TL_general-Information/86859_modify-financial-deficit.cy'
import '../e2e/84468_TL_general-Information/92796_modify_distance-from-school.cy'


// 86338_error-404-checks_url-routing
import './86338_error-404-checks_url-routing/86339_advisory-board-urls.cy'

// 86340_TL-record-dates-LA-information
import './86340_TL-record-dates-LA-information/86462_verify-date-sent-return.cy'
import './86340_TL-record-dates-LA-information/86856_comments-updated-correctly.cy'
import './86340_TL-record-dates-LA-information/87641_checkmark-application-status.cy'

// 86826_TL_project-notes
import './86826_TL_project-notes/86317_happy-path_project-notes.cy'

// 86831_TL_cookies
import './86831_TL_cookies/86314_happy-path_cookie-consent-preferences.cy'

// 86862_TL_generate-project-template
import './86862_TL_generate-project-template/86341_error-handling_advisory-board-date-errors.cy'

// 91521_SCAPP_apply-to-become
import './91521_SCAPP_apply-to-become/91489_Apply-to-become-get-application types.cy'
import './91521_SCAPP_apply-to-become/91489_Apply-to-become-get-application-data-map.cy'
import './91521_SCAPP_apply-to-become/98085_integration-tests.cy'

// 94464_landing-page
import './94464_landing-page/106119_landing-page.cy'

// 96066_TL_legal-requirements
import './96066_TL_legal-requirements/105392_legal-requirements.cy'

// 98961_home-pagination
import './98961_home-pagination/101092_home-page-pagination.cy'

// Record Decision
import './Record_Decision/103195_create-approved-decision.cy'
import './Record_Decision/103195_new-plus-edit-approved-decision.cy'
import './Record_Decision/103787_error-handling.cy'
import './Record_Decision/103788_create-deferred-decision.cy'
import './Record_Decision/103788_new-plus-edit-deferred-decision.cy'
import './Record_Decision/103791_create-declined-decision.cy'
import './Record_Decision/103791_new-plus-edit_declined-decision.cy'
