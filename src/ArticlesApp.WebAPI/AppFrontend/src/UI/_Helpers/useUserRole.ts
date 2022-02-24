import { useSelector } from "react-redux";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import { parseUserRoles } from "src/Store/Startup/parseUserRoles";
import { storeType } from "src/Store/store";



export function useUserRole(): UserRolesEnum[] {
  const userRole: string | string[] | undefined = useSelector((state: storeType) => state.oidc.user?.profile?.role);

  return parseUserRoles(userRole);
}