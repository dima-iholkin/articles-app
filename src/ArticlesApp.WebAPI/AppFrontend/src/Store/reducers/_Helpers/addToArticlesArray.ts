import { WritableDraft } from "immer/dist/internal";
import { Article } from "src/Entities/Article";



export function addToArticlesArray(
  articles: WritableDraft<Article>[],
  newArticle: Article
) {
  const foundIndex = articles.findIndex(ar => ar.id === newArticle.id);

  if (foundIndex === -1) {
    articles.push(newArticle);
  } else {
    if (newArticle.versionId > articles[foundIndex].versionId) {
      articles[foundIndex] = newArticle;
      // in an unlikely case, that the article is already in the array and has a newer versionId.
    }
  }

  return articles;
}