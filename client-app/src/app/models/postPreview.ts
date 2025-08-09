import {ContractType} from "../enums/contractType";
import {PostCurrency} from "../enums/postCurrency";
import {PostPriceType} from "../enums/postPriceType";
import {PostPromotionType} from "../enums/postPromotionType";
import {PostType} from "../enums/postType";
import {RentObjectType} from "../enums/rentObjectType";
import {WorkloadType} from "../enums/workLoadType";
import {WorkLocationType} from "../enums/WorkLocationType";

export interface PostPreview {
    id: string;
    subCategoryId: string;
    ownerId: string;
    title: string | null;
    city: string | null;
    street: string | null;
    houseNumber: string | null;
    shortDescription: string | null;
    price: number | null;
    minSalary: number | null;
    maxSalary: number | null;
    postType: PostType | null;
    priceType: PostPriceType | null;
    mainImageUrl: string | null;
    postPromotionType: PostPromotionType;
    workload: WorkloadType | null;
    workLocation: WorkLocationType | null;
    contract: ContractType | null;
    experienceRequired: boolean | null;
    rentObjectType: RentObjectType | null;
    currency: PostCurrency | null;
    numberOfRooms: number | null;
    area: number | null;
    floor: number | null;
}