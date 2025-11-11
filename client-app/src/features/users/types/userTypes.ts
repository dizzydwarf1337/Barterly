import { PostPreview } from "../../posts/types/postTypes";

export interface PostOwner {
  id:string;
  firstName:string;
  lastName:string;
  createdAt:Date;
  profilePicturePath?:string | null;
  posts: PostPreview[]
}