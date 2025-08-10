import { SubCategory } from "../../categories/types/categoryTypes";

export enum ContractType {
  "Employment Contract" = 0,
  "Specific Work Contract" = 1,
  "Task Contract" = 2,
  "B2B" = 3,
  "Volunteer Contract" = 4,
}

export enum PostCurrency {
  "$" = 0,
  "€" = 1,
  "zł" = 2,
}

export enum PostPriceType {
  "Onetime Payment" = 0,
  "Per Hour" = 1,
  "Per Day" = 2,
  "Per Month" = 3,
  "Free" = 4,
  "Per Item" = 5,
}

export enum PostPromotionType {
  None = 0,
  Highlight = 1,
  Top = 2,
}

export enum PostType {
  Common = "Common",
  Work = "Work",
  Rent = "Rent",
}

export enum RentObjectType {
  "House" = 0,
  "Apartment" = 1,
  "Studio" = 2,
  "Room" = 3,
  "WareHouse" = 4,
  "Office" = 5,
  "Other" = 6,
}

export enum WorkloadType {
  "Full-Time" = 0,
  "Part-Time" = 1,
  "Freelance" = 2,
  "Internship" = 3,
  "Shift" = 4,
  "Seasonal" = 5,
  "Other" = 6,
}

export enum WorkLocationType {
  "OnSite" = 0,
  "Remote" = 1,
  "Hybrid" = 2,
}

export interface PostPreview {
  id: string;
  subCategoryId: string;
  ownerId: string;
  title: string;
  city?: string | null;
  street?: string | null;
  houseNumber?: string | null;
  shortDescription: string | null;
  price?: number | null;
  minSalary?: number | null;
  maxSalary?: number | null;
  priceType: PostPriceType;
  mainImageUrl?: string | null;
  postPromotionType: PostPromotionType;
  workload?: WorkloadType | null;
  workLocation?: WorkLocationType | null;
  contract?: ContractType | null;
  experienceRequired?: boolean | null;
  rentObjectType?: RentObjectType | null;
  currency?: PostCurrency | null;
  numberOfRooms?: number | null;
  area?: number | null;
  floor?: number | null;
  postType: string;
  ownerName:string;
  
}

export interface PostDetails extends PostPreview {
  fullDescription: string;
  tags?: string[] | null;

  region?: string | null;
  viewsCount?: number | null;
  country?: string | null;
  street?: string | null;
  houseNumber?: string | null;

  createdAt: string;
  updatedAt?: string | null;

  buildingNumber?: string | null;

  subCategory?: SubCategory | null;
  postImages?: PostImage[] | null;
}

export interface PostImage {
  id: string;
  imageUrl?: string | null;
}

export interface PostImagesModal {
  postId: string;
  mainImageUrl: string;
  secondaryImagesUrl: string[];
}
