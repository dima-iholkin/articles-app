import React, { PropsWithChildren, useEffect, useLayoutEffect, useState } from 'react';
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert, { AlertProps } from '@material-ui/lab/Alert';



let positionSnackbarAboveFab = (value: boolean): void => {
  throw new Error("Not implemented.");
}

export function useFab() {
  useLayoutEffect(() => {
    positionSnackbarAboveFab(true);
    return () => {
      positionSnackbarAboveFab(false);
    }
  });
}



const messagesQueue: string[] = [];

export let addMessage = (str: string) => {
  messagesQueue.push(str);
};

const popMessage = () => messagesQueue.shift();



interface State {
  walkingThroughMessagesInTheQueue: boolean;
  messageToDisplay: string | undefined;
}

export function SnackbarProvider(props: PropsWithChildren<{}>) {
  const [state, setState] = useState<State>({
    walkingThroughMessagesInTheQueue: false,
    messageToDisplay: undefined
  });

  const onMessageAddedToQueue = () => {
    if (state.walkingThroughMessagesInTheQueue === false) {
      walkThroughMessagesInTheQueue();
    }
  }

  addMessage = (str: string) => {
    messagesQueue.push(str);
    if (messagesQueue.length > 0) {
      onMessageAddedToQueue();
    }
  }
  // override the function.

  useEffect(() => {
    if (messagesQueue.length > 0) {
      onMessageAddedToQueue();
    }
    // need this to avoid infinite recursion.
  });

  const walkThroughMessagesInTheQueue = () => {
    const poppedMessage = popMessage();
    if (poppedMessage === undefined) {
      setState({
        walkingThroughMessagesInTheQueue: false,
        messageToDisplay: undefined
      });
      handleClose();
    } else {
      setState({
        walkingThroughMessagesInTheQueue: true,
        messageToDisplay: poppedMessage
      });
      setOpen(true);
      setTimeout(() => {
        setState({
          walkingThroughMessagesInTheQueue: true,
          messageToDisplay: undefined
        });
        handleClose();
        setTimeout(() => {
          walkThroughMessagesInTheQueue()
        }, 1000);
      }, 5000);
    }
  }

  const [fabIsOn, setFabIsOn] = React.useState(false);
  positionSnackbarAboveFab = (value: boolean) => {
    setFabIsOn(value);
  }
  // override the function.

  let myStyle: React.CSSProperties;
  if (fabIsOn) {
    myStyle = { bottom: "88px" };
  }

  const [open, setOpen] = React.useState(true);

  // const handleClick = () => {
  //   setOpen(true);
  // };

  const handleClose = (event?: React.SyntheticEvent, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setOpen(false);
  };

  if (state.messageToDisplay !== undefined) {
    return (
      <>
        {props.children}
        <Snackbar open={open} onClose={handleClose} style={myStyle!}>
          <Alert onClose={handleClose} severity="success">
            {state.messageToDisplay}
          </Alert>
        </Snackbar>
      </>
    )
  } else {
    return (
      <>
        {props.children}
      </>
    )
  }
}



function Alert(props: AlertProps) {
  return <MuiAlert elevation={6} variant="filled" {...props} />;
}