import { useSelector } from 'react-redux';
import { RouteComponentProps } from "react-router-dom";
import { push } from "connected-react-router";
import { Button } from '@material-ui/core';
import { styled } from "@material-ui/core/styles";
import { ArticleStatesEnum } from "src/Entities/ArticleStatesEnum";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import { moderatorDecidesOnArticle } from "src/Store/reducers/articlesSlice";
import { storeType, useAppDispatch } from 'src/Store/store';
import { Routes } from 'UI/_components/Routing/Routes';
import { addMessage } from "UI/_components/SnackbarProvider";
import { useUserRole } from 'UI/_Helpers/useUserRole';
import { ArticleDetails } from 'UI/Pages/_components/ArticleDetails/ArticlesDetails';
import { RouterMatchParams } from "UI/Pages/_components/ArticleDetails/RouterMatchParams";
import { ArticleStateChip } from "UI/Pages/_components/ArticleStateChip";



export function PendingArticleDecisionPage(props: RouteComponentProps<RouterMatchParams>) {
  const userRole: UserRolesEnum[] = useUserRole();

  let pendingArticles = useSelector((state: storeType) => state.articles.pendingArticles.entities);
  const articleId = parseInt(props.match.params.articleId);
  let article = pendingArticles.find(ar => ar.id === articleId);

  const userId = useSelector((state: storeType) => state.oidc.user?.profile.sub);

  const dispatch = useAppDispatch();

  function handleButtonClick(decision: ArticleStatesEnum) {
    return () => {
      dispatch(moderatorDecidesOnArticle({
        articleId: article!.id,
        decision: decision,
        versionId: article!.versionId,
        currentUserId: userId!
      }));

      dispatch(push(Routes.articles.pending.root));

      addMessage("Decision was submitted.");
    }
  }

  return (
    <ArticleDetails
      article={article}
      titleChildren={(
        <TitleContainer>
          <Title>{article?.title}</Title>
          <ArticleStateChip
            articleState={article?.articleStateId === undefined ? ArticleStatesEnum.Approved : article.articleStateId}
          />
        </TitleContainer>
      )}
      belowTextChildren={(
        <Div>
          <Button
            variant="contained"
            onClick={handleButtonClick(ArticleStatesEnum.Rejected)}
            disabled={userRole.includes(UserRolesEnum.Moderator) === false}
            style={{
              marginLeft: "10%",
              backgroundColor: "salmon"
            }}
          >
            Reject
          </Button>
          <Button
            variant="contained"
            onClick={handleButtonClick(ArticleStatesEnum.Approved)}
            disabled={userRole.includes(UserRolesEnum.Moderator) === false}
            style={{
              marginRight: "10%",
              backgroundColor: "palegreen"
            }}
          >
            Approve
          </Button>
        </Div>
      )}
    />
  )
}



const Div = styled("div")({
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  marginTop: "3rem"
})

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