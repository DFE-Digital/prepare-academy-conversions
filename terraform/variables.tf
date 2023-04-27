## ========================================================================== ##
#  PaaS						                                                   #
## ========================================================================== ##
variable cf_api_url {
  type			= string
  description 	= "Cloud Foundry api url"
}

variable cf_user {
  type			= string
  description 	= "Cloud Foundry user"
}

variable cf_password {
  type			= string
  description 	= "Cloud Foundry password"
}

variable cf_space {
  type			= string
  description 	= "Cloud Foundry space"
}

variable cf_app_image_tag {
	type        = string
	description = "The tag to use for the docker image"
}

## ========================================================================== ##
#  Environment				                                                   #
## ========================================================================== ##
variable app_environment {
  type			= string
  description 	= "Application environment development, staging, production"
}

variable app_trams_api_endpoint {
	type = string
	description = "Application variable for the TRAMS API URL"
}

variable app_trams_api_key {
	type = string
	description = "Application variable for the TRAMS API Key"
}

variable app_academisation_api_baseurl {
	type = string
	description = "Application variable for the Academisation API url"
}

variable app_academisation_api_apikey {
	type = string
	description = "Application variable for the Academisation API key"
}

variable app_servicelink_transfersurl {
	type = string
	description = "Application variable for the Transfers service URL"
}

variable app_azuread_clientsecret {
  type = string
  description = "Application variable for the Azure AD Authorization header"
}

variable app_azuread_clientid {
  type = string
  description = "Application variable for the Azure AD client id"
}

variable app_azuread_tenantid {
  type = string
  description = "Application variable for the Azure AD tenant id"
}

variable app_azuread_groupid {
  type = string
  description = "Application variable for the Azure AD group id"
}

variable app_cypresstest_secret {
  type = string
  description = "Application variable for the Cypress tests secret"
}

variable aspnetcore_environment {
  type = string
  description = ".NET Core Environment to run the service in"
}

variable enable_google_analytics {
  type = string
  description = "Whether or not to enable google analytics"
}

variable app_feedback_link {
	type = string
	description = "Link in phase banner"
}

variable logit_sink_url {
	type = string
	description = "Target URL (HTTPS) for logs to be streamed to"
}

variable app_support_email {
	type = string
	description = "email address for support"
}

variable app_feature_use_academisation {
   type = bool
   default = false
   description = "Whether to use the new Academisation API (true) or default to the old Academies API (false)"
}
variable app_feature_use_academisation_application {
   type = bool
   default = false
   description = "Whether to use the new Academisation API (true) or default to the old Academies API (false) for School Application forms"
}

variable app_feature_show_directedacademyorders {
   type = bool
   default = false
   description = "Whether to show the directed academy orders button"
}

variable app_accessrequest {
   type = string
   description = "URL for form used to request access to the service"
}

variable app_templatefeedback {
   type = string
   description = "Feedback form URL used on the template download page"
}

## ========================================================================== ##
#  Locals					                                                   #
## ========================================================================== ##
locals {
  app_name_suffix      = var.app_environment
  web_app_name         = var.app_environment != "production" ? "apply-to-become-an-academy-internal-${local.app_name_suffix}" : "apply-to-become-an-academy-internal"
  web_app_routes       = cloudfoundry_route.web_app_cloudapp_digital_route
  logit_service_name   = "academy-transfers-logit-sink-${local.app_name_suffix}"
	docker_image         = "ghcr.io/dfe-digital/a2b-internal:${var.cf_app_image_tag}"
}
