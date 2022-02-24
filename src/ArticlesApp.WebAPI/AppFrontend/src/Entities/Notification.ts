import { NotificationTypesEnum } from "src/Entities/NotificationTypesEnum";



export class Notification {
  constructor(notif: Notification) {
    this.id = notif.id;
    this.message = notif.message;
    this.reciever_UserId = notif.reciever_UserId;
    this.createdAt_DateUtc = notif.createdAt_DateUtc;
    // this.readAt_DateUtc = notif.readAt_DateUtc;
    // this.referencedArticle_ArticleId = notif.referencedArticle_ArticleId;
    // this.notificationType_TypeId = notif.notificationType_TypeId;
  }

  id: number;

  message: string;

  reciever_UserId: string;

  createdAt_DateUtc: Date;

  readAt_DateUtc?: Date;

  referencedArticle_ArticleId?: number;

  notificationType_TypeId?: NotificationTypesEnum;
}