resource cloudfoundry_app worker_app {
  name               = local.web_app_name
  space              = data.cloudfoundry_space.space.id
	docker_image			 = local.docker_image
	strategy           = "blue-green-v2"

  service_binding { 
		service_instance = cloudfoundry_service_instance.redis.id 
	}

  service_binding {
    service_instance = cloudfoundry_user_provided_service.logit.id
  }

	routes {
		route = cloudfoundry_route.web_app_cloudapp_digital_route.id
	} 

	environment = {
		"ASPNETCORE_ENVIRONMENT"   = var.aspnetcore_environment
		"ASPNETCORE_URLS"          = "http://+:8080"
		"TramsApi__Endpoint"       = var.app_trams_api_endpoint
		"TramsApi__ApiKey"         = var.app_trams_api_key
		"Authentication__Username" = var.app_username
		"Authentication__Password" = var.app_password
		"GoogleAnalytics__Enable"  = var.enable_google_analytics
		"FeedbackLink"             = var.app_feedback_link
		"SupportEmail"             = var.app_support_email
		"SENTRY_RELEASE"           = "a2b-internal:${var.cf_app_image_tag}"
		"AZUREAD__CLIENTSECRET"    = var.app_azuread_clientsecret
	}
}

resource cloudfoundry_route web_app_cloudapp_digital_route {
  domain   = data.cloudfoundry_domain.london_cloud_apps_digital.id
  space    = data.cloudfoundry_space.space.id
  hostname = local.web_app_name
}

resource cloudfoundry_service_instance redis {
  name         = local.redis_service_name
  space        = data.cloudfoundry_space.space.id
  service_plan = data.cloudfoundry_service.redis.service_plans[var.cf_redis_service_plan]
}

resource cloudfoundry_user_provided_service logit {
	name             = local.logit_service_name
	space            = data.cloudfoundry_space.space.id
	syslog_drain_url = var.logit_sink_url
}
