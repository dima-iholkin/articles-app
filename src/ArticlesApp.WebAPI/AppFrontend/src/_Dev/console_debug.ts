export function console_debug(text: string) {
  // const styles = "color: #E8810C;";
  const styles = "color: orange";

  return console.debug.bind(window.console, `%c${text}`, styles);
}

// example:
// debugga("hello me")();