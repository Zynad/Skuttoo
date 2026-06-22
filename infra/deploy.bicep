// Deploys the Skuttoo web app as a SECOND web app onto an EXISTING (shared) Linux App Service
// plan, to keep marginal cost ~0. No database resource (SQLite file on /home), no container
// registry. Secrets (e.g. Speech key) are NOT set here — add them as App Service settings
// out-of-band so nothing sensitive lives in this public repo.

@description('Base name for the app.')
param appName string = 'skuttoo'

@description('Environment short name, e.g. dev / prod.')
param environmentName string

@description('Name of the EXISTING App Service plan to deploy onto (reused to keep cost low).')
param existingPlanName string

@description('Resource group of the existing plan. Defaults to the deployment resource group.')
param existingPlanResourceGroup string = resourceGroup().name

@description('Always On. Requires Basic+ tier; set false for Free (F1) plans.')
param alwaysOn bool = true

param location string = resourceGroup().location

// Reference the already-provisioned shared plan (created elsewhere; not managed here).
resource plan 'Microsoft.Web/serverfarms@2023-12-01' existing = {
  name: existingPlanName
  scope: resourceGroup(existingPlanResourceGroup)
}

resource web 'Microsoft.Web/sites@2023-12-01' = {
  name: 'web-${appName}-${environmentName}'
  location: location
  kind: 'app,linux'
  properties: {
    httpsOnly: true
    serverFarmId: plan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      ftpsState: 'Disabled'
      http20Enabled: true
      minTlsVersion: '1.2'
      alwaysOn: alwaysOn
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          // SQLite on the persistent /home mount. The app creates the directory on startup.
          name: 'ConnectionStrings__Default'
          value: 'Data Source=/home/data/app.db'
        }
        // Secret settings (Speech__Key, Speech__Region) are configured out-of-band, never in source.
      ]
    }
  }
}

output webAppName string = web.name
output defaultHostName string = web.properties.defaultHostName
