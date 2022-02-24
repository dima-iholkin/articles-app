export function clearStaleOidcTokens() {
  let keysToRemove: string[] = [];

  for (let [key, value] of Object.entries(localStorage)) {
    if (key.startsWith("oidc")) {
      const oidcToken = JSON.parse(value);
      const dateCreatedMills = new Date(oidcToken.created * 1000).getTime();
      const dateNowMills = Date.now();
      const timeElapsedSeconds = (dateNowMills - dateCreatedMills) / 1000;
      if (timeElapsedSeconds > 30) {
        keysToRemove.push(key);
      }
    }
  }

  keysToRemove.forEach((key) => {
    localStorage.removeItem(key);
  });
}