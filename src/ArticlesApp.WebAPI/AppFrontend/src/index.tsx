// Logger must be imported first, so that it's executed (imported) before other modules.
// eslint-disable-next-line
import { logError, startLogging } from "src/LogsService/Logger";
import ReactDOM from 'react-dom';
import { Provider as ReduxProvider } from "react-redux";
import { BrowserRouter as RouterProvider } from 'react-router-dom';
import { OidcProvider } from "redux-oidc";
import { ConnectedRouter as ConnectedRouterProvider } from "connected-react-router";
import CssReset from "@material-ui/core/CssBaseline";
import { styled } from '@material-ui/core/styles';
import { App } from 'src/App';
import { userManager } from 'src/IdentityService/UserManager';
import { history } from "src/Store/reducers/connectedRouterReducer";
import { store } from "src/Store/store";
import { storeStartup } from "src/Store/storeStartup";



try {
  startLogging();

  storeStartup(store);

  const AppContainer = styled("div")((props) => ({
    textAlign: "center",
    backgroundColor: "whitesmoke",
    display: "flex",
    flexDirection: "column",
    '@media (min-width: 60ch)': {
      width: "60ch"
    },
    width: "100%",
    margin: "auto",
    position: "relative",
    flexGrow: 1
  }));

  const baseUrl = document
    .getElementsByTagName('base')[0]
    .getAttribute('href');

  ReactDOM.render(
    <RouterProvider basename={baseUrl ?? "/"}>
      <ReduxProvider store={store}>
        <ConnectedRouterProvider history={history}>
          <OidcProvider store={store} userManager={userManager}>
            <CssReset>
              <AppContainer>
                <App />
              </AppContainer>
            </CssReset>
          </OidcProvider>
        </ConnectedRouterProvider>
      </ReduxProvider>
    </RouterProvider>,
    document.getElementById('root')
  )
} catch (error: any) {
  logError(
    error.message,
    error.stack
  );

  throw error;
}



//import registerServiceWorker from './registerServiceWorker';

// Uncomment the line above that imports the registerServiceWorker function
// and the line below to register the generated service worker.
// By default create-react-app includes a service worker to improve the
// performance of the application by caching static assets. This service
// worker can interfere with the Identity UI, so it is
// disabled by default when Identity is being used.
//
//registerServiceWorker();