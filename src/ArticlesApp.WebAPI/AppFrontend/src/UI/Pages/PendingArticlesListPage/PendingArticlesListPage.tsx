import { useSelector } from "react-redux";
import { storeType } from "src/Store/store";
import { useLoginPrompt } from "UI/_components/LoginPromptProvider";
import { Routes } from "UI/_components/Routing/Routes";
import { ArticlesList } from "UI/Pages/_components/ArticlesList/ArticlesList";



export function PendingArticlesListPage() {
  useLoginPrompt();

  let articles = useSelector((state: storeType) => state.articles.pendingArticles.entities);
  if (articles === undefined) {
    articles = [];
  }

  return (
    <ArticlesList
      showArticleStatus={true}
      title={"Pending decisions"}
      articles={articles}
      articleRouteBase={Routes.articles.pending.root}
    />
  )
}