import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { User } from "oidc-client";
import { Article } from "src/Entities/Article";
// import { parseUserRoles } from "src/Store/Startup/parseUserRoles";
import { articlesSlice } from "../../Store/reducers/articlesSlice";
// import { ArticlePayload, articlesSlice } from "../../Store/reducers/articlesSlice";
import { store } from '../../Store/store'



let connection: HubConnection;



export function InitializeHub(user: User) {
  if (connection !== undefined) {
    console.warn("ArticlesHub attempted to initialize more than once.");
    return;
  }

  connection = new HubConnectionBuilder()
    .withUrl(
      "/api/myArticlesHub",
      {
        accessTokenFactory: () => user.access_token
      }
    )
    .build();

  connection.on(
    "MyArticleCreated",
    (payload: { article: Article }) => {
      store.dispatch(
        articlesSlice.actions.myArticleCreated(payload.article)
      );
    }
  )

  connection.on(
    "MyArticleModeratorDecision",
    (payload: { article: Article }) => {
      store.dispatch(
        articlesSlice.actions.myArticleModeratorDecision(payload.article)
      );
    }
  )

  connection.on(
    "MyArticleSoftDeleted",
    (payload: { article: Article }) => {
      store.dispatch(
        articlesSlice.actions.myArticleSoftDeleted(payload.article)
      );
    }
  )

  connection.start()
    .catch(
      err => console.log(err)
    );
}