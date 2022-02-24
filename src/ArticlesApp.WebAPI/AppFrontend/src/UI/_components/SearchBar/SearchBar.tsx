import { useSelector } from 'react-redux';
import { BsFunnel } from 'react-icons/bs';
import { MdClose } from 'react-icons/md';
import { makeStyles, styled } from "@material-ui/core/styles";
import Autocomplete from '@material-ui/lab/Autocomplete';
import { UIModeEnum } from "src/_UI/uiModeEnum";
import { Article } from 'src/Entities/Article';
import { setUIMode } from "src/Store/reducers/uiSlice";
import { store, storeType } from "src/Store/store";
import { TopBar } from 'src/UI/_components/NavBar/_components/TopBar';



export function SearchBar() {
  const classes = useStylesSearch();

  let articles = useSelector((state: storeType) => state.articles.approvedArticles.entities);

  const defaultProps = {
    options: articles,
    getOptionLabel: (option: Article) => option.title,
  };

  const handleClose = () => {
    store.dispatch(setUIMode(UIModeEnum.Default) as any);
  }

  return (
    <TopBar>
      <LeftCorner_Close onClick={handleClose} >
        <MdClose size={40} />
      </LeftCorner_Close>
      <Center_Search>
        <Autocomplete
          {...defaultProps}
          id="debug"
          debug
          className={classes.autocomplete}
          renderInput={(params) => (
            <div ref={params.InputProps.ref} className={classes.div}>
              <input
                {...params.inputProps}
                placeholder="Search here..."
                type="text"
                className={classes.input} />
            </div>
          )}
        />
      </Center_Search>
      <RightCorner_Filter>
        <BsFunnel size={32} className={classes.funnel} />
      </RightCorner_Filter>
    </TopBar>
  )
}



const LeftCorner_Close = styled("div")({
  paddingRight: "8px"
});

const Center_Search = styled("div")({
  flexGrow: 1
});

const RightCorner_Filter = styled("div")({
  paddingLeft: "8px",
  height: "40px",
  width: "40px"
});

const useStylesSearch = makeStyles({
  input: {
    width: "100%",
    height: "40px",
    boxSizing: "border-box"
  },
  autocomplete: {
    height: "100%",
    width: "100%"
  },
  div: {
    height: "100%",
    width: "100%"
  },
  funnel: {
    marginTop: "4px"
  }
});

// const AutocompleteStyled = styled(Autocomplete)