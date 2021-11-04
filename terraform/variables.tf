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

variable cf_redis_service_plan {
  type			= string
  description 	= "Cloud Foundry redis service plan"
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

variable app_username {
	type = string
	description = "Application variable for the username for the service"
}

variable app_password {
	type = string
	description = "Application variable for the password for the service"
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

## ========================================================================== ##
#  Locals					                                                   #
## ========================================================================== ##
locals {
  app_name_suffix      = var.app_environment
  web_app_name         = var.app_environment != "production" ? "apply-to-become-an-academy-internal-${local.app_name_suffix}" : "apply-to-become-an-academy-internal"
  web_app_routes       = cloudfoundry_route.web_app_cloudapp_digital_route
  redis_service_name   = "apply-to-become-an-academy-internal-redis-${local.app_name_suffix}"
  logit_service_name   = "academy-transfers-logit-sink-${local.app_name_suffix}"
	docker_image         = "ghcr.io/dfe-digital/a2b-internal:${var.cf_app_image_tag}"
}