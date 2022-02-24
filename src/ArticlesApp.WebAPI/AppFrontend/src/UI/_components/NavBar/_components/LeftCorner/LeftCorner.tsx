import { useState } from 'react';
import { useSelector } from 'react-redux';
import { goBack } from "connected-react-router";
import { MdArrowBack } from "react-icons/md";
import { Button } from '@material-ui/core';
import { styled } from '@material-ui/core/styles';
import { store, storeType } from "src/Store/store";
import { Routes } from '../../../Routing/Routes';
import { Logo } from "./_components/Logo";
import { NavbarLeftButtonValuesEnum } from './NavbarLeftButtonValuesEnum';



export let setLeftButtonValue = (buttonValue: NavbarLeftButtonValuesEnum): void => {
  throw new Error("Not implemented exception.");
}



export function LeftCorner() {
  const [leftButtonValue, _setLeftButtonValue] = useState(NavbarLeftButtonValuesEnum.Default);

  setLeftButtonValue = (buttonValue) => {
    _setLeftButtonValue(buttonValue);
  }

  const route: string = useSelector((state: storeType) => state.router.location.pathname);

  const handleBackClick = () => {
    store.dispatch(goBack());
  }

  if (route === Routes.root) {
    return (
      <Div>
        <div>
          <Logo />
        </div>
      </Div>
    )
  }

  const buttonText: string = NavbarLeftButtonValuesEnum[leftButtonValue];
  // get the string name from enum.

  return (
    <Div>
      <ArrowDiv onClick={handleBackClick}>
        {
          leftButtonValue === NavbarLeftButtonValuesEnum.Default
            ? (
              <MdArrowBack size={iconSize} />
            )
            : (
              <Button
                variant="outlined"
                style={{
                  backgroundColor: "#FFF",
                  marginLeft: "6px"
                }}
              >
                {buttonText}
              </Button>
            )
        }
      </ArrowDiv>
    </Div>
  )
}



const iconSize: number = 40;

const Div = styled("div")({
  width: "auto",
  display: "flex",
  flexDirection: "row",
  justifyContent: "flex-start"
});

const ArrowDiv = styled("div")({
  transform: "translateX(-6px)"
})