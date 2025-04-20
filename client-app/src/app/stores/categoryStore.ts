import { makeAutoObservable, runInAction } from "mobx";
import Category from "../models/category";
import agent from "../API/agent";
import ApiResponse from "../models/apiResponse";


export default class CategoryStore {
    static setCategories(arg0: any[]) {
        throw new Error("Method not implemented.");
    }

    constructor() {
        makeAutoObservable(this);
        this.getCategoriesApi();
    }

    categories: Category[] | null;

    setCategories = (categories: Category[] | null) => this.categories = categories;
    getCategories = () => this.categories;
    

    getCategoriesApi = async () => {
        this.setCategoryLoading(true);
        try {
            const res :ApiResponse<Category[]> = await agent.Categories.GetCategories();
            if(res)
            runInAction(async () => {
                this.setCategories(res.value);
            })
        }
        catch {
            console.error("Error while getting categories");
            throw new Error("Error while getting categories");
        }
        finally {
            this.setCategoryLoading(false);
        }
    }

    categoryLoading: boolean = false;

    getCategoryLoading = () => this.categoryLoading;

    setCategoryLoading = (value: boolean) => this.categoryLoading = value;


}