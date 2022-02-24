import { createUserManager } from "redux-oidc";
import {
  UserManagerSettings,
  WebStorageStateStore
} from "oidc-client";
import { ApplicationName } from './ApiAuthorizationConstants';



const userManagerConfig: UserManagerSettings = {
  authority: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}`,
  client_id: 'ArticlesApp.WebAPI',
  redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/authentication/login-callback`,
  post_logout_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/authentication/logout-callback`,
  response_type: 'code',
  scope: 'ArticlesApp.WebAPIAPI openid profile roles offline_access',
  silent_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/silent-refresh`,
  automaticSilentRenew: true,
  includeIdTokenInSilentRenew: true,
  accessTokenExpiringNotificationTime: 60,
  filterProtocolClaims: true,
  loadUserInfo: true,
  userStore: new WebStorageStateStore({
    prefix: ApplicationName
  })
}

export let userManager = createUserManager(userManagerConfig);