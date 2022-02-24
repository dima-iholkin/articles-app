import { Article } from "src/Entities/Article";
import { Notification } from "src/Entities/Notification";
import { HttpErrorsEnum } from "src/Network/HttpErrorsEnum";



export interface EntitiesArrayShape<T> {
  entities: T[],
  isLoading: boolean,
  httpError?: HttpErrorsEnum
}

function createInitialState<T>(): EntitiesArrayShape<T> {
  return {
    entities: [] as T[],
    isLoading: false,
    httpError: undefined
  }
}



export interface ArticlesArrayShape extends EntitiesArrayShape<Article> { }

export const createInitialArticlesState =
  () => createInitialState<Article>();



export interface NotificationsArrayShape extends EntitiesArrayShape<Notification> { }

export const createInitialNotificationsState =
  () => createInitialState<Notification>();



export interface SubmitShape {
  inProgress: boolean,
  httpError?: HttpErrorsEnum
}

export const createInitialSubmitState =
  (): SubmitShape => ({
    inProgress: false,
    httpError: undefined
  })