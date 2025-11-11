import { 
    TextField, 
    Autocomplete, 
    Button, 
    InputAdornment,
    Paper,
    alpha,
    useTheme,
    Stack,
    Box,
    Typography,
    Collapse,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    FormControlLabel,
    Switch
} from "@mui/material";
import { 
    Search, 
    Clear, 
    ExpandMore, 
    ExpandLess,
    FilterList,
    PriceCheck,
    LocationOn,
    Category as CategoryIconMui,
    Work,
    Home,
    Business
} from "@mui/icons-material";
import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import { Category, SubCategory } from "../../categories/types/categoryTypes";
import categoryApi from "../../categories/api/categoriesApi";
import {
    PostType,
    RentObjectType,
    WorkloadType,
    WorkLocationType,
} from "../types/postTypes";

interface SearchBarProps {
    onSearch: (filters: SearchFilters) => void;
    search?: string;
    categoryId?: string;
    subCategoryId?: string;
    isAdvanced?: boolean;
}

interface SearchFilters {
    search?: string;
    categoryId?: string;
    subCategoryId?: string;
    city?: string;
    minPrice?: number;
    maxPrice?: number;
    postType?: PostType;
    // Rent filters
    rentObjectType?: RentObjectType;
    numberOfRooms?: number;
    area?: number;
    floor?: number;
    // Work filters
    workload?: WorkloadType;
    workLocation?: WorkLocationType;
    minSalary?: number;
    maxSalary?: number;
    experienceRequired?: boolean;
}

const SearchBar = observer(({ onSearch, search, categoryId, subCategoryId, isAdvanced }: SearchBarProps) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const { uiStore } = useStore();
    
    // Basic filters
    const [searchText, setSearchText] = useState("");
    const [categories, setCategories] = useState<Category[]>([]);
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
    const [selectedSubCategory, setSelectedSubCategory] = useState<SubCategory | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    
    // Advanced filters toggle
    const [showAdvanced, setShowAdvanced] = useState(false);
    
    // Advanced filters state
    const [city, setCity] = useState("");
    const [minPrice, setMinPrice] = useState<number | "">("");
    const [maxPrice, setMaxPrice] = useState<number | "">("");
    const [postType, setPostType] = useState<PostType>(PostType.Common);
    
    // Rent specific filters
    const [rentObjectType, setRentObjectType] = useState<RentObjectType | "">("");
    const [numberOfRooms, setNumberOfRooms] = useState<number | "">("");
    const [area, setArea] = useState<number | "">("");
    const [floor, setFloor] = useState<number | "">("");
    
    // Work specific filters
    const [workload, setWorkload] = useState<WorkloadType | "">("");
    const [workLocation, setWorkLocation] = useState<WorkLocationType | "">("");
    const [minSalary, setMinSalary] = useState<number | "">("");
    const [maxSalary, setMaxSalary] = useState<number | "">("");
    const [experienceRequired, setExperienceRequired] = useState(false);

    useEffect(() => {
        if (search) {
            setSearchText(search);
        }
    }, [search]);

    useEffect(() => {
        if (categoryId && categories.length > 0) {
            const category = categories.find(x => x.id === categoryId);
            if (category) {
                setSelectedCategory(category);
            }
        }
    }, [categoryId, categories]);

    useEffect(() => {
        if (subCategoryId && selectedCategory) {
            const subCategory = selectedCategory.subCategories.find(x => x.id === subCategoryId);
            if (subCategory) {
                setSelectedSubCategory(subCategory);
            }
        }
    }, [subCategoryId, selectedCategory]);

    // Initialize advanced filters from URL on mount
    useEffect(() => {
        if (isAdvanced && typeof window !== 'undefined') {
            const params = new URLSearchParams(window.location.search);
            
            // Set basic advanced filters
            if (params.get('city')) setCity(params.get('city') || "");
            if (params.get('postType')) setPostType(params.get('postType') as PostType);
            if (params.get('minPrice')) setMinPrice(Number(params.get('minPrice')));
            if (params.get('maxPrice')) setMaxPrice(Number(params.get('maxPrice')));
            
            // Set rent filters
            if (params.get('rentObjectType')) setRentObjectType(Number(params.get('rentObjectType')) as RentObjectType);
            if (params.get('numberOfRooms')) setNumberOfRooms(Number(params.get('numberOfRooms')));
            if (params.get('area')) setArea(Number(params.get('area')));
            if (params.get('floor')) setFloor(Number(params.get('floor')));
            
            // Set work filters
            if (params.get('workload')) setWorkload(Number(params.get('workload')) as WorkloadType);
            if (params.get('workLocation')) setWorkLocation(Number(params.get('workLocation')) as WorkLocationType);
            if (params.get('minSalary')) setMinSalary(Number(params.get('minSalary')));
            if (params.get('maxSalary')) setMaxSalary(Number(params.get('maxSalary')));
            if (params.get('experienceRequired')) setExperienceRequired(params.get('experienceRequired') === 'true');
            
            // Auto-expand advanced filters if any advanced filter is set
            const hasAdvancedFilters = params.get('city') || params.get('postType') || 
                params.get('minPrice') || params.get('maxPrice') ||
                params.get('rentObjectType') || params.get('numberOfRooms') || 
                params.get('area') || params.get('floor') ||
                params.get('workload') || params.get('workLocation') ||
                params.get('minSalary') || params.get('maxSalary') ||
                params.get('experienceRequired');
            
            if (hasAdvancedFilters) {
                setShowAdvanced(true);
            }
        }
    }, [isAdvanced]);

    useEffect(() => {
        fetchCategories();
    }, []);

    const fetchCategories = async () => {
        try {
            setIsLoading(true);
            const response = await categoryApi.getCategories();
            if (response.isSuccess) {
                setCategories(response.value);
            }
        } catch (error) {
            console.error("Error loading categories:", error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleCategoryChange = (_: any, newValue: Category | null) => {
        setSelectedCategory(newValue);
        setSelectedSubCategory(null);
    };

    const handleSubCategoryChange = (_: any, newValue: SubCategory | null) => {
        setSelectedSubCategory(newValue);
    };

    const handleSearch = () => {
        const filters: SearchFilters = {
            search: searchText || undefined,
            categoryId: selectedCategory?.id || undefined,
            subCategoryId: selectedSubCategory?.id || undefined,
        };

        if (isAdvanced && showAdvanced) {
            filters.city = city || undefined;
            filters.postType = postType;

            if (postType === PostType.Common || postType === PostType.Rent) {
                filters.minPrice = minPrice || undefined;
                filters.maxPrice = maxPrice || undefined;
            }

            if (postType === PostType.Rent) {
                filters.rentObjectType = rentObjectType || undefined;
                filters.numberOfRooms = numberOfRooms || undefined;
                filters.area = area || undefined;
                filters.floor = floor || undefined;
            }

            if (postType === PostType.Work) {
                filters.workload = workload || undefined;
                filters.workLocation = workLocation || undefined;
                filters.minSalary = minSalary || undefined;
                filters.maxSalary = maxSalary || undefined;
                filters.experienceRequired = experienceRequired || undefined;
            }
        }

        onSearch(filters);
    };

    const handleClear = () => {
        setSearchText("");
        setSelectedCategory(null);
        setSelectedSubCategory(null);
        setCity("");
        setMinPrice("");
        setMaxPrice("");
        setPostType(PostType.Common);
        setRentObjectType("");
        setNumberOfRooms("");
        setArea("");
        setFloor("");
        setWorkload("");
        setWorkLocation("");
        setMinSalary("");
        setMaxSalary("");
        setExperienceRequired(false);
        onSearch({});
    };

    const handleKeyPress = (event: React.KeyboardEvent) => {
        if (event.key === 'Enter') {
            handleSearch();
        }
    };

    const renderAdvancedFilters = () => (
        <Collapse in={showAdvanced}>
            <Stack spacing={3} sx={{ mt: 3 }}>
                {/* Location and Post Type */}
                <Stack direction={{ xs: "column", md: "row" }} spacing={2}>
                    <TextField
                        label={t("posts:city")}
                        value={city}
                        onChange={(e) => setCity(e.target.value)}
                        fullWidth
                        InputProps={{
                            startAdornment: (
                                <InputAdornment position="start">
                                    <LocationOn sx={{ color: 'primary.main' }} />
                                </InputAdornment>
                            ),
                        }}
                        sx={{
                            '& .MuiOutlinedInput-root': {
                                borderRadius: 3,
                                backgroundColor: alpha(theme.palette.background.paper, 0.5),
                            },
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>{t("posts:postType")}</InputLabel>
                        <Select
                            value={postType}
                            onChange={(e) => setPostType(e.target.value as PostType)}
                            label={t("posts:postType")}
                            startAdornment={
                                <InputAdornment position="start">
                                    <CategoryIconMui sx={{ color: 'primary.main', ml: 1 }} />
                                </InputAdornment>
                            }
                            sx={{
                                borderRadius: 3,
                                backgroundColor: alpha(theme.palette.background.paper, 0.5),
                            }}
                        >
                            <MenuItem value={PostType.Common}>
                                <Stack direction="row" spacing={1} alignItems="center">
                                    <CategoryIconMui fontSize="small" />
                                    <span>{t("common")}</span>
                                </Stack>
                            </MenuItem>
                            <MenuItem value={PostType.Rent}>
                                <Stack direction="row" spacing={1} alignItems="center">
                                    <Home fontSize="small" />
                                    <span>{t("rent")}</span>
                                </Stack>
                            </MenuItem>
                            <MenuItem value={PostType.Work}>
                                <Stack direction="row" spacing={1} alignItems="center">
                                    <Work fontSize="small" />
                                    <span>{t("work")}</span>
                                </Stack>
                            </MenuItem>
                        </Select>
                    </FormControl>
                </Stack>

                {/* Price Range (for Common and Rent) */}
                {(postType === PostType.Common || postType === PostType.Rent) && (
                    <Paper
                        elevation={0}
                        sx={{
                            p: 3,
                            borderRadius: 3,
                            backgroundColor: alpha(theme.palette.primary.main, 0.03),
                            border: `1px solid ${alpha(theme.palette.primary.main, 0.1)}`,
                        }}
                    >
                        <Typography 
                            variant="subtitle2" 
                            sx={{ 
                                mb: 2, 
                                fontWeight: 600,
                                display: 'flex',
                                alignItems: 'center',
                                gap: 1,
                                color: 'primary.main'
                            }}
                        >
                            <PriceCheck fontSize="small" />
                            {t("posts:priceRange")}
                        </Typography>
                        <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                            <TextField
                                label={t("posts:minPrice")}
                                type="number"
                                value={minPrice}
                                onChange={(e) => setMinPrice(e.target.value ? Number(e.target.value) : "")}
                                fullWidth
                                sx={{
                                    '& .MuiOutlinedInput-root': {
                                        borderRadius: 3,
                                        backgroundColor: theme.palette.background.paper,
                                    },
                                }}
                            />
                            <TextField
                                label={t("posts:maxPrice")}
                                type="number"
                                value={maxPrice}
                                onChange={(e) => setMaxPrice(e.target.value ? Number(e.target.value) : "")}
                                fullWidth
                                sx={{
                                    '& .MuiOutlinedInput-root': {
                                        borderRadius: 3,
                                        backgroundColor: theme.palette.background.paper,
                                    },
                                }}
                            />
                        </Stack>
                    </Paper>
                )}

                {/* Rent Specific Filters */}
                {postType === PostType.Rent && (
                    <Paper
                        elevation={0}
                        sx={{
                            p: 3,
                            borderRadius: 3,
                            backgroundColor: alpha(theme.palette.secondary.main, 0.03),
                            border: `1px solid ${alpha(theme.palette.secondary.main, 0.1)}`,
                        }}
                    >
                        <Typography 
                            variant="subtitle2" 
                            sx={{ 
                                mb: 2, 
                                fontWeight: 600,
                                display: 'flex',
                                alignItems: 'center',
                                gap: 1,
                                color: 'secondary.main'
                            }}
                        >
                            <Home fontSize="small" />
                            {t("posts:propertyDetails")}
                        </Typography>
                        <Stack spacing={2}>
                            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                                <FormControl fullWidth>
                                    <InputLabel>{t("posts:propertyType")}</InputLabel>
                                    <Select
                                        value={rentObjectType}
                                        onChange={(e) => setRentObjectType(e.target.value as RentObjectType)}
                                        label={t("posts:propertyType")}
                                        sx={{ borderRadius: 3, backgroundColor: theme.palette.background.paper }}
                                    >
                                        <MenuItem value="">
                                            <em>{t("posts:any")}</em>
                                        </MenuItem>
                                        {Object.entries(RentObjectType)
                                            .filter(([key]) => isNaN(Number(key)))
                                            .map(([key, value]) => (
                                                <MenuItem key={value} value={value}>
                                                    {t(key)}
                                                </MenuItem>
                                            ))}
                                    </Select>
                                </FormControl>

                                <TextField
                                    label={t("posts:numberOfRooms")}
                                    type="number"
                                    value={numberOfRooms}
                                    onChange={(e) => setNumberOfRooms(e.target.value ? Number(e.target.value) : "")}
                                    fullWidth
                                    sx={{
                                        '& .MuiOutlinedInput-root': {
                                            borderRadius: 3,
                                            backgroundColor: theme.palette.background.paper,
                                        },
                                    }}
                                />
                            </Stack>

                            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                                <TextField
                                    label={t("posts:area")}
                                    type="number"
                                    value={area}
                                    onChange={(e) => setArea(e.target.value ? Number(e.target.value) : "")}
                                    fullWidth
                                    sx={{
                                        '& .MuiOutlinedInput-root': {
                                            borderRadius: 3,
                                            backgroundColor: theme.palette.background.paper,
                                        },
                                    }}
                                />
                                <TextField
                                    label={t("posts:floor")}
                                    type="number"
                                    value={floor}
                                    onChange={(e) => setFloor(e.target.value ? Number(e.target.value) : "")}
                                    fullWidth
                                    sx={{
                                        '& .MuiOutlinedInput-root': {
                                            borderRadius: 3,
                                            backgroundColor: theme.palette.background.paper,
                                        },
                                    }}
                                />
                            </Stack>
                        </Stack>
                    </Paper>
                )}

                {/* Work Specific Filters */}
                {postType === PostType.Work && (
                    <Paper
                        elevation={0}
                        sx={{
                            p: 3,
                            borderRadius: 3,
                            backgroundColor: alpha(theme.palette.info.main, 0.03),
                            border: `1px solid ${alpha(theme.palette.info.main, 0.1)}`,
                        }}
                    >
                        <Typography 
                            variant="subtitle2" 
                            sx={{ 
                                mb: 2, 
                                fontWeight: 600,
                                display: 'flex',
                                alignItems: 'center',
                                gap: 1,
                                color: 'info.main'
                            }}
                        >
                            <Business fontSize="small" />
                            {t("posts:workDetails")}
                        </Typography>
                        <Stack spacing={2}>
                            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                                <FormControl fullWidth>
                                    <InputLabel>{t("posts:workload")}</InputLabel>
                                    <Select
                                        value={workload}
                                        onChange={(e) => setWorkload(e.target.value as WorkloadType)}
                                        label={t("posts:workload")}
                                        sx={{ borderRadius: 3, backgroundColor: theme.palette.background.paper }}
                                    >
                                        <MenuItem value="">
                                            <em>{t("posts:any")}</em>
                                        </MenuItem>
                                        {Object.entries(WorkloadType)
                                            .filter(([key]) => isNaN(Number(key)))
                                            .map(([key, value]) => (
                                                <MenuItem key={value} value={value}>
                                                    {t(key)}
                                                </MenuItem>
                                            ))}
                                    </Select>
                                </FormControl>

                                <FormControl fullWidth>
                                    <InputLabel>{t("posts:workLocation")}</InputLabel>
                                    <Select
                                        value={workLocation}
                                        onChange={(e) => setWorkLocation(e.target.value as WorkLocationType)}
                                        label={t("posts:workLocation")}
                                        sx={{ borderRadius: 3, backgroundColor: theme.palette.background.paper }}
                                    >
                                        <MenuItem value="">
                                            <em>{t("posts:any")}</em>
                                        </MenuItem>
                                        {Object.entries(WorkLocationType)
                                            .filter(([key]) => isNaN(Number(key)))
                                            .map(([key, value]) => (
                                                <MenuItem key={value} value={value}>
                                                    {t(key)}
                                                </MenuItem>
                                            ))}
                                    </Select>
                                </FormControl>
                            </Stack>

                            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                                <TextField
                                    label={t("posts:minSalary")}
                                    type="number"
                                    value={minSalary}
                                    onChange={(e) => setMinSalary(e.target.value ? Number(e.target.value) : "")}
                                    fullWidth
                                    sx={{
                                        '& .MuiOutlinedInput-root': {
                                            borderRadius: 3,
                                            backgroundColor: theme.palette.background.paper,
                                        },
                                    }}
                                />
                                <TextField
                                    label={t("posts:maxSalary")}
                                    type="number"
                                    value={maxSalary}
                                    onChange={(e) => setMaxSalary(e.target.value ? Number(e.target.value) : "")}
                                    fullWidth
                                    sx={{
                                        '& .MuiOutlinedInput-root': {
                                            borderRadius: 3,
                                            backgroundColor: theme.palette.background.paper,
                                        },
                                    }}
                                />
                            </Stack>

                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={experienceRequired}
                                        onChange={(e) => setExperienceRequired(e.target.checked)}
                                        sx={{
                                            '& .MuiSwitch-switchBase.Mui-checked': {
                                                color: 'info.main',
                                                '& + .MuiSwitch-track': {
                                                    backgroundColor: 'info.main',
                                                },
                                            },
                                        }}
                                    />
                                }
                                label={
                                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                                        {t("posts:experienceRequired")}
                                    </Typography>
                                }
                            />
                        </Stack>
                    </Paper>
                )}
            </Stack>
        </Collapse>
    );

    return (
        <Paper 
            elevation={0}
            sx={{ 
                p: 4,
                borderRadius: 4,
                border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
                backdropFilter: 'blur(20px)',
            }}
        >
            <Stack spacing={3}>
                {/* Search Input Row */}
                <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                    <TextField
                        placeholder={t("posts:searchPlaceholder")}
                        value={searchText}
                        onChange={(e) => setSearchText(e.target.value)}
                        onKeyPress={handleKeyPress}
                        fullWidth
                        InputProps={{
                            startAdornment: (
                                <InputAdornment position="start">
                                    <Search sx={{ color: 'primary.main' }} />
                                </InputAdornment>
                            ),
                        }}
                        sx={{
                            '& .MuiOutlinedInput-root': {
                                borderRadius: 3,
                                backgroundColor: alpha(theme.palette.background.paper, 0.5),
                                '& fieldset': {
                                    borderColor: alpha(theme.palette.divider, 0.3),
                                    transition: 'border-color 0.3s ease',
                                },
                                '&:hover fieldset': {
                                    borderColor: theme.palette.primary.main,
                                },
                                '&.Mui-focused fieldset': {
                                    borderColor: theme.palette.primary.main,
                                    borderWidth: '2px',
                                },
                            },
                            '& .MuiInputBase-input': {
                                fontWeight: 500,
                            },
                        }}
                    />
                    
                    <Button 
                        variant="contained" 
                        onClick={handleSearch}
                        startIcon={<Search />}
                        sx={{ 
                            minWidth: { xs: '100%', sm: 140 },
                            borderRadius: 3,
                            fontWeight: 700,
                            px: 4,
                            py: 1.5,
                            background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
                            boxShadow: `0 4px 12px ${alpha(theme.palette.primary.main, 0.3)}`,
                            '&:hover': {
                                transform: "translateY(-2px)",
                                boxShadow: `0 6px 16px ${alpha(theme.palette.primary.main, 0.4)}`,
                            },
                        }}
                    >
                        {t("posts:search")}
                    </Button>
                </Stack>

                {/* Category Selectors Row */}
                <Stack direction={{ xs: "column", md: "row" }} spacing={2}>
                    <Autocomplete
                        options={categories}
                        value={selectedCategory}
                        onChange={handleCategoryChange}
                        getOptionLabel={(option) => uiStore.lang === "pl" ? option.namePL : option.nameEN}
                        renderInput={(params) => (
                            <TextField 
                                {...params} 
                                placeholder={t("posts:selectCategory")}
                                sx={{
                                    '& .MuiOutlinedInput-root': {
                                        borderRadius: 3,
                                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                                    },
                                }}
                            />
                        )}
                        loading={isLoading}
                        sx={{ flex: 1 }}
                        slotProps={{
                            paper: {
                                sx: {
                                    borderRadius: 3,
                                    mt: 1,
                                    boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                                },
                            },
                        }}
                    />

                    <Autocomplete
                        options={selectedCategory?.subCategories || []}
                        value={selectedSubCategory}
                        onChange={handleSubCategoryChange}
                        getOptionLabel={(option) => uiStore.lang === "pl" ? option.namePL : option.nameEN}
                        renderInput={(params) => (
                            <TextField 
                                {...params} 
                                placeholder={t("posts:selectSubCategory")}
                                sx={{
                                    '& .MuiOutlinedInput-root': {
                                        borderRadius: 3,
                                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                                    },
                                }}
                            />
                        )}
                        disabled={!selectedCategory}
                        sx={{ flex: 1 }}
                        slotProps={{
                            paper: {
                                sx: {
                                    borderRadius: 3,
                                    mt: 1,
                                    boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                                },
                            },
                        }}
                    />

                    <Button 
                        variant="outlined" 
                        onClick={handleClear}
                        startIcon={<Clear />}
                        sx={{
                            minWidth: { xs: '100%', md: 140 },
                            borderRadius: 3,
                            fontWeight: 600,
                            px: 3,
                            borderColor: alpha(theme.palette.divider, 0.5),
                            color: 'text.primary',
                            '&:hover': {
                                borderColor: theme.palette.primary.main,
                                backgroundColor: alpha(theme.palette.primary.main, 0.05),
                            },
                        }}
                    >
                        {t("posts:clear")}
                    </Button>
                </Stack>

                {/* Advanced Filters Toggle */}
                {isAdvanced && (
                    <>
                        <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                            <Button
                                onClick={() => setShowAdvanced(!showAdvanced)}
                                endIcon={showAdvanced ? <ExpandLess /> : <ExpandMore />}
                                startIcon={<FilterList />}
                                sx={{
                                    borderRadius: 3,
                                    fontWeight: 600,
                                    px: 3,
                                    color: 'primary.main',
                                    '&:hover': {
                                        backgroundColor: alpha(theme.palette.primary.main, 0.08),
                                    },
                                }}
                            >
                                {showAdvanced ? t("posts:hideAdvancedFilters") : t("posts:showAdvancedFilters")}
                            </Button>
                        </Box>

                        {/* Advanced Filters Content */}
                        {renderAdvancedFilters()}
                    </>
                )}
            </Stack>
        </Paper>
    );
});

export default SearchBar;