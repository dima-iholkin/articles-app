import { useSelector } from "react-redux";
import { storeType } from "src/Store/store";
import { Routes } from "UI/_components/Routing/Routes";
import { useFab } from "UI/_components/SnackbarProvider";
import { ArticlesList } from "UI/Pages/_components/ArticlesList/ArticlesList";
import { FabAddArticle } from "UI/Pages/_components/FabAddArticle";



export function ArticlesListPage() {
  useFab();

  let articles = useSelector((state: storeType) => state.articles.approvedArticles.entities);
  if (articles === undefined) {
    articles = [];
  }

  return (
    <>
      <ArticlesList
        showArticleStatus={false}
        title={"Everyone's articles"}
        articles={articles}
        articleRouteBase={Routes.articles.root}
      />
      <FabAddArticle />
    </>
  )
}