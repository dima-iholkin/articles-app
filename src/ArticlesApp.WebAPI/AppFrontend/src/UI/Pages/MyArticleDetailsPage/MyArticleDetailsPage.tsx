import { useSelector } from 'react-redux';
import { RouteComponentProps } from "react-router-dom";
import { styled } from "@material-ui/core/styles";
import { ArticleStatesEnum } from "src/Entities/ArticleStatesEnum";
import { storeType } from 'src/Store/store';
import { useLoginPrompt } from "src/UI/_components/LoginPromptProvider";
import { ArticleDetails } from 'src/UI/Pages/_components/ArticleDetails/ArticlesDetails';
import { RouterMatchParams } from "src/UI/Pages/_components/ArticleDetails/RouterMatchParams";
import { ArticleStateChip } from "src/UI/Pages/_components/ArticleStateChip";



export function MyArticleDetailsPage(props: RouteComponentProps<RouterMatchParams>) {
  useLoginPrompt();

  const articlesFromStore = useSelector((state: storeType) => state.articles.myArticles.entities);
  const articleId = parseInt(props.match.params.articleId);
  const article = articlesFromStore.find(ar => ar.id === articleId);

  return (
    <ArticleDetails
      article={article}
      titleChildren={(
        <TitleContainer >
          <Title>
            {article?.title}
          </Title>
          <ArticleStateChip
            articleState={article?.articleStateId === undefined ? ArticleStatesEnum.Approved : article.articleStateId}
          />
        </TitleContainer>
      )}
    />
  )
}



const TitleContainer = styled("div")({
  flexGrow: 1,
  display: "flex",
  flexDirection: "row",
  alignContent: "flex-start",
  alignItems: "center",
  minHeight: "46px",
  marginTop: "8px",
  marginBottom: "8px",
  paddingTop: "8px"
})

const Title = styled("h2")({
  textAlign: "left",
  paddingLeft: "16px",
  margin: "0",
  marginRight: "2px",
  fontFamily: "'Open Sans', sans-serif"
})