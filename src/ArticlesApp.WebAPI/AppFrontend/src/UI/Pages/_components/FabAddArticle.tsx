import { useHistory } from 'react-router-dom';
import Fab from '@material-ui/core/Fab';
import AddIcon from '@material-ui/icons/Add';
import { styled } from '@material-ui/styles';
import { Routes } from 'UI/_components/Routing/Routes';



export function FabAddArticle() {
  const history = useHistory();

  const handleClick = () => {
    history.push(Routes.articles.add);
  }

  return (
    <FabContainer>
      <FabStyled
        size="large"
        variant="extended"
        onClick={handleClick}
        aria-label="add article"
      >
        <AddIcon />
        Add article
      </FabStyled>
    </FabContainer>
  )
}



const FabStyled = styled(Fab)({
  color: "white",
  backgroundColor: "forestgreen"
})

const FabContainer = styled("div")({
  bottom: "20px",
  position: "sticky",
  marginLeft: "auto",
  marginRight: "auto"
})