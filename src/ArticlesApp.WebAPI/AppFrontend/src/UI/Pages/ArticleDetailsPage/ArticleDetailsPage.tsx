import { useSelector } from 'react-redux';
import { RouteComponentProps } from "react-router-dom";
import { styled } from "@material-ui/core/styles";
import { storeType } from 'src/Store/store';
import { ArticleDetails } from '../_components/ArticleDetails/ArticlesDetails';
import { RouterMatchParams } from "../_components/ArticleDetails/RouterMatchParams";



export function ArticleDetailsPage(props: RouteComponentProps<RouterMatchParams>) {
  const articlesFromStore = useSelector((state: storeType) => state.articles.approvedArticles.entities);
  const articleId = parseInt(props.match.params.articleId);
  const article = articlesFromStore.find(ar => ar.id === articleId);

  return (
    <ArticleDetails
      titleChildren={(
        <TitleContainer>
          <Title>{article?.title}</Title>
        </TitleContainer>
      )}
      article={article}
    />
  )
}



const TitleContainer = styled("div")({
  flexGrow: 1
})

const Title = styled("h2")({
  textAlign: "left",
  padding: "16px 0 8px 16px",
  margin: "0",
  fontFamily: "'Open Sans', sans-serif"
})