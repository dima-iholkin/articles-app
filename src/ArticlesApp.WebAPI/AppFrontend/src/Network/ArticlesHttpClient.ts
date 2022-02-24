import axios, { AxiosResponse } from "axios";
import { Article, ArticleSubmit } from "src/Entities/Article";
import { ArticleStatesEnum } from "src/Entities/ArticleStatesEnum";
import { getAuthorizationHeader } from "./_Helpers/getAuthorizationHeader";
import { routes } from "./routes";



export async function getAllApprovedArticles(): Promise<Article[]> {
  const response = await axios.get(routes.getAllApprovedArticles);
  return response.data;
  // GET: api/articles
}



export async function getAllMyArticles(): Promise<Article[]> {
  const headers = await getAuthorizationHeader();
  const response = await axios.get(
    routes.getAllMyArticles,
    headers
  );
  return response.data;
  // GET: api/articles/my
}



export async function getApprovedArticleById(articleId: number): Promise<Article> {
  const response = await axios.get(routes.articlesWithId(articleId));
  return response.data;
  // GET: api/articles/5
}



export async function moderatorGetsAllOwnDecisions(): Promise<Article[]> {
  const header = await getAuthorizationHeader();
  const response: AxiosResponse<Article[]> = await axios.get(
    routes.moderatorGetsAllOwnDecisions,
    header
  );

  return response.data;
  // GET: api/articles/my/decisions
}



export async function moderatorGetsAllPendingArticles(): Promise<Article[]> {
  const header = await getAuthorizationHeader();
  const response: AxiosResponse<Article[]> = await axios.get(
    routes.moderatorGetsAllPendingArticles,
    header
  );
  return response.data;
  // GET: api/articles/pending
}



export async function moderatorGetsPendingArticleById(articleId: number): Promise<Article> {
  const header = await getAuthorizationHeader();
  const response: AxiosResponse<Article> = await axios.get(
    routes.moderatorGetsPendingArticleById(articleId),
    header
  );
  return response.data;
  // GET: api/articles/pending/5
}



export async function moderatorMakesDecisionOnPendingArticle(
  articleId: number,
  decision: ArticleStatesEnum,
  versionId: number
): Promise<Article> {
  const header = await getAuthorizationHeader();
  const config = {
    ...header,
    params: {
      decision: ArticleStatesEnum[decision],
      versionId: versionId
    }
  };

  const response: AxiosResponse<Article> = await axios.put(
    routes.moderatorMakesADecisionOnPendingArticle(articleId),
    undefined,
    config
  );
  return response.data;
  // PUT: api/articles/5?decision=approved&versionId=1
}



export async function userDeletesOwnArticle(
  articleId: number,
  versionId: number
): Promise<Article> {
  const header = await getAuthorizationHeader();
  const config = {
    ...header,
    params: {
      versionId: versionId
    }
  };

  const response: AxiosResponse<Article> = await axios.delete(
    routes.articlesWithId(articleId),
    config
  );
  return response.data;
  // DELETE: api/articles/5?versionId=1
}



export async function userSubmitsAnArticle(article: ArticleSubmit): Promise<Article> {
  const header = await getAuthorizationHeader();
  const response: AxiosResponse<Article> = await axios.post(
    routes.userSubmitsAnArticle,
    article,
    header
  );
  return response.data;
  // POST: api/articles
  // body: { ArticleSubmit }
}