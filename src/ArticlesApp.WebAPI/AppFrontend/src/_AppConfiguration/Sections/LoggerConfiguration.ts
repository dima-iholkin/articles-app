import { LogLevelsEnum } from "./LogLevelsEnum";



export interface LoggerConfiguration {
  currentLogLevel: LogLevelsEnum,
  batchSize: number,
  sendTimeoutSec: number,
  urlPathToBackend: string
}