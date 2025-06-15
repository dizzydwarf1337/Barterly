import { makeAutoObservable, runInAction } from "mobx";
import { PostPreview } from "../models/postPreview";
import ApiResponse from "../models/apiResponse";
import agent from "../API/agent";
import PostImages from "../models/postImages";

export default class PostStore {
    // Existing constructor and properties...
    constructor() {
        makeAutoObservable(this);
        var userJson = localStorage.getItem("brt_user");
        if (userJson) {
            this.isLoggedIn = true;
        }
        else {
            this.isLoggedIn = false;
        }
    }

    feedPosts: PostPreview[] = [];
    setFeedPosts = (posts: PostPreview[]) => this.feedPosts = posts;
    getFeedPosts = () => this.feedPosts;

    popularPosts: PostPreview[] = [];
    setPopularPosts = (posts: PostPreview[]) => this.popularPosts = posts;
    getPopularPosts = () => this.popularPosts;

    postLoading: boolean = false;
    setPostLoading = (value: boolean) => this.postLoading = value;
    getPostLoading = () => this.postLoading;
    isLoggedIn: boolean = false;

    feedPage: number = 1;
    setFeedPage = (value: number) => this.feedPage = value;
    getFeedPage = () => this.feedPage;

    postError: string | null = null;
    setPostError = (value: string | null) => this.postError = value;
    getPostError = () => this.postError;

    currentPost: PostPreview | null = null;
    loadingCurrentPost: boolean = false;
    currentPostError: string | null = null;

    getFeedApi = async () => {
        this.setPostLoading(true);
        try {
            const res: ApiResponse<PostPreview[]> = await agent.Recommendation.GetFeed(!this.isLoggedIn, this.feedPage);
            if (res.isSuccess) {
                runInAction(() => {
                    this.setFeedPosts(res.value);
                });
            }
        }
        catch (e: any) { // Catch Error type or `any` for broader compatibility
            this.setPostError("Error while loading feed");
            console.error("Feed API Error:", this.getPostError, e); // Use console.error for errors
            throw e; // Re-throw the original error, not a new generic Error
        }
        finally {
            this.setPostLoading(false);
        }
    }

    getPopularPostsApi = async (count: number, city?: string) => {
        this.setPostLoading(true);
        try {
            const res: ApiResponse<PostPreview[]> = await agent.Recommendation.GetPopularPosts(!this.isLoggedIn, count, city);
            if (res.isSuccess) {
                runInAction(() => {
                    this.setPopularPosts(res.value);
                });
            }
        }
        catch (e: any) { // Catch Error type or `any`
            this.setPostError("Error while loading popular posts");
            console.error("Popular Posts API Error:", this.getPostError, e);
            throw e; // Re-throw the original error
        }
        finally {
            this.setPostLoading(false);
        }
    }

    getPostImages = async (postId: string) => {
        try {
            const res: ApiResponse<PostImages> = await agent.Posts.GetPostImages(postId);
            return res; // The component should check res.isSuccess
        }
        catch (error: any) { // Catch Error type or `any`
            console.error("Error fetching post images:", error);
            // Optionally, set an error state here if ImagesPreview needs it
            throw error; // Re-throw so the caller can handle it
        }
    }

    loadPost = async (id: string) => {
        // If the post is already loaded, avoid re-fetching unless explicitly needed
        if (this.currentPost && this.currentPost.id === id) {
            return this.currentPost;
        }

        this.loadingCurrentPost = true;
        this.currentPostError = null; // Clear any previous errors

        try {
            // Assuming `agent.Posts.details` is the API call for a single post by ID
            const res: ApiResponse<PostPreview> = await agent.Posts.GetPostById(id); // Adjust this API call if needed
            if (res.isSuccess) {
                runInAction(() => {
                    this.currentPost = res.value;
                });
                return res.value; // Return the fetched post
            } else {
                // Handle API success but logic failure (e.g., res.isSuccess is false)
                runInAction(() => {
                    this.currentPostError = res.error || "Failed to load post details.";
                });
                throw new Error(res.error || "API response indicated failure.");
            }
        } catch (error: any) { // Use `any` for broad error catching or specific Error type
            console.error("Error loading single post:", error);
            runInAction(() => {
                this.currentPostError = error.message || "Failed to load post details.";
            });
            throw error; // Re-throw the error for component to handle (e.g., display specific error message)
        } finally {
            runInAction(() => {
                this.loadingCurrentPost = false;
            });
        }
    }

    clearCurrentPost = () => {
        this.currentPost = null;
        this.currentPostError = null;
    }
}