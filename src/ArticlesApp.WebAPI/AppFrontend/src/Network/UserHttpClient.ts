import axios, { AxiosResponse } from "axios";



interface IsSignedInObj {
  isSignedIn: boolean,
  isSoftDeleted: boolean
}



export async function getUserIsSignedIn(): Promise<IsSignedInObj> {
  const response: AxiosResponse<IsSignedInObj> = await axios.get("/api/user/_signed-in");

  return response.data;
}
// GET: api/user/_signed-in
// body: { 
//   isSignedIn: boolean,
//   isSoftDeleted: boolean
// }