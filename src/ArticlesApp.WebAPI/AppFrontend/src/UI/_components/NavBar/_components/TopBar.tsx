import { PropsWithChildren } from 'react';
import { Box } from '@material-ui/core';
import { styled } from "@material-ui/core/styles";



export function TopBar(props: PropsWithChildren<{}>) {
  return (
    <BoxStyled boxShadow={4} p={1}>
      {props.children}
    </BoxStyled>
  )
}



const iconSize: number = 40;
const iconSize_string: string = iconSize + "px";

const BoxStyled = styled(Box)({
  backgroundColor: "#E8E8E8",
  position: "sticky",
  top: "0",
  height: iconSize_string,
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  padding: "8px",
  boxSizing: "content-box",
  zIndex: 1 // add the fix the NotificationList's "new" Chips, 
  // because if these chips have an onClick handler,
  // they will pop-up over normal components such as the NavBar.  
});