import { useSelector } from 'react-redux';
import { styled } from '@material-ui/core/styles';
import { UIModeEnum } from 'src/_UI/uiModeEnum';
import { storeType } from 'src/Store/store';
import { CookiePrompt } from "src/UI/_components/CookiePrompt";
import { LoginPromptProvider } from 'src/UI/_components/LoginPromptProvider';
import { NavBar } from 'src/UI/_components/NavBar/NavBar';
import { Routing } from 'src/UI/_components/Routing/Routing';
import { SearchBar } from 'src/UI/_components/SearchBar/SearchBar';
import { SnackbarProvider } from "src/UI/_components/SnackbarProvider";
import { SearchPage } from "src/UI/Pages/SearchPage/SearchPage";



export function App() {
  let uiMode: UIModeEnum = useSelector((state: storeType) => state.ui.uiMode);

  switch (uiMode) {
    case UIModeEnum.Default:
      return (
        <CookiePrompt>
          <LoginPromptProvider>
            <SnackbarProvider>
              <NavBar />
              <ContentArea>
                <Routing />
              </ContentArea>
            </SnackbarProvider>
          </LoginPromptProvider>
        </CookiePrompt>
      )
    case UIModeEnum.Search:
      return (
        <>
          <SearchBar />
          <ContentArea>
            <SearchPage />
          </ContentArea>
        </>
      )
    default:
      return (
        <>
          <p>Please wait...</p>
        </>
      )
  }
}



const ContentArea = styled("div")({
  display: "flex",
  flexDirection: "column",
  flexGrow: 1
});