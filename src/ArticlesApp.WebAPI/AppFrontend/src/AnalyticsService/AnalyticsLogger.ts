import { LoggerConfiguration } from "src/_AppConfiguration/Sections/LoggerConfiguration";
import { LogLevelsEnum } from "src/_AppConfiguration/Sections/LogLevelsEnum";
import { LogItem, LogsAggregator } from "src/_Logs/LogsAggregator";



interface RouteChangeItem extends LogItem {
  routeChange: string;
}

let loggerConfiguration: LoggerConfiguration = {
  currentLogLevel: LogLevelsEnum.Info,
  batchSize: 25,
  sendTimeoutSec: 15,
  urlPathToBackend: "/api/analytics"
}

const logger = new LogsAggregator<RouteChangeItem>(loggerConfiguration);



export function logRouteChange(route: string) {
  const item: RouteChangeItem = {
    routeChange: route
  };
  logger.addLogItem(item);
}