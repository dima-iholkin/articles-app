import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";



export function parseUserRoles(userRolesStr: string | string[] | undefined) {
  const returnObj: UserRolesEnum[] = [];

  if (userRolesStr === undefined) {
    returnObj.push(UserRolesEnum.Unauthenticated);

    return returnObj;
  }

  if (typeof userRolesStr === "string") {
    switch (userRolesStr) {
      case "User":
        returnObj.push(UserRolesEnum.User);
        break;

      case "Moderator":
        returnObj.push(UserRolesEnum.Moderator);
        break;

      default:
        returnObj.push(UserRolesEnum.Unauthenticated);
    }

    return returnObj;
  }

  userRolesStr.forEach(userRole => {
    switch (userRole) {
      case "User":
        returnObj.push(UserRolesEnum.User);
        break;

      case "Moderator":
        returnObj.push(UserRolesEnum.Moderator);
        break;

      default:
        break;
    }
  });

  return returnObj;
}