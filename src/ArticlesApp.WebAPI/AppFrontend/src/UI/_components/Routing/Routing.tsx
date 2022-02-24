import { Route, Switch } from 'react-router';
import { Redirect } from 'react-router-dom';
import { ApplicationPaths } from 'src/IdentityService/ApiAuthorizationConstants';
import { ArticleDetailsPage } from "../../Pages/ArticleDetailsPage/ArticleDetailsPage";
import { ArticleProposalPage } from "../../Pages/ArticleProposalPage/ArticleProposalPage";
import { ArticlesListPage } from "../../Pages/ArticlesListPage/ArticlesListPage";
import SigninCallbackPage from "../../Pages/Identity/SigninCallbackPage";
import SignoutCallbackPage from "../../Pages/Identity/SignoutCallbackPage";
import SilentRefreshPage from "../../Pages/Identity/SilentRefreshPage";
import { MyArticleDetailsPage } from "../../Pages/MyArticleDetailsPage/MyArticleDetailsPage";
import { MyArticlesListPage } from '../../Pages/MyArticlesListPage/MyArticlesListPage';
import { MyDecisionsListPage } from '../../Pages/MyDecisionsListPage/MyDecisionsListPage';
import { NotificationsPage } from '../../Pages/NotificationsPage/NotificationsPage';
import { PendingArticleDecisionPage } from '../../Pages/PendingArticleDecisionPage/PendingArticleDecisionPage';
import { PendingArticlesListPage } from '../../Pages/PendingArticlesListPage/PendingArticlesListPage';
import { Routes } from "./Routes";



export function Routing() {
  return (
    <Switch>
      <Route exact path={Routes.root} component={ArticlesListPage} />
      <Route exact path={["/Home", Routes.articles.root]}
        render={() => <Redirect to="/" />}
      />
      {/* view the list of approved articles. */}

      <Route path={Routes.articles.my.root}>
        <Switch>
          <Route exact path={Routes.articles.my.root} component={MyArticlesListPage} />
          <Route exact path={Routes.articles.my.decisions} component={MyDecisionsListPage} />
          <Route exact path={Routes.articles.my.articleId} component={MyArticleDetailsPage} />
        </Switch>
      </Route>
      {/* view the list of my articles. */}

      <Route path={Routes.articles.pending.root}>
        <Switch>
          <Route exact path={Routes.articles.pending.root} component={PendingArticlesListPage} />
          <Route exact path={Routes.articles.pending.articleId} component={PendingArticleDecisionPage} />
        </Switch>
      </Route>
      {/* Pending articles. */}

      <Route path={Routes.articles.add}>
        <ArticleProposalPage proposalType="add" />
      </Route>
      {/* Add a new Article. */}

      <Route exact path={Routes.articles.articleId} component={ArticleDetailsPage} />
      {/* View the Article's details. */}

      <Route path={Routes.articles.edit}
        children={({ match }) => (
          <ArticleProposalPage proposalType="edit" articleId={parseInt((match?.params.articleId)!)} />
        )}
      />
      {/* Edit an Article. */}

      <Route path={Routes.articles.delete}
        children={({ match }) => (
          <ArticleProposalPage proposalType="delete" articleId={parseInt((match?.params.articleId)!)} />
        )}
      />
      {/* Delete an Article. */}

      <Route path={ApplicationPaths.LoginCallback} component={SigninCallbackPage} />
      <Route path={ApplicationPaths.LogOutCallback} component={SignoutCallbackPage} />
      <Route path={"/silent-refresh"} component={SilentRefreshPage} />
      {/* OpenIDConnect routes. */}

      <Route exact path={Routes.notifications} component={NotificationsPage} />
      {/* view the list of notifications. */}
    </Switch>
  )
}