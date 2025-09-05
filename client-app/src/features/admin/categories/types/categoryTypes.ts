

export interface Category {
    id:string,
    nameEn: string,
    namePl: string,
    description: string,
    subCategories: SubCategory[],
}

export interface SubCategory {
    id:string,
    nameEn: string,
    namePl: string,
}