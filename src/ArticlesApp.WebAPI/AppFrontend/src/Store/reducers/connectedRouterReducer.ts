import { connectRouter } from "connected-react-router";
import { createBrowserHistory } from "history";
import { logRouteChange } from "src/AnalyticsService/AnalyticsLogger";



export const history = createBrowserHistory();

const route = window.location.pathname;
if (
  route !== "/"
  && route !== "/authentication/login-callback"
  && route !== "/authentication/logout-callback"
  && route !== "/silent-refresh"
) {
  let oldState = window.history.state;
  let oldUrl = window.location.pathname;

  history.replace("/", {});
  history.push(oldUrl, oldState);
  history.goForward();
}
// For when opening the app in a new tab with not "/" (Home) Url. See #516.

logRouteChange(history.location.pathname);
history.listen((location) => {
  logRouteChange(location.pathname);
});
// Log the route changes.

export const connectedRouterReducer = connectRouter(history);