import axios, { AxiosResponse } from "axios";
import { User } from "oidc-client";
import qs from "qs";
import { ApplicationName } from "src/IdentityService/ApiAuthorizationConstants";
import { getAuthorizationHeader } from "./_Helpers/getAuthorizationHeader";



export async function refreshAccessToken(refreshToken: string): Promise<User> {
  const headers = await getAuthorizationHeader();
  // @ts-ignore
  headers.headers["Content-Type"] = "application/x-www-form-urlencoded";

  const body = qs.stringify({
    "client_id": ApplicationName,
    "grant_type": "refresh_token",
    "refresh_token": refreshToken
  });

  const response: AxiosResponse<User> = await axios.post(
    "/connect/token",
    body,
    headers
  );

  return response.data;
}
// POST: connect/token
// body: { User }