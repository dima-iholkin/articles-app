export const Routes = {
  root: "/",
  articles: {
    root: "/articles",
    my: {
      root: "/articles/my",
      articleId: "/articles/my/:articleId",
      decisions: "/articles/my/decisions",
    },
    pending: {
      root: "/articles/pending",
      articleId: "/articles/pending/:articleId",
    },
    articleId: "/articles/:articleId",
    add: "/articles/add",
    edit: "/articles/:articleId/edit",
    editWithId: (articleId: number) => "/articles/" + articleId + "/edit",
    delete: "/articles/:articleId/delete",
    deleteWithId: (articleId: number) => "/articles/" + articleId + "/delete",
  },
  notifications: "/notifications",
  identityAccountManage: "/Identity/Account/Manage",
  identityRegister: "/Identity/Account/Register"
}