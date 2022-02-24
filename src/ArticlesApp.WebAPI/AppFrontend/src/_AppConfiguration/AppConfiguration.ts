import { LoggerConfiguration } from "./Sections/LoggerConfiguration";
import { LogLevelsEnum } from "./Sections/LogLevelsEnum";



export const loggerConfiguration: LoggerConfiguration = {
  currentLogLevel: LogLevelsEnum.Warn,
  batchSize: 25,
  sendTimeoutSec: 15,
  urlPathToBackend: "/api/logs"
}