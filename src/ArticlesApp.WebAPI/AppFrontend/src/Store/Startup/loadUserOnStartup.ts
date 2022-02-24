import { loadUser as loadUserFromLocalStorage } from "redux-oidc";
import { User } from "oidc-client";
// import { clearStaleOidcTokens } from "src/IdentityService/_Helpers/clearStaleOidcTokens";
import { userManager } from "src/IdentityService/UserManager";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import { getUserIsSignedIn } from "src/Network/UserHttpClient";
import { InitializeHub as Initialize_ModeratorArticlesHub } from "../../Network/SignalR/ModeratorArticlesHub";
import { InitializeHub as Initialize_MyArticlesHub } from "../../Network/SignalR/MyArticlesHub";
import { InitializeHub as Initialize_NotificationsHub } from "../../Network/SignalR/NotificationsHub";
import {
  fetchAllMyArticles,
  moderatorFetchesAllOwnDecisions,
  moderatorFetchesAllPendingArticles
} from "../reducers/articlesSlice";
import { fetchAllNotifications } from "../reducers/notificationsSlice";
import { store } from "../store";
import { parseUserRoles } from "./parseUserRoles";



export function loadUserOnStartup(_store: typeof store) {
  // clearStaleOidcTokens();
  userManager.clearStaleState();

  userManager.events.addUserLoaded((user: User | null) => {
    checkSigninDivergence(user);

    if (user === null || user === undefined) {
      return;
    }

    _store.dispatch(fetchAllNotifications() as any);

    Initialize_NotificationsHub(user);
    Initialize_MyArticlesHub(user);

    const userRoles: UserRolesEnum[] = parseUserRoles(user?.profile?.role);

    if (userRoles.includes(UserRolesEnum.User)) {
      _store.dispatch(fetchAllMyArticles() as any);
    }

    if (userRoles.includes(UserRolesEnum.Moderator)) {
      _store.dispatch(moderatorFetchesAllPendingArticles() as any);
      _store.dispatch(moderatorFetchesAllOwnDecisions() as any);

      Initialize_ModeratorArticlesHub(user);
    }
  });

  userManager.events.addUserUnloaded(() => {
    userManager.clearStaleState();
    // clearStaleOidcTokens();
  })

  const route = window.location.pathname;

  if (
    route !== "/authentication/login-callback"
    && route !== "/authentication/logout-callback"
    && route !== "/silent-refresh"
  ) {
    loadUserFromLocalStorage(_store, userManager)
      .then((user: User | null) => {

        if (user === null || undefined) {
          userManager.clearStaleState();
          // clearStaleOidcTokens();

          checkSigninDivergence(user);

          return;
        }

        userManager.events.load(user!);
      });
  }
}



async function checkSigninDivergence(user: User | null) {
  const isSignedInObj_AppBackend = await getUserIsSignedIn();

  const isSignedIn_AppFrontend: boolean = !!user;

  if (
    isSignedInObj_AppBackend.isSignedIn === true
    && isSignedInObj_AppBackend.isSoftDeleted === false
    && isSignedIn_AppFrontend === false
  ) {
    userManager.signinRedirect({
      state: {
        prevPage: window.location.pathname
      }
    });

    return;
  }

  if (
    isSignedInObj_AppBackend.isSignedIn === true
    && isSignedInObj_AppBackend.isSoftDeleted === true
    && isSignedIn_AppFrontend === true
  ) {
    userManager.signoutRedirect({
      state: {
        prevPage: window.location.pathname
      }
    });

    return;
  }

  if (
    isSignedInObj_AppBackend.isSignedIn === false
    && isSignedIn_AppFrontend === true
  ) {
    userManager.signoutRedirect({
      state: {
        prevPage: window.location.pathname
      }
    });

    return;
  }
}