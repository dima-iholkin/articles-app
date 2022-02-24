import { useSelector } from "react-redux";
import { storeType } from "src/Store/store";
import { useLoginPrompt } from "src/UI/_components/LoginPromptProvider";
import { Routes } from "src/UI/_components/Routing/Routes";
import { ArticlesList } from "src/UI/Pages/_components/ArticlesList/ArticlesList";
import { FabAddArticle } from "UI/Pages/_components/FabAddArticle"



export function MyArticlesListPage() {
  useLoginPrompt();

  let articles = useSelector((state: storeType) => state.articles.myArticles.entities);
  if (articles === undefined) {
    articles = [];
  }

  return (
    <>
      <ArticlesList
        showArticleStatus={true}
        title={"My articles"}
        articles={articles}
        articleRouteBase={Routes.articles.my.root}
      />
      <FabAddArticle />
    </>
  )
}