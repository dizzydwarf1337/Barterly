

export interface Category {
    id:string;
    nameEN:string;
    namePL:string;
    description:string;
    subCategories:SubCategory[];
}

export interface SubCategory {
    id:string;
    titleEN:string;
    titlePL:string;
}