# room :: terraform

## BACKENDS
terraform {
  backend "remote" {}
}

## PROVIDERS
provider "azuread" {
  version = "~>0.4.0"
}

provider "azurerm" {
  version = "~>1.30.0"
}

provider "cloudflare" {
  version = "~>1.16.0"
}

## RESOURCES
resource "azurerm_app_service" "roomxyz" {
  app_service_plan_id = "${azurerm_app_service_plan.roomxyz.id}"
  https_only = "${var.app_service["https"]}"
  location = "${azurerm_resource_group.roomxyz.location}"
  name = "${var.app_service["name"]}"
  resource_group_name = "${azurerm_resource_group.roomxyz.name}"

  app_settings = {
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
  }

  site_config {
    app_command_line = ""
    linux_fx_version = "COMPOSE|${filebase64(var.app_service["linux"])}"
  }
}

resource "azurerm_app_service_plan" "roomxyz" {
  kind = "${var.app_service_plan["kind"]}"
  location = "${azurerm_resource_group.roomxyz.location}"
  name = "${var.app_service_plan["name"]}"
  resource_group_name = "${azurerm_resource_group.roomxyz.name}"
  reserved = "${var.app_service_plan["reserved"]}"

  sku {
    size = "${var.app_service_plan["size"]}"
    tier = "${var.app_service_plan["tier"]}"
  }
}

resource "azurerm_resource_group" "roomxyz" {
  name = "${var.resource_group["name"]}"
  location = "${var.resource_group["location"]}"

  tags = {
    owner = "${var.resource_group["owner"]}"
  }
}
