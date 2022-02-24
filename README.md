# Articles app

This is a simple app, where people can publish and view the public articles or messages published by others.  
It's my sandbox for testing the web development technologies.

## Deployment

**Warning:**
This app isn't ready for a global Internet deployment. But it can be deployed on a local network, if you trust your local network. I mean to deploy on the local network with no access from the global Internet.

For the deployment instructions go into the folder `/_docs/ProductionLocal/`.

### Deployment requirements

**Required software:**

* Windows OS (hopefully will add Linux support later)
* SQL Server instance or Visual Studio's LocalDB
* MSBuild
* OpenSSL to generate an HTTPS cert and an IdentityServer4 key
* GitHub account for third-party login provider

**Optional software:**

* ElasticSearch instance for logging and analytics
* Prometheus instance for metrics

## Technical details

The back-end is built with `ASP.NET Core`, the front-end is `React + TypeScript`.  
`IdentityServer4` is used for user authenication and authorization with JWT.  
The main and only database is `SQL Server`.  
`ElasticSearch` is used to store logs and analytics.  
Metrics are gathered into `Prometheus`.  
For now there is no proper HTTP server, or reverse proxy, in front of the Kestrel server. Which is a security risk on the global Internet.
