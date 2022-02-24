import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Notification } from "src/Entities/Notification";
import { HttpErrorsEnum } from "src/Network/HttpErrorsEnum";
import { getAllNotificationsForUser, markNotificationAsRead as markNotificationAsReadHttp } from "src/Network/NotificationsHttpClient";
import {
  fetchEntities_Failure,
  fetchEntities_Started,
  fetchEntities_Success,
  submitEntity_Failure,
  submitEntity_Started,
  submitEntity_Success
} from "./helpers";
import {
  createInitialNotificationsState,
  createInitialSubmitState
} from "./interfaces";



export const notificationsSlice = createSlice({
  name: "notifications",
  initialState: {
    notifications: createInitialNotificationsState(),
    submittingNotificationRead: createInitialSubmitState()
  },
  reducers: {
    //
    // Fetch all notifications:
    //
    fetchAllNotifications_Started(state, action: PayloadAction<void>) {
      fetchEntities_Started(state, "notifications", action)
    },
    fetchAllNotifications_Success(state, action: PayloadAction<Notification[]>) {
      fetchEntities_Success<Notification>(state, "notifications", action);
    },
    fetchAllNotifications_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      fetchEntities_Failure(state, "notifications", action);
    },
    //
    // Submit that a notification was read:
    //
    markNotificationAsRead_Started(state, action: PayloadAction<void>) {
      state.submittingNotificationRead = submitEntity_Started(state.submittingNotificationRead);
    },
    markNotificationAsRead_Success(state, action: PayloadAction<Notification>) {
      state.submittingNotificationRead = submitEntity_Success(state.submittingNotificationRead);
      
      const notifIndex = state.notifications.entities.findIndex(notif => notif.id === action.payload.id);
      
      const notification: Notification = state.notifications.entities[notifIndex];
      if (notification.readAt_DateUtc !== undefined) {
        return;
      }
      // if the notification was already read, update nothing.

      state.notifications.entities[notifIndex] = action.payload;
      // replace the notification with the returned one.
    },
    markNotificationAsRead_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      state.submittingNotificationRead = submitEntity_Failure(state.submittingNotificationRead, action)
    },
    //
    // NotificationCreated event:
    //
    notificationCreated(state, action: PayloadAction<Notification>) {
      state.notifications.entities = [...state.notifications.entities, action.payload];
    },
    //
    // NotificationRead event:
    //
    notificationRead(state, action: PayloadAction<Notification>) {
      const notifIndex = state.notifications.entities
        .findIndex(notif => notif.id === action.payload.id);

      const notification: Notification = state.notifications.entities[notifIndex];
      if (notification.readAt_DateUtc !== undefined) {
        return;
      }
      // if the notification was already read, update nothing.

      state.notifications.entities[notifIndex] = action.payload;
      // replace the notification with the returned one.
    }
  }
});



export const fetchAllNotifications = createAsyncThunk(
  "notifications/fetchAllNotifications",
  async (arg = undefined, { dispatch }) => {
    dispatch(notificationsSlice.actions.fetchAllNotifications_Started());

    try {
      const notifs = await getAllNotificationsForUser();
      dispatch(notificationsSlice.actions.fetchAllNotifications_Success(notifs));
    } catch (error) {
      dispatch(notificationsSlice.actions.fetchAllNotifications_Failure(HttpErrorsEnum.UnknownError));
    }
  }
)



export const markNotificationAsRead = createAsyncThunk(
  "notifications/markNotificationAsRead",
  async (notifId: number, { dispatch }: { dispatch: any }) => {
    dispatch(notificationsSlice.actions.markNotificationAsRead_Started());

    try {
      const notif = await markNotificationAsReadHttp(notifId);
      dispatch(notificationsSlice.actions.markNotificationAsRead_Success(notif));
    } catch (error) {
      dispatch(notificationsSlice.actions.markNotificationAsRead_Failure(HttpErrorsEnum.UnknownError));
    }
  }
)