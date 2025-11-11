import { useNavigate, useSearchParams } from "react-router";
import SearchBar from "../components/searchBar";
import { Box, Typography, ToggleButton, ToggleButtonGroup, IconButton } from "@mui/material";
import { useEffect, useState } from "react";
import { PostPreview, PostType } from "../types/postTypes";
import FeedPostList from "../components/feedPostList";
import { ChevronLeft, ChevronRight } from "@mui/icons-material";
import { useTranslation } from "react-i18next";
import postApi from "../api/postApi";
import { SearchFilters } from "../dto/postDto";

const SearchPostsPage = () => {
    const { t } = useTranslation();
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [posts, setPosts] = useState<PostPreview[] | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(0);

    useEffect(() => {
        const fetchPosts = async () => {
            try {
                setIsLoading(true);
                const response = await postApi.getPosts({
                    filterBy: {
                        pageNumber: page,
                        pageSize: pageSize,
                        search: searchParams.get('search') || undefined,
                        categoryId: searchParams.get('categoryId') || undefined,
                        subCategoryId: searchParams.get('subCategoryId') || undefined,
                        city: searchParams.get('city') || undefined,
                        minPrice: searchParams.get('minPrice') ? Number(searchParams.get('minPrice')) : undefined,
                        maxPrice: searchParams.get('maxPrice') ? Number(searchParams.get('maxPrice')) : undefined,
                        postType: searchParams.get('postType') 
                        ? PostType[searchParams.get('postType') as keyof typeof PostType] 
                        : undefined,
                        // Rent filters
                        rentObjectType: searchParams.get('rentObjectType') ? Number(searchParams.get('rentObjectType')) : undefined,
                        numberOfRooms: searchParams.get('numberOfRooms') ? Number(searchParams.get('numberOfRooms')) : undefined,
                        area: searchParams.get('area') ? Number(searchParams.get('area')) : undefined,
                        floor: searchParams.get('floor') ? Number(searchParams.get('floor')) : undefined,
                        // Work filters
                        workload: searchParams.get('workload') ? Number(searchParams.get('workload')) : undefined,
                        workLocation: searchParams.get('workLocation') ? Number(searchParams.get('workLocation')) : undefined,
                        minSalary: searchParams.get('minSalary') ? Number(searchParams.get('minSalary')) : undefined,
                        maxSalary: searchParams.get('maxSalary') ? Number(searchParams.get('maxSalary')) : undefined,
                        experienceRequired: searchParams.get('experienceRequired') === 'true' ? true : undefined,
                    }
                });
                if (response.isSuccess) {
                    setPosts(response.value.items);
                    setTotalPages(response.value.totalPages || Math.ceil(response.value.totalCount / pageSize));
                }
            } catch {
                console.error("Error while loading posts");
            } finally {
                setIsLoading(false);
            }
        };
        fetchPosts();
    }, [page, pageSize, searchParams]);

    useEffect(() => {
        setPage(1);
    }, [searchParams]);

    const handleSearch = (filters: SearchFilters) => {
        const params = new URLSearchParams();

        // Basic filters
        if (filters.search) params.append('search', filters.search);
        if (filters.categoryId) params.append('categoryId', filters.categoryId);
        if (filters.subCategoryId) params.append('subCategoryId', filters.subCategoryId);
        
        // Advanced filters
        if (filters.city) params.append('city', filters.city);
        if (filters.minPrice !== undefined) params.append('minPrice', filters.minPrice.toString());
        if (filters.maxPrice !== undefined) params.append('maxPrice', filters.maxPrice.toString());
        if (filters.postType !== undefined) params.append('postType', filters.postType.toString());
        
        // Rent filters
        if (filters.rentObjectType !== undefined) params.append('rentObjectType', filters.rentObjectType.toString());
        if (filters.numberOfRooms !== undefined) params.append('numberOfRooms', filters.numberOfRooms.toString());
        if (filters.area !== undefined) params.append('area', filters.area.toString());
        if (filters.floor !== undefined) params.append('floor', filters.floor.toString());
        
        // Work filters
        if (filters.workload !== undefined) params.append('workload', filters.workload.toString());
        if (filters.workLocation !== undefined) params.append('workLocation', filters.workLocation.toString());
        if (filters.minSalary !== undefined) params.append('minSalary', filters.minSalary.toString());
        if (filters.maxSalary !== undefined) params.append('maxSalary', filters.maxSalary.toString());
        if (filters.experienceRequired !== undefined) params.append('experienceRequired', filters.experienceRequired.toString());

        navigate(`/search?${params.toString()}`);
    };

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
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
                <SearchBar 
                    onSearch={handleSearch} 
                    search={searchParams.get('search') || undefined} 
                    categoryId={searchParams.get('categoryId') || undefined} 
                    subCategoryId={searchParams.get('subCategoryId') || undefined}
                    isAdvanced
                />
            </Box>

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
                        <FeedPostList posts={posts} />
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
                            {t("posts:noPostsFound")}
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
};

export default SearchPostsPage;