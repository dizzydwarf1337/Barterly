import { PostStatusType } from "../types/postTypes";

export const getStatusColor = (status: PostStatusType) => {
  switch (status) {
    case PostStatusType.Published:
      return "success";
    case PostStatusType.Rejected:
      return "error";
    case PostStatusType.UnderReview:
      return "warning";
    default:
      return "info";
  }
};

export const getStatusText = (status: PostStatusType) => {
  switch (status) {
    case PostStatusType.UnderReview:
      return "Under Review";
    case PostStatusType.Published:
      return "Published";
    case PostStatusType.Rejected:
      return "Rejected";
    case PostStatusType.ReSubmitted:
      return "Re-Submitted";
    case PostStatusType.Deleted:
      return "Deleted";
    default:
      return "Unknown";
  }
};
