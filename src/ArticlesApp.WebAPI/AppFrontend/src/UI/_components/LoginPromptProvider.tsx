import React, { PropsWithChildren, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { goBack } from "connected-react-router";
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import { userManager } from "src/IdentityService/UserManager";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import { store, storeType } from 'src/Store/store';
import { useUserRole } from '../_Helpers/useUserRole';



let showLoginPrompt = (): void => {
  throw new Error("Not implemented.")
}

export function useLoginPrompt() {
  const user: UserRolesEnum[] = useUserRole();
  let isLoadingUser = useSelector((state: storeType) => state.oidc.isLoadingUser);
  let isLogoutRedirect = useSelector((state: storeType) => state.ui.isLogoutRedirect);

  useEffect(() => {
    if (
      user.includes(UserRolesEnum.Unauthenticated)
      && isLoadingUser === false
      && isLogoutRedirect === false
    ) {
      showLoginPrompt();
    }
  });
}



export function LoginPromptProvider(props: PropsWithChildren<{}>) {
  const [open, setOpen] = React.useState(false);

  showLoginPrompt = () => {
    if (open === false) {
      setOpen(true);
    }
  }
  // override the exported function, to enable the external triggering of it.



  const handleClose = () => {
    setOpen(false);
  };

  const handleCancel = () => {
    store.dispatch(goBack());
    setOpen(false);
  }

  const handleProceed = () => {
    userManager.signinRedirect({
      state: {
        prevPage: window.location.pathname
      }
    });
    setOpen(false);
  }



  return (
    <>
      <div>
        <Dialog
          open={open}
          onClose={handleClose}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description"
        >
          <DialogContent>
            <DialogContentText
              id="alert-dialog-description"
              align="center"
            >
              You have to be logged-in to accomplish this action.
            </DialogContentText>
          </DialogContent>
          <DialogActions style={{ justifyContent: "space-evenly" }}>
            <Button
              onClick={handleCancel}
              color="primary"
            >
              Cancel
            </Button>
            <Button
              onClick={handleProceed}
              color="primary"
              autoFocus
            >
              Proceed to Login
            </Button>
          </DialogActions>
        </Dialog>
      </div>
      {props.children}
    </>
  );
}