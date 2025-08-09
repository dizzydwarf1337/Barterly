import {makeAutoObservable} from "mobx";
import agent from "../../API/agent";
import Category from "../../models/category";


export default class AdminCategoryStore {

    openModal: boolean = false;
    category: Category | null | undefined;

    constructor() {
        makeAutoObservable(this);
    }

    setOpenModal = (value: boolean) => {
        this.openModal = value;
    }

    getOpenModal = () => {
        this.openModal;
    }

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
        } catch {
            console.error("Error while deleting category");
            throw new Error("Error while deleting category");
        }
    }
    addCategory = async (category: Category) => {
        try {
            await agent.Categories.AddCategory(category);
        } catch (error) {
            console.error(error);
            throw new Error(error);
        }
    }

    editCategory = async (category: Category) => {
        try {
            await agent.Categories.EditCategory(category);
        } catch (error) {
            console.error(error);
            throw new Error(error.toString());
        }
    }


}