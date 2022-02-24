import { Article } from "src/Entities/Article";
import { List } from "../List";
import { CardComponent } from "./_components/CardComponent";



interface Props {
  title: string,
  articles: Article[],
  showArticleStatus: boolean,
  articleRouteBase: string
}

export function ArticlesList(props: Props) {
  return (
    <List
      title={props.title}
      entities={props.articles}
      render={(ownProps) => (
        <CardComponent
          entity={ownProps.entity}
          showArticleStatus={props.showArticleStatus}
          articleRouteBase={props.articleRouteBase}
        />
      )}
    />
  )
}