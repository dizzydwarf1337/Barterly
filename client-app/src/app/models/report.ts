import {ReportStatusType} from "../enums/reportStatusType";

export default interface Report {
    id: string,
    message: string,
    createdAt: Date,
    status: ReportStatusType,
    authorId: string,
    reportedSubjectId: string,
}