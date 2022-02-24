import { fetchAllApprovedArticles } from "./reducers/articlesSlice";
import { loadUserOnStartup } from "./Startup/loadUserOnStartup";
import { store } from "./store";



export function storeStartup(_store: typeof store) {
  _store.dispatch(fetchAllApprovedArticles());

  loadUserOnStartup(_store);
}