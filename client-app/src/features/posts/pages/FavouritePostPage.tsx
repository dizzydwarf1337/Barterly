import { Box, Typography, ToggleButton, ToggleButtonGroup, IconButton } from "@mui/material";
import { useTranslation } from "react-i18next";
import useStore from "../../../app/stores/store";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { PostPreview } from "../types/postTypes";
import userPostApi from "../api/userPostApi";
import FeedPostList from "../components/feedPostList";
import { ChevronLeft, ChevronRight } from "@mui/icons-material";


const FavouritePostsPage = () => {

    const { t } = useTranslation();
    const { authStore } = useStore();
    const { user, isLoggedIn } = authStore;
    const navigate = useNavigate();
    const [posts, setPosts] = useState<PostPreview[] | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(0);

    useEffect(()=>{
        const fetchPosts = async () => {

            try {
                setIsLoading(true);
                    const response = await userPostApi.getFavPosts({filterBy:{
                        pageNumber:page,
                        pageSize:pageSize
                    }})
                    if(response.isSuccess) {
                        setPosts(response.value.items);
                        setTotalPages(response.value.totalPages || Math.ceil(response.value.totalCount / pageSize));
                    }
            }
            catch {
                console.error("Error while loading posts")
            }
            finally{
                setIsLoading(false);
            }
        }
        fetchPosts();
    }, [user, isLoggedIn, page, pageSize])


    useEffect(() => {
        if(!authStore.isLoggedIn)
            navigate("/login");
    }, [authStore.isLoggedIn])

    const handlePageSizeChange = (_: React.MouseEvent<HTMLElement>, newPageSize: number | null) => {
        if (newPageSize !== null) {
            setPageSize(newPageSize);
            setPage(1);
        }
    };

    const handlePageClick = (pageNum: number) => {
        setPage(pageNum);
        window.scrollTo({ top: 0, behavior: 'smooth' });
    };

    const getPageNumbers = () => {
        const pages = [];
        const maxVisible = 7;
        
        if (totalPages <= maxVisible) {
            for (let i = 1; i <= totalPages; i++) {
                pages.push(i);
            }
        } else {
            if (page <= 4) {
                for (let i = 1; i <= 5; i++) pages.push(i);
                pages.push('...');
                pages.push(totalPages);
            } else if (page >= totalPages - 3) {
                pages.push(1);
                pages.push('...');
                for (let i = totalPages - 4; i <= totalPages; i++) pages.push(i);
            } else {
                pages.push(1);
                pages.push('...');
                for (let i = page - 1; i <= page + 1; i++) pages.push(i);
                pages.push('...');
                pages.push(totalPages);
            }
        }
        return pages;
    };


    return (
        <Box sx={{display:'flex', flexDirection:'column', gap:2}}>
            <Typography fontWeight={700} fontSize={32}>
                {t("posts:favouritePosts")}
            </Typography>
            
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                <Typography>{t("posts:common.itemsPerPage")}:</Typography>
                <ToggleButtonGroup
                    value={pageSize}
                    exclusive
                    onChange={handlePageSizeChange}
                    size="small"
                    disabled={isLoading}
                >
                    <ToggleButton value={10}>10</ToggleButton>
                    <ToggleButton value={25}>25</ToggleButton>
                    <ToggleButton value={50}>50</ToggleButton>
                </ToggleButtonGroup>
            </Box>

            <Box>
                {posts && posts.length > 0 ? (
                    <>
                        <FeedPostList posts={posts}/>
                        {totalPages > 1 && (
                            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 1, mt: 4, mb: 2 }}>
                                <IconButton 
                                    onClick={() => handlePageClick(page - 1)}
                                    disabled={page === 1 || isLoading}
                                    size="small"
                                >
                                    <ChevronLeft />
                                </IconButton>
                                
                                {getPageNumbers().map((pageNum, index) => (
                                    pageNum === '...' ? (
                                        <Typography key={`ellipsis-${index}`} sx={{ px: 1 }}>...</Typography>
                                    ) : (
                                        <IconButton
                                            key={pageNum}
                                            onClick={() => handlePageClick(pageNum as number)}
                                            disabled={isLoading}
                                            sx={{
                                                minWidth: 40,
                                                height: 40,
                                                bgcolor: page === pageNum ? 'primary.main' : 'transparent',
                                                color: page === pageNum ? 'primary.contrastText' : 'text.primary',
                                                '&:hover': {
                                                    bgcolor: page === pageNum ? 'primary.dark' : 'action.hover',
                                                },
                                                fontWeight: page === pageNum ? 700 : 400,
                                            }}
                                        >
                                            {pageNum}
                                        </IconButton>
                                    )
                                ))}
                                
                                <IconButton 
                                    onClick={() => handlePageClick(page + 1)}
                                    disabled={page === totalPages || isLoading}
                                    size="small"
                                >
                                    <ChevronRight />
                                </IconButton>
                            </Box>
                        )}
                    </>
                ) : (
                    !isLoading && (
                        <Typography sx={{ textAlign: 'center', mt: 4, color: 'text.secondary' }}>
                            {t("posts:noFavouritePosts")}
                        </Typography>
                    )
                )}
                {isLoading && (
                    <Typography sx={{ textAlign: 'center', mt: 4 }}>
                        {t("posts:common.loading")}
                    </Typography>
                )}
            </Box>
        </Box>
    );
}

export default FavouritePostsPage;