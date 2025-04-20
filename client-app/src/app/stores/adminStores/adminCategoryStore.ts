import { makeAutoObservable, runInAction } from "mobx";
import agent from "../../API/agent";
import ApiResponse from "../../models/apiResponse";
import async from "react-select/async";
import Category from "../../models/category";


export default class AdminCategoryStore {

    constructor() {
        makeAutoObservable(this);
    }

    openModal: boolean = false;
    setOpenModal = (value: boolean) => {
        this.openModal = value;
    }
    getOpenModal = () => {
        this.openModal;
    }

    category: Category | null | undefined;
    setCategory = (category: Category | null) => {
        this.category = category;
    }
    getCategory = () => this.category;

    openModalFunc = (category: Category | null) => {
        this.setOpenModal(true);
        this.setCategory(category);
    }

    deleteCategory = async (id: string) => {
        try {
            await agent.Categories.DeleteCategory(id)
        }
        catch {
            console.error("Error while deleting category");
            throw new Error("Error while deleting category");
        }
    }
    addCategory = async (category: Category) => {
        try {
            await agent.Categories.AddCategory(category);
        }
        catch (error) {
            console.error(error);
            throw new Error(error);
        }
    }

    editCategory = async (category: Category) => {
        try {
            await agent.Categories.EditCategory(category);
        }
        catch (error) {
            console.error(error);
            throw new Error(error.toString());
        }
    }

    
}