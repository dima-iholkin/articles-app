import { LoggerConfiguration } from "src/_AppConfiguration/Sections/LoggerConfiguration";
import { LogLevelsEnum } from "src/_AppConfiguration/Sections/LogLevelsEnum";



export interface LogItem {
  logLevel?: LogLevelsEnum;
}



export class LogsAggregator<T extends LogItem> {
  private logsQueue: Array<T>;
  private nextEmptyIndexInQueue: number;

  private timeoutID: ReturnType<typeof setTimeout> | undefined;
  private loggerConfiguration: LoggerConfiguration;

  constructor(loggerConfig: LoggerConfiguration) {
    this.logsQueue = new Array<T>(25);
    this.nextEmptyIndexInQueue = 0;
    this.loggerConfiguration = loggerConfig;

    window.addEventListener(
      "beforeunload",
      () => {
        const firstEmptyIndex: number | undefined = this.logsQueue.findIndex(li => li === undefined);
        // as the queue might not be full, find where the populated spots end.

        if (firstEmptyIndex !== 0) {
          this.sendLogs(this.logsQueue);
        }
      }
    );
    // send the logs before the tab closes.
  }



  addLogItem(item: T) {
    if (
      item.logLevel !== undefined
      && item.logLevel < this.loggerConfiguration.currentLogLevel
    ) {
      return;
    }
    // ignore the messages below the configured log level.

    this.logsQueue[this.nextEmptyIndexInQueue] = item;

    if (this.nextEmptyIndexInQueue === this.loggerConfiguration.batchSize) {
      this.sendLogs(this.logsQueue);
      return;
      // if the index just written equals the batch size, this means the queue is full,
      // and should be send to the backend.
      // And the logs queue should be emptied.
    } else {
      this.nextEmptyIndexInQueue++;
    }

    if (this.timeoutID === undefined || this.timeoutID === null) {
      this.timeoutID = setTimeout(
        () => this.sendLogs(this.logsQueue),
        this.loggerConfiguration.sendTimeoutSec * 1000
      );
      // if next call is not scheduled, set it to the configured sendTimeout duration.
    } else {
      // if the next call is scheduled, do nothing here.
    }
  }



  private sendLogs(logsQueue: LogItem[]) {
    let firstEmptyIndex: number | undefined = logsQueue.findIndex(li => li === undefined);
    // as the queue might not be full, find where the populated spots end.

    if (firstEmptyIndex === 0) {
      throw new Error("Array should contain not-empty values.");
    }

    if (firstEmptyIndex === -1) {
      firstEmptyIndex = undefined;
    }
    // -1 means the array is full, and therefore all the elements have to be copied below.

    let copiedQueueToSend: LogItem[] = logsQueue.slice(0, firstEmptyIndex);
    // the new array should contain only the populated spots. No empty elements. 
    // Therefore the size may variate. 

    fetch(
      this.loggerConfiguration.urlPathToBackend,
      {
        method: "post",
        body: JSON.stringify(copiedQueueToSend),
        headers: {
          'Content-Type': 'application/json'
        }
      }
    );

    logsQueue.forEach((li, index) => logsQueue[index] = undefined!)
    // clear the original array for the next batch.
    this.nextEmptyIndexInQueue = 0;
    // start over the index counter from 0, for the next batch.

    // lastSentTimestamp = Date.now();
    clearTimeout(this.timeoutID!);
    this.timeoutID = undefined;
  }
}