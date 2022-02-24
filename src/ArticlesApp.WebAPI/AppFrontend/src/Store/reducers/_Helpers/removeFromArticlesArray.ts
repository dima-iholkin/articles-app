import { WritableDraft } from "immer/dist/internal";
import { Article } from "src/Entities/Article";



export function removeFromArticlesArray(
  articles: WritableDraft<Article>[],
  newArticle: Article
) {
  let oldArticle = articles.find(ar => ar.id === newArticle.id);
  if (
    oldArticle !== undefined
    && newArticle.versionId > oldArticle.versionId
  ) {
    articles = articles.filter(ar => ar.id !== newArticle.id);
  }

  return articles;
}