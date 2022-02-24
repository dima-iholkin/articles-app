import { MdSearch } from "react-icons/md";
import { styled } from "@material-ui/styles";
import { UIModeEnum } from 'src/_UI/uiModeEnum';
import { setUIMode } from "src/Store/reducers/uiSlice";
import { store } from "src/Store/store";



export function SearchButton() {
  const handleSearchClick = () => {
    store.dispatch(setUIMode(UIModeEnum.Search) as any);
  }

  return (
    <SearchDiv onClick={handleSearchClick}>
      <MdSearch size={iconSize} />
    </SearchDiv>
  )
}



const iconSize: number = 40;

const SearchDiv = styled("div")({
  paddingRight: "8px"
})