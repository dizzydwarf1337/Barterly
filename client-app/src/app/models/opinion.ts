export default interface Opinion {
    id: string,
    authorId: string,
    content: string,
    isHidden: boolean,
    createdAt: Date,
    lastUpdatedAt: Date | null,
    rate: number | null,
    subjectId: string,
}