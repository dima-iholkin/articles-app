import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
// import { User } from "oidc-client";
import { WritableDraft } from "immer/dist/internal";
import { Article, ArticleSubmit } from "src/Entities/Article";
import { ArticleStatesEnum } from "src/Entities/ArticleStatesEnum";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import {
  getAllApprovedArticles,
  getAllMyArticles,
  moderatorGetsAllOwnDecisions as moderatorGetsAllOwnDecisionsHttp,
  moderatorGetsAllPendingArticles as moderatorGetsAllPendingArticlesHttp,
  moderatorMakesDecisionOnPendingArticle,
  userDeletesOwnArticle as userDeletesOwnArticleHttp,
  userSubmitsAnArticle as userSubmitsAnArticle_HttpClient
} from "src/Network/ArticlesHttpClient";
import { HttpErrorsEnum } from "src/Network/HttpErrorsEnum";
// import { store as reduxStore } from "src/Store/store";
// import { parseUserRoles } from "../Startup/parseUserRoles";
import { addToArticlesArray } from "./_Helpers/addToArticlesArray";
import { removeFromArticlesArray } from "./_Helpers/removeFromArticlesArray";
import {
  fetchEntities_Failure,
  fetchEntities_Started,
  fetchEntities_Success,
  submitEntity_Failure,
  submitEntity_Started,
  submitEntity_Success
} from "./helpers";
import { createInitialArticlesState, createInitialSubmitState } from "./interfaces";
import { EntitiesArrayShape, SubmitShape } from "./interfaces";



export const articlesSlice = createSlice({
  name: "articles",
  initialState: {
    approvedArticles: createInitialArticlesState(),
    myArticles: createInitialArticlesState(),
    pendingArticles: createInitialArticlesState(),
    moderatorOwnDecisions: createInitialArticlesState(),
    submittingArticle: createInitialSubmitState(),
    moderatorSubmittingDecision: createInitialSubmitState(),
    userDeletesOwnArticle: createInitialSubmitState()
  },
  reducers: {
    //
    // All approved articles:
    //
    fetchAllApprovedArticles_Started(state, action: PayloadAction<void>) {
      fetchEntities_Started(state, "approvedArticles", action);
    },
    fetchAllApprovedArticles_Success(state, action: PayloadAction<Article[]>) {
      fetchEntities_Success<Article>(state, "approvedArticles", action);
    },
    fetchAllApprovedArticles_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      fetchEntities_Failure(state, "approvedArticles", action);
    },
    //
    // All my articles:
    //
    fetchAllMyArticles_Started(state, action: PayloadAction<void>) {
      fetchEntities_Started(state, "myArticles", action);
    },
    fetchAllMyArticles_Success(state, action: PayloadAction<Article[]>) {
      fetchEntities_Success<Article>(state, "myArticles", action);
    },
    fetchAllMyArticles_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      fetchEntities_Failure(state, "myArticles", action);
    },
    //
    // All pending articles:
    //
    fetchAllPendingArticles_Started(state, action: PayloadAction<void>) {
      fetchEntities_Started(state, "pendingArticles", action);
    },
    fetchAllPendingArticles_Success(state, action: PayloadAction<Article[]>) {
      fetchEntities_Success<Article>(state, "pendingArticles", action);
    },
    fetchAllPendingArticles_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      fetchEntities_Failure(state, "pendingArticles", action);
    },
    //
    // All moderator own decisions:
    //
    fetchAllModeratorOwnDecisions_Started(state, action: PayloadAction<void>) {
      fetchEntities_Started(state, "moderatorOwnDecisions", action);
    },
    fetchAllModeratorOwnDecisions_Success(state, action: PayloadAction<Article[]>) {
      fetchEntities_Success<Article>(state, "moderatorOwnDecisions", action);
    },
    fetchAllModeratorOwnDecisions_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      fetchEntities_Failure(state, "moderatorOwnDecisions", action);
    },
    //
    // Submit an article:
    //
    submitArticle_Started(state, action: PayloadAction<void>) {
      state.submittingArticle = submitEntity_Started(state.submittingArticle);
    },
    submitArticle_Success(state, action: PayloadAction<Article>) {
      state.submittingArticle = submitEntity_Success(state.submittingArticle);

      const newArticle: Article = action.payload;
      myArticleCreated(
        state,
        newArticle
      );
    },
    submitArticle_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      state.submittingArticle = submitEntity_Failure(state.submittingArticle, action)
    },
    //
    // Moderator decides on an article:
    //
    moderatorDecidesOnArticle_Started(state, action: PayloadAction<void>) {
      state.moderatorSubmittingDecision = submitEntity_Started(state.moderatorSubmittingDecision);
    },
    moderatorDecidesOnArticle_Success(state, action: PayloadAction<ArticlePayload>) {
      state.moderatorSubmittingDecision = submitEntity_Success(state.moderatorSubmittingDecision);

      const newArticle: Article = action.payload.article;
      moderatorArticleDecision(
        state,
        newArticle,
        action.payload.userId!
      );
    },
    moderatorDecidesOnArticle_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      state.moderatorSubmittingDecision = submitEntity_Failure(state.moderatorSubmittingDecision, action);
    },
    //
    // User deletes own article:
    //
    userDeletesOwnArticle_Started(state, action: PayloadAction<void>) {
      state.userDeletesOwnArticle = submitEntity_Started(state.userDeletesOwnArticle);
    },
    userDeletesOwnArticle_Success(state, action: PayloadAction<Article>) {
      state.userDeletesOwnArticle = submitEntity_Success(state.userDeletesOwnArticle);

      const newArticle: Article = action.payload;
      myArticleSoftDeleted(
        state,
        newArticle
      );
    },
    userDeletesOwnArticle_Failure(state, action: PayloadAction<HttpErrorsEnum>) {
      state.userDeletesOwnArticle = submitEntity_Failure(state.userDeletesOwnArticle, action)
    },
    //
    // MyArticleCreated event:
    //
    myArticleCreated(state, action: PayloadAction<Article>) {
      const newArticle: Article = action.payload;
      myArticleCreated(
        state,
        newArticle
      );
    },
    //
    // MyArticleModeratorDecision event:
    //
    myArticleModeratorDecision(state, action: PayloadAction<Article>) {
      const newArticle: Article = action.payload;

      // if Approved, add to publicArticles:
      if (action.payload.articleStateId === ArticleStatesEnum.Approved) {
        state.approvedArticles.entities = addToArticlesArray(
          state.approvedArticles.entities,
          newArticle
        );
      }

      // modify myArticles:
      state.myArticles.entities = addToArticlesArray(
        state.myArticles.entities,
        newArticle
      );
    },
    //
    // MyArticleSoftDeleted event:
    //
    myArticleSoftDeleted(state, action: PayloadAction<Article>) {
      const newArticle: Article = action.payload;
      myArticleSoftDeleted(
        state,
        newArticle
      );
    },
    //
    // PendingArticleCreated event:
    //
    moderatorPendingArticleCreated(state, action: PayloadAction<Article>) {
      const newArticle: Article = action.payload;

      // add to pendingArticles:
      state.pendingArticles.entities = addToArticlesArray(
        state.pendingArticles.entities,
        newArticle
      );
    },
    //
    // ArticleModeratorDecision event:
    //
    moderatorArticleDecision(state, action: PayloadAction<ArticlePayload>) {
      const newArticle: Article = action.payload.article;

      moderatorArticleDecision(
        state,
        newArticle,
        action.payload.userId!
      );
    },
    //
    // ArticleSoftDeleted event:
    //
    moderatorArticleSoftDeleted(state, action: PayloadAction<Article>) {
      const newArticle: Article = action.payload;

      // remove from pendingArticles:
      state.pendingArticles.entities = removeFromArticlesArray(
        state.pendingArticles.entities,
        newArticle
      );

      // remove from publicArticles:
      state.approvedArticles.entities = removeFromArticlesArray(
        state.approvedArticles.entities,
        newArticle
      );

      // remove from myArticles:
      state.myArticles.entities = removeFromArticlesArray(
        state.myArticles.entities,
        newArticle
      );

      // remove from moderatorOwnDecisions:
      state.moderatorOwnDecisions.entities = removeFromArticlesArray(
        state.moderatorOwnDecisions.entities,
        newArticle
      );
    }
  }
});



export const fetchAllApprovedArticles = createAsyncThunk(
  "articles/fetchAllApprovedArticles",
  async (
    arg = undefined,
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.fetchAllApprovedArticles_Started()
    );

    try {
      const articles = await getAllApprovedArticles();
      dispatch(
        articlesSlice.actions.fetchAllApprovedArticles_Success(articles)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.fetchAllApprovedArticles_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export const fetchAllMyArticles = createAsyncThunk(
  "articles/fetchAllMyArticles",
  async (
    arg = undefined,
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.fetchAllMyArticles_Started()
    );

    try {
      const articles = await getAllMyArticles();
      dispatch(
        articlesSlice.actions.fetchAllMyArticles_Success(articles)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.fetchAllMyArticles_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export const moderatorFetchesAllPendingArticles = createAsyncThunk(
  "articles/moderatorFetchesAllPendingArticles",
  async (
    arg = undefined,
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.fetchAllPendingArticles_Started()
    );

    try {
      const pendingArticles = await moderatorGetsAllPendingArticlesHttp();
      dispatch(
        articlesSlice.actions.fetchAllPendingArticles_Success(pendingArticles)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.fetchAllPendingArticles_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export const moderatorFetchesAllOwnDecisions = createAsyncThunk(
  "articles/moderatorFetchesAllOwnDecisions",
  async (
    arg = undefined,
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.fetchAllModeratorOwnDecisions_Started()
    );

    try {
      const ownDecisions = await moderatorGetsAllOwnDecisionsHttp();
      dispatch(
        articlesSlice.actions.fetchAllModeratorOwnDecisions_Success(ownDecisions)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.fetchAllModeratorOwnDecisions_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export const userSubmitsAnArticle = createAsyncThunk(
  "articles/userSubmitsAnArticle",
  async (
    article: ArticleSubmit,
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.submitArticle_Started()
    );

    try {
      const articleReturned = await userSubmitsAnArticle_HttpClient(article);
      dispatch(
        articlesSlice.actions.submitArticle_Success(articleReturned)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.submitArticle_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export const moderatorDecidesOnArticle = createAsyncThunk(
  "articles/moderatorDecidesOnArticle",
  async (
    { articleId, decision, versionId, currentUserId }:
      { articleId: number, decision: ArticleStatesEnum, versionId: number, currentUserId: string },
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.moderatorDecidesOnArticle_Started()
    );

    try {
      const articleSaved: Article = await moderatorMakesDecisionOnPendingArticle(
        articleId,
        decision,
        versionId
      );

      const payload: ArticlePayload = {
        article: articleSaved,
        userId: currentUserId
      }
      dispatch(
        articlesSlice.actions.moderatorDecidesOnArticle_Success(payload)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.moderatorDecidesOnArticle_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export const userDeletesOwnArticle = createAsyncThunk(
  "articles/userDeletesOwnArticle",
  async (
    { articleId, versionId }: { articleId: number, versionId: number },
    { dispatch }
  ) => {
    dispatch(
      articlesSlice.actions.userDeletesOwnArticle_Started()
    );

    try {
      const articleSaved: Article = await userDeletesOwnArticleHttp(
        articleId,
        versionId
      );
      dispatch(
        articlesSlice.actions.userDeletesOwnArticle_Success(articleSaved)
      );
    } catch (error) {
      dispatch(
        articlesSlice.actions.userDeletesOwnArticle_Failure(HttpErrorsEnum.UnknownError)
      );
    }
  }
)



export interface ArticlePayload {
  article: Article,
  userId?: string | undefined,
  userRoles?: UserRolesEnum[] | undefined
}



type articlesSliceState = WritableDraft<{
  approvedArticles: EntitiesArrayShape<Article>;
  myArticles: EntitiesArrayShape<Article>;
  pendingArticles: EntitiesArrayShape<Article>;
  moderatorOwnDecisions: EntitiesArrayShape<Article>;
  submittingArticle: SubmitShape;
  moderatorSubmittingDecision: SubmitShape;
  userDeletesOwnArticle: SubmitShape;
}>;



function myArticleSoftDeleted(
  state: articlesSliceState,
  newArticle: Article
) {
  // remove from myArticles:
  state.myArticles.entities = removeFromArticlesArray(
    state.myArticles.entities,
    newArticle
  );

  // remove from publicArticles:
  state.approvedArticles.entities = removeFromArticlesArray(
    state.approvedArticles.entities,
    newArticle
  );

  // remove from pendingArticles:
  state.pendingArticles.entities = removeFromArticlesArray(
    state.pendingArticles.entities,
    newArticle
  );
}



function moderatorArticleDecision(
  state: articlesSliceState,
  newArticle: Article,
  userId: string
) {
  // remove from pendingArticles:
  state.pendingArticles.entities = removeFromArticlesArray(
    state.pendingArticles.entities,
    newArticle
  );

  // add to moderatorOwnDecisions, if is the decision's moderator:
  if (newArticle.articleStateId_LastChangedBy_ModeratorId === userId) {
    state.moderatorOwnDecisions.entities = addToArticlesArray(
      state.moderatorOwnDecisions.entities,
      newArticle
    );
  }

  // modify the myArticles, if is the article's author.
  if (newArticle.authorId === userId) {
    state.myArticles.entities = addToArticlesArray(
      state.myArticles.entities,
      newArticle
    );
  }

  // if Approved, add to publicArticles:
  if (newArticle.articleStateId === ArticleStatesEnum.Approved) {
    state.approvedArticles.entities = addToArticlesArray(
      state.approvedArticles.entities,
      newArticle
    );
  }
}



function myArticleCreated(
  state: articlesSliceState,
  newArticle: Article
) {
  // add to myArticles:
  state.myArticles.entities = addToArticlesArray(
    state.myArticles.entities,
    newArticle
  );
}