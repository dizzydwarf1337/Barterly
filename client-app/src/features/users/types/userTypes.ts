import { PostPreview } from "../../posts/types/postTypes";

export interface PostOwner {
  id:string;
  firstName:string;
  lastName:string;
  createdAt:Date;
  profilePicturePath?:string | null;
  posts: PostPreview[]
}

export interface UserData {
  id: string;
  firstName: string;
  lastName: string;
  bio?: string | null;
  country?: string | null;
  city?: string | null;
  street?: string | null;
  houseNumber?: string | null;
  postalCode?: string | null;
  profilePicturePath?: string | null;
}