export const routes = {
  getAllApprovedArticles: "/api/articles",
  articlesWithId: (articleId: number) => `/api/articles/${articleId}`,
  getAllMyArticles: "/api/articles/my",
  userSubmitsAnArticle: "/api/articles",
  moderatorGetsAllPendingArticles: "/api/articles/pending",
  moderatorGetsPendingArticleById: (articleId: number) => `/api/articles/pending/${articleId}`,
  moderatorGetsAllOwnDecisions: "/api/articles/my/decisions",
  moderatorMakesADecisionOnPendingArticle: (articleId: number) => `/api/articles/${articleId}`,
  getAllNotificationsForUser: "/api/notifications",
  getNewNotificationsForUser: (lastFetchedNotifId: number) => `/api/notifications/new/${lastFetchedNotifId}`,
  markNotificationAsRead: (notificationId: number) => `/api/notifications/${notificationId}`,
};