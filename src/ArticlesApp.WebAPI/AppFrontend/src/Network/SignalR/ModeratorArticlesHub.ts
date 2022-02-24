import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { User } from "oidc-client";
import { Article } from "src/Entities/Article";
// import { parseUserRoles } from "src/Store/Startup/parseUserRoles";
import { ArticlePayload, articlesSlice } from "../../Store/reducers/articlesSlice";
// import { ArticlePayload, articlesSlice } from "../../Store/reducers/articlesSlice";
import { store } from '../../Store/store'



let connection: HubConnection;



export function InitializeHub(user: User) {
  if (connection !== undefined) {
    console.warn("ModeratorArticlesHub attempted to initialize more than once.");
    return;
  }

  connection = new HubConnectionBuilder()
    .withUrl(
      "/api/moderatorArticlesHub",
      {
        accessTokenFactory: () => user.access_token
      }
    )
    .build();

  connection.on(
    "PendingArticleCreated",
    (payload: { article: Article }) => {
      store.dispatch(
        articlesSlice.actions.moderatorPendingArticleCreated(payload.article)
      );
    }
  )

  connection.on(
    "ArticleModeratorDecision",
    (payload: { article: Article }) => {
      const user: User | undefined = store.getState().oidc.user;
      const userId: string | undefined = user?.profile?.sub;

      const articlePayload: ArticlePayload = {
        article: payload.article,
        userId: userId
      };
      store.dispatch(
        articlesSlice.actions.moderatorArticleDecision(articlePayload)
      );
    }
  )

  connection.on(
    "ArticleSoftDeleted",
    (payload: { article: Article }) => {
      store.dispatch(
        articlesSlice.actions.moderatorArticleSoftDeleted(payload.article)
      );
    }
  )

  connection.start()
    .catch(
      err => console.log(err)
    );
}