import { reducer as oidcReducer } from "redux-oidc";
import { combineReducers } from "@reduxjs/toolkit";
import { articlesSlice } from "./reducers/articlesSlice";
import { connectedRouterReducer } from "./reducers/connectedRouterReducer";
import { notificationsSlice } from "./reducers/notificationsSlice";
import { uiSlice } from "./reducers/uiSlice";



export const rootReducer = combineReducers({
  router: connectedRouterReducer,
  oidc: oidcReducer,
  articles: articlesSlice.reducer,
  notifications: notificationsSlice.reducer,
  ui: uiSlice.reducer
});