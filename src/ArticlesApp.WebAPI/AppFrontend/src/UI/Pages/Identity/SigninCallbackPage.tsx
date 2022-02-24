import { useHistory } from "react-router-dom";
import { CallbackComponent } from "redux-oidc";
import { userManager } from "src/IdentityService/UserManager";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import { Routes } from "../../_components/Routing/Routes";
import { useUserRole } from "../../_Helpers/useUserRole";



export default function SigninCallbackPage() {
  const history = useHistory();
  const userRole: UserRolesEnum[] = useUserRole();

  return (
    <CallbackComponent
      userManager={userManager}
      successCallback={(user) => {
        if (user.state.prevPage) {
          history.push(user.state.prevPage);
        } else {
          history.push(Routes.root);
        }
      }}
      errorCallback={(error) => {
        if (error.message === "No state in response") {
          history.push(Routes.root);
          console.log("This seems to be a Logout Callback.");
        }
        else if (error.message === "login_required") {
          console.log("error login_required flow started. in CallbackPage");
        }
        else {
          if (userRole.includes(UserRolesEnum.Unauthenticated) === false) {
            history.push(Routes.root);
          } else {
            console.log("Unknown error in CallbackPage.");
          }
        }
      }}
    >
      <div>Redirecting...</div>
    </CallbackComponent>
  )
}