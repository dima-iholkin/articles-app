import React, { PropsWithChildren } from 'react';
import {
  get as getCookie,
  set as setCookie
} from "js-cookie";
import {
  NIL as uuidNIL,
  v4 as uuidV4
} from "uuid";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText
} from '@material-ui/core';
import { makeStyles } from '@material-ui/styles';



export function CookiePrompt(props: PropsWithChildren<{}>) {
  const classes = useStyles();

  let showCookiePrompt: boolean = false;
  const analyticsCookie = getCookie("ClientLoggingId");
  if (analyticsCookie === undefined) {
    showCookiePrompt = true;
  }

  const [open, setOpen] = React.useState(showCookiePrompt);

  const handleDisagree = () => {
    setCookie(
      "ClientLoggingId",
      uuidNIL,
      { expires: 30 }
    );
    setOpen(false);
  }

  const handleAgree = () => {
    const newId = uuidV4();
    setCookie(
      "ClientLoggingId",
      newId,
      { expires: 365 }
    );
    setOpen(false);
  }

  const handleClose = () => {
    setOpen(false);
  }

  return (
    <>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        classes={classes}
      >
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            Please agree to the analytics cookie, if you want to help us improve the app. 
            We collect information like the pages visited with an anonymous cliend id.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDisagree} color="primary">
            Disagree
          </Button>
          <Button onClick={handleAgree} color="primary" autoFocus>
            Agree
          </Button>
        </DialogActions>
      </Dialog>
      {props.children}
    </>
  )
}



const useStyles = makeStyles({
  paper: {
    position: "absolute",
    bottom: 0,
    marginBottom: 0,
    borderRadius: "4px 4px 0 0",
    ["@media (min-width: 60ch)"]: { // eslint-disable-line no-useless-computed-key
      width: "60ch"
    },
    width: "100%",
  },
  container: {
    overflowY: "scroll"
  }
});