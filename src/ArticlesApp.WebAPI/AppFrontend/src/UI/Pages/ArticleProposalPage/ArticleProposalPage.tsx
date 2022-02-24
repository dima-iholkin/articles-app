import { useLayoutEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { push } from "connected-react-router";
import { Button, styled, TextField } from '@material-ui/core';
import { ArticleSubmit } from 'src/Entities/Article';
import { userSubmitsAnArticle } from 'src/Store/reducers/articlesSlice';
import { storeType, useAppDispatch } from 'src/Store/store';
import { useLoginPrompt } from "../../_components/LoginPromptProvider";
import { setLeftButtonValue } from "../../_components/NavBar/_components/LeftCorner/LeftCorner";
import { NavbarLeftButtonValuesEnum } from '../../_components/NavBar/_components/LeftCorner/NavbarLeftButtonValuesEnum';
import { Routes } from '../../_components/Routing/Routes';
import { addMessage } from "../../_components/SnackbarProvider";
import { PageTitle } from '../_components/PageTitle';
import { validateTheField } from "./validation";



interface Props {
  proposalType: "edit" | "add" | "delete";
  articleId?: number;
}

export function ArticleProposalPage(props: Props) {
  useLayoutEffect(() => {
    setLeftButtonValue(NavbarLeftButtonValuesEnum.Cancel);
    return () => {
      setLeftButtonValue(NavbarLeftButtonValuesEnum.Default);
    };
  });

  const user = useSelector((state: storeType) => state.oidc.user);
  useLoginPrompt();

  const dispatch = useAppDispatch();

  const titleRef = useRef<HTMLInputElement>();
  const textRef = useRef<HTMLTextAreaElement>();

  const [validationSuccess, setValidationSuccess] = useState({
    title: true,
    text: true
  });

  const handleSubmit = () => {
    const titleField = titleRef.current?.value;
    const titleFieldValidated = validateTheField(titleField);
    // check the title field.

    const textField = textRef.current?.value;
    const textFieldValidated = validateTheField(textField);
    // check the text field.

    if (titleFieldValidated && textFieldValidated) {
      const article = {
        title: titleField,
        text: textField
      } as ArticleSubmit;
      // gather the values from the textfields.

      dispatch(userSubmitsAnArticle(article));
      // userSubmitsAnArticle(article);
      // store.dispatch(userSubmitsAnArticle(article) as any);
      // try to send the new article to the server + get the succesful response.

      dispatch(push(Routes.articles.my.root));
      // after the save, return to the list of all articles. 

      addMessage("Article was submitted.");
    } else {
      setValidationSuccess({
        title: titleFieldValidated,
        text: textFieldValidated
      });
    }
  }

  return (
    <Div>
      <PageTitle>Add article</PageTitle>
      <Form>
        <TextField
          id="title"
          label="title"
          fullWidth
          inputRef={titleRef}
          disabled={!user}
          style={{
            marginBottom: "10px"
          }}
          error={!validationSuccess.title}
          helperText={validationSuccess.title === false && "This field cannot be empty."}
          onKeyDown={(event) => {
            if (event.key === "Enter") {
              event.preventDefault()
              textRef.current?.focus();
            }
          }}
        />
        <TextField
          multiline id="text"
          label="text"
          fullWidth
          inputRef={textRef}
          disabled={!user}
          error={!validationSuccess.text}
          helperText={validationSuccess.text === false && "This field cannot be empty."}
        />
      </Form>
      <Button
        variant="contained"
        onClick={handleSubmit}
        disabled={!user}
        style={{
          alignSelf: "flex-end",
          margin: "20px 20px 20px 0"
        }}
      >
        Submit
      </Button>
    </Div>
  )
}



const Div = styled("div")({
  backgroundColor: "white",
  display: "flex",
  flexDirection: "column",
  flexGrow: 1
})

const Form = styled("form")({
  margin: "0 20px 20px 20px"
})