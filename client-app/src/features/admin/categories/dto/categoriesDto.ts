
export interface AddCategoryRequestDto {
    namePl:string,
    nameEn:string,
    description?:string,
    subCategories: {namePl:string,nameEn:string}[]
}

export interface UpdateCategoryRequestDto {
    id:string,
    namePl:string,
    nameEn:string,
    description?:string,
    subCategories: {id?:string,namePl:string,nameEn:string}[]
}