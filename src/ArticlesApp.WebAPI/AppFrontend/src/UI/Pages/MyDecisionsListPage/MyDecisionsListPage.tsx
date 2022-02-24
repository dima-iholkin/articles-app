import { useSelector } from "react-redux";
import { storeType } from "src/Store/store";
import { useLoginPrompt } from "src/UI/_components/LoginPromptProvider";
import { Routes } from "src/UI/_components/Routing/Routes";
import { ArticlesList } from "src/UI/Pages/_components/ArticlesList/ArticlesList";



export function MyDecisionsListPage() {
  useLoginPrompt();

  let articles = useSelector((state: storeType) => state.articles.moderatorOwnDecisions.entities);
  if (articles === undefined) {
    articles = [];
  }

  return (
    <ArticlesList
      showArticleStatus={true}
      title={"My decisions"}
      articles={articles}
      articleRouteBase={Routes.articles.root}
    />
  )
}