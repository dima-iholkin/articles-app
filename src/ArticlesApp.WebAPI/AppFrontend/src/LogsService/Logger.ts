import { loggerConfiguration } from "src/_AppConfiguration/AppConfiguration";
import { LogLevelsEnum } from "src/_AppConfiguration/Sections/LogLevelsEnum";
import { LogsAggregator } from "src/_Logs/LogsAggregator";



interface LogItem {
  message: string,
  stack?: string,
  logLevel: LogLevelsEnum
}

const logger = new LogsAggregator<LogItem>(loggerConfiguration);



// const originalConsoleLog = console.log;
// console.log = (str: string, ...data: any[]) => {
//   originalConsoleLog(str, ...data);

//   if (LogLevelsEnum.Info < loggerConfiguration.currentLogLevel) {
//     return;
//   } else {
//     logger.addLogItem({
//       message: str,
//       logLevel: LogLevelsEnum.Log
//     });
//   }
// }

const originalConsoleInfo = console.info;
console.info = (str: string, ...data: any[]) => {
  originalConsoleInfo(str, ...data);

  if (LogLevelsEnum.Info < loggerConfiguration.currentLogLevel) {
    return;
  } else {
    logger.addLogItem({
      message: str,
      logLevel: LogLevelsEnum.Info
    });
  }
}

const originalConsoleWarn = console.warn;
console.warn = (str: string, ...data: any[]) => {
  originalConsoleWarn(str, ...data);

  let obj: { stack?: string } = {};
  Error.captureStackTrace(obj);
  // creates a stack property on the provided object.

  let stack: string[] = obj.stack!.split("\n").map((line) => line.trim());
  // turn the string into an array of lines.

  stack.splice(0, stack[0] === "Error" ? 2 : 1);
  // remove the first 2 redundant lines: "Error" and "at console.warn".

  const stackString: string = stack.join("\n");
  // turn the array into a single single again.

  if (LogLevelsEnum.Warn < loggerConfiguration.currentLogLevel) {
    return;
  } else {
    logger.addLogItem({
      message: str,
      stack: stackString,
      logLevel: LogLevelsEnum.Warn
    });
  }
}



export function logError(message: string, stack?: string) {
  if (LogLevelsEnum.Error < loggerConfiguration.currentLogLevel) {
    return;
  } else {
    logger.addLogItem({
      message: message,
      stack: stack,
      logLevel: LogLevelsEnum.Error
    });
  }
}



export function startLogging() { }