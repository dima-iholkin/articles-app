import { useDispatch } from "react-redux";
import logger from "redux-logger";
import { configureStore } from "@reduxjs/toolkit";
import { routerMiddleware } from "connected-react-router";
import { history } from "./reducers/connectedRouterReducer";
import { rootReducer } from "./rootReducer";



export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      thunk: true,
      immutableCheck: true,
      serializableCheck: false
    })
      .concat(
        routerMiddleware(history),
        logger
      )
});
export type storeType = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>();