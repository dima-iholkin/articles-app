import axios, { AxiosRequestConfig, AxiosResponse } from "axios";
import { Notification } from "src/Entities/Notification";
import { getAuthorizationHeader } from "./_Helpers/getAuthorizationHeader";
import { routes } from "./routes";



export async function getAllNotificationsForUser(): Promise<Notification[]> {
  const header = await getAuthorizationHeader();
  const response = await axios.get(
    routes.getAllNotificationsForUser,
    header
  );
  return response.data;
}
// GET: api/notifications



export async function getNewNotificationsForUser(lastFetchedNotifId: number): Promise<Notification[]> {
  const header: AxiosRequestConfig = await getAuthorizationHeader();
  const response = await axios.get(
    routes.getNewNotificationsForUser(lastFetchedNotifId),
    header
  );
  return response.data;
}
// GET: api/notifications/new/5



export async function markNotificationAsRead(notificationId: number): Promise<Notification> {
  const header = await getAuthorizationHeader();
  const response: AxiosResponse<Notification> = await axios.put(
    routes.markNotificationAsRead(notificationId),
    undefined,
    header
  );
  return response.data;
}
// PUT: api/notifications/5