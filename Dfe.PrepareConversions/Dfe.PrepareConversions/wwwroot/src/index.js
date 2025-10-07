// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
import * as GOVUKFrontend from "govuk-frontend";
import * as MOJFrontend from "@ministryofjustice/frontend";

window.GOVUKFrontend = GOVUKFrontend;
window.MOJFrontend = MOJFrontend;

// Initialize everything when DOM is ready
document.addEventListener('DOMContentLoaded', function () {

   // Initialize GOV.UK Frontend
   GOVUKFrontend.initAll();

   // Initialize MOJ Frontend
   MOJFrontend.initAll();
});