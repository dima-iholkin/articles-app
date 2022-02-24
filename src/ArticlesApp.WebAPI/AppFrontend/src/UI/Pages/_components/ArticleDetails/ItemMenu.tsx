import React from 'react';
import { useHistory } from 'react-router-dom';
import { MdMoreVert } from "react-icons/md";
import { Menu, MenuItem } from '@material-ui/core';
import { userDeletesOwnArticle } from 'src/Store/reducers/articlesSlice';
import { store } from 'src/Store/store';
import { Routes } from 'src/UI/_components/Routing/Routes';
import { addMessage } from "src/UI/_components/SnackbarProvider";



interface Props {
  articleId: number;
  versionId: number;
}

export function ItemMenu(props: Props) {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  }

  const handleClose = () => {
    setAnchorEl(null);
  }

  const handleDeleteClick = (routeChange: string) => {
    handleClose();
    store.dispatch(userDeletesOwnArticle({
      articleId: props.articleId,
      versionId: props.versionId
    }));
    addMessage("Delete was submitted.");
    history.push(routeChange);
  }

  const history = useHistory();

  return (
    <div style={{ padding: "16px 8px 0 0" }}>
      <div
        aria-controls="simple-menu"
        aria-haspopup="true"
        onClick={handleClick}
      >
        <MdMoreVert size={iconSize} />
      </div>
      <Menu
        id="simple-menu"
        anchorEl={anchorEl}
        keepMounted
        open={Boolean(anchorEl)}
        onClose={handleClose}
      >
        <MenuItem
          key={4}
          onClick={() => handleDeleteClick(Routes.articles.my.root)}
        >
          Delete article
        </MenuItem>
      </Menu>
    </div>
  )
}



const iconSize: number = 32;