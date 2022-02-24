import { ArticleStatesEnum } from "src/Entities/ArticleStatesEnum";



export class Article {
  constructor(ar: Article) {
    this.id = ar.id;
    this.title = ar.title;
    this.text = ar.text;
    this.submittedAt_DateUtc = ar.submittedAt_DateUtc;
    this.authorId = ar.authorId;
    this.articleStateId = ar.articleStateId;
    this.articleStateId_LastChangedAt_DateUtc = ar.articleStateId_LastChangedAt_DateUtc;
    this.articleStateId_LastChangedBy_ModeratorId = ar.articleStateId_LastChangedBy_ModeratorId;
    this.softDeletedAt_DateUtc = ar.softDeletedAt_DateUtc;
    this.versionId = ar.versionId;
  }

  id: number;

  title: string;
  text: string;

  submittedAt_DateUtc: Date;

  authorId: string;

  articleStateId: ArticleStatesEnum;

  articleStateId_LastChangedAt_DateUtc?: Date;
  articleStateId_LastChangedBy_ModeratorId?: string;

  softDeletedAt_DateUtc?: Date;

  versionId: number;
}



export type ArticleSubmit = Pick<Article, "title" | "text">;