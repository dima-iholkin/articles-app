import { userManager } from "src/IdentityService/UserManager";



interface AuthorizationHeader {
  headers: {
    Authorization: string
  }
}

export async function getAuthorizationHeader(): Promise<AuthorizationHeader> {
  const user = await userManager.getUser();
  const token = user?.access_token;
  if (user === null) {
    throw new Error("User is null.");
  }
  if (token === undefined) {
    throw new Error("access_token is undefined.");
  }

  return {
    headers: {
      Authorization: `Bearer ${token}`
    }
  };
}