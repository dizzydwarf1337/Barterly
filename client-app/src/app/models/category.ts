import SubCategory from "./subCategory";


export default interface Category {
    id: string,
    namePL: string,
    nameEN:string,
    description: string,
    subCategories?: SubCategory[],
}