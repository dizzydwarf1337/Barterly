export interface PostSettings {
  id: string;
  isHidden: boolean;
  isDeleted: boolean;
  postStatusType: PostStatusType;
  rejectionMessage?: string;
}

export interface PostOpinion {
  id: string;
  authorId: string;
  content: string;
  isHidden: string;
  createdAt: string;
  lastUpdatedAt: string;
  rate?: number;
}

export enum PostStatusType {
  UnderReview = 0,
  Published = 1,
  Rejected = 2,
  ReSubmitted = 3,
  Deleted = 4,
}

export interface OwnerData {
  ownerId: string;
  firstName: string;
  lastName: string;
}
