import { useHistory } from 'react-router-dom';
import { Card, CardContent, Typography } from "@material-ui/core";
import { makeStyles, styled } from '@material-ui/styles';
import { Article } from "src/Entities/Article";
import { ArticleStateChip } from "UI/Pages/_components/ArticleStateChip";
import { RenderProps } from "UI/Pages/_components/List";



interface CardComponentProps {
  showArticleStatus: boolean,
  articleRouteBase: string
}

export function CardComponent(props: RenderProps<Article> & CardComponentProps) {
  const classes = useStyles();
  const typographyClasses = typographyTitle();
  const history = useHistory();

  const handleArticleClick = (id: number) => {
    history.push(props.articleRouteBase + "/" + id);
  }

  return (
    <Card
      classes={classes}
      onClick={() => { handleArticleClick(props.entity.id) }} >
      <LeftChipDiv />
      <MiddleDiv>
        <CardContent style={{
          paddingRight: "10px",
          paddingLeft: "10px",
          paddingBottom: "16px"
        }}>
          <Typography classes={typographyClasses}>
            {props.entity.title}
          </Typography>
        </CardContent>
      </MiddleDiv>
      <RightChipDiv>
        {
          props.showArticleStatus ? (
            <ArticleStateChip articleState={props.entity.articleStateId} />
          ) : (
            <div />
          )
        }
      </RightChipDiv>
    </Card>
  )
}



const useStyles = makeStyles({
  root: {
    backgroundColor: "white",
    borderRadius: "0",
    display: "grid",
    gridTemplateColumns: "1fr auto 1fr",
    paddingBottom: "8px"
  }
});

const typographyTitle = makeStyles({
  body1: {
    fontFamily: "'Open Sans', sans-serif",
    userSelect: "none",
    whiteSpace: "nowrap",
    overflow: "hidden",
    textOverflow: "ellipsis"
  }
})

const LeftChipDiv = styled("div")({
});

const MiddleDiv = styled("div")({
  overflow: "hidden"
});

const RightChipDiv = styled("div")({
  display: "flex",
  flexDirection: "row",
  justifyContent: "flex-end",
  alignItems: "center"
});