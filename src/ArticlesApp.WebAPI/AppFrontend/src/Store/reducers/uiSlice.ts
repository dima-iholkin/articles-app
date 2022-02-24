import { createSlice } from "@reduxjs/toolkit";
import { UIModeEnum } from 'src/_UI/uiModeEnum';



export const uiSlice = createSlice({
  name: "ui",
  initialState: {
    uiMode: UIModeEnum.Default,
    isLogoutRedirect: false
  },
  reducers: {
    setUIMode(state, action) {
      state.uiMode = action.payload;
    },
    setIsLogoutRedirect(state) {
      state.isLogoutRedirect = true;
    }
  }
})



export const setUIMode = (mode: UIModeEnum) => {
  return function (dispatch: any) {
    dispatch(uiSlice.actions.setUIMode(mode));
  }
}



export const setIsLogoutRedirect = () => {
  return function (dispatch: any) {
    dispatch(uiSlice.actions.setIsLogoutRedirect());
  }
}