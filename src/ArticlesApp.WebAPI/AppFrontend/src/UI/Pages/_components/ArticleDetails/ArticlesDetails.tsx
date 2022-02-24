import { ReactNode } from 'react';
import { useSelector } from 'react-redux';
import { styled } from "@material-ui/core/styles";
import { Article } from "src/Entities/Article";
import { storeType } from 'src/Store/store';
import { ItemMenu } from "./ItemMenu";



interface Props {
  article?: Article,
  titleChildren?: ReactNode,
  belowTextChildren?: ReactNode
}

export function ArticleDetails(props: Props) {
  let userId = useSelector((state: storeType) => state.oidc.user?.profile.sub);

  if (props.article === undefined) {
    return (
      <div>
        <p>Article not found.</p>
      </div>
    )
  }

  let showItemMenu: JSX.Element | undefined;
  if (
    props.article.authorId !== undefined
    && userId !== undefined
    && props.article.authorId === userId
  ) {
    showItemMenu = (
      <ItemMenu
        articleId={props.article.id}
        versionId={props.article.versionId}
      />
    );
  } else {
    showItemMenu = undefined;
  }

  return (
    <Div>
      <TitleRow>
        {props.titleChildren}
        {showItemMenu}
      </TitleRow>
      <Text>
        {props.article.text}
      </Text>
      {props.belowTextChildren}
    </Div>
  )
}



const Div = styled("div")({
  backgroundColor: "white",
  flexGrow: 1,
})

const TitleRow = styled("div")({
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  alignItems: "center"
})

const Text = styled("p")({
  textAlign: "left",
  padding: "0 16px 0 16px",
  margin: "0",
  fontFamily: "'Lora', serif",
  fontSize: "1rem"
})