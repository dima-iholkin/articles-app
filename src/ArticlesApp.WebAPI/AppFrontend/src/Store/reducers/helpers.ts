import { PayloadAction } from "@reduxjs/toolkit";
import { HttpErrorsEnum } from "src/Network/HttpErrorsEnum";
import { SubmitShape } from "./interfaces";



export function fetchEntities_Started(state: any, stateRoute: string, action: PayloadAction<void>){
  state[stateRoute] = {
    ...state[stateRoute],
    isLoading: true,
    httpError: undefined
  }
}

export function fetchEntities_Success<T>(state: any, stateRoute: string, action: PayloadAction<T[]>) {
  state[stateRoute] = {
    entities: action.payload,
    isLoading: false,
    httpError: undefined
  }
}

export function fetchEntities_Failure(state: any, stateRoute: string, action: PayloadAction<HttpErrorsEnum>) {
  state[stateRoute] = {
    ...state[stateRoute],
    isLoading: false,
    httpError: action.payload
  }
}



export function submitEntity_Started(stateSubmit: SubmitShape): SubmitShape {
  return {
    inProgress: true,
    httpError: undefined
  };
}

export function submitEntity_Success(stateSubmit: SubmitShape): SubmitShape {
  return {
    inProgress: false,
    httpError: undefined
  };
}

export function submitEntity_Failure(stateSubmit: SubmitShape, action: PayloadAction<HttpErrorsEnum>): SubmitShape {
  return {
    inProgress: false,
    httpError: action.payload
  };
}