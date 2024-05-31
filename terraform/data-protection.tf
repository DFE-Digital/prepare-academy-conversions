module "data_protection" {
  source = "github.com/DFE-Digital/terraform-azurerm-aspnet-data-protection?ref=v1.0.1"

  data_protection_key_vault_assign_role   = false
  data_protection_key_vault_subnet_prefix = "172.16.100.0/28"
  data_protection_key_vault_access_ipv4   = local.key_vault_access_ipv4
  data_protection_resource_prefix         = "${local.environment}${local.project_name}"
  data_protection_azure_location          = local.azure_location
  data_protection_tags                    = local.tags
  data_protection_resource_group_name     = module.azure_container_apps_hosting.azurerm_resource_group_default.name
}
