import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { User } from "oidc-client";
import { notificationsSlice } from "../../Store/reducers/notificationsSlice";
import { store } from '../../Store/store'



let connection: HubConnection;



export function InitializeHub(user: User) {
  if (connection !== undefined) {
    console.warn("NotificationsHub attempted to initialize more than once.");
    return;
  }

  connection = new HubConnectionBuilder()
    .withUrl(
      "/api/notificationsHub",
      {
        accessTokenFactory: () => user.access_token
      }
    )
    .build();

  connection.on(
    "NotificationCreated",
    payload => {
      store.dispatch(notificationsSlice.actions.notificationCreated(payload.notification));
    }
  );

  connection.on(
    "NotificationRead",
    payload => {
      store.dispatch(notificationsSlice.actions.notificationRead(payload.notification));
    }
  )

  connection.start()
    .catch(err => console.log(err));
}