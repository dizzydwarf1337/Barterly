import {
    Dialog,
    DialogContent,
    Box,
    Typography,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Divider,
    Avatar,
    TextField,
    InputAdornment,
    Slide,
    useTheme,
    alpha,
    Switch,
    Chip,
    Badge,
} from '@mui/material';
import { TransitionProps } from '@mui/material/transitions';
import { forwardRef, useState, ReactElement } from 'react';
import { observer } from 'mobx-react-lite';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router';

// Icons
import CloseIcon from '@mui/icons-material/Close';
import SearchIcon from '@mui/icons-material/Search';
import HomeIcon from '@mui/icons-material/Home';
import CategoryIcon from '@mui/icons-material/Category';
import AddIcon from '@mui/icons-material/Add';
import PersonIcon from '@mui/icons-material/Person';
import LoginIcon from '@mui/icons-material/Login';
import LogoutIcon from '@mui/icons-material/Logout';
import SettingsIcon from '@mui/icons-material/Settings';
import NotificationsIcon from '@mui/icons-material/Notifications';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import LightModeIcon from '@mui/icons-material/LightMode';
import LanguageIcon from '@mui/icons-material/Language';
import FavoriteIcon from '@mui/icons-material/Favorite';
import BookmarkIcon from '@mui/icons-material/Bookmark';
import HistoryIcon from '@mui/icons-material/History';
import HelpIcon from '@mui/icons-material/Help';
import InfoIcon from '@mui/icons-material/Info';
import useStore from '../stores/store';
import darkTheme from '../theme/DarkTheme';
import lightTheme from '../theme/LightTheme';


const Transition = forwardRef(function Transition(
    props: TransitionProps & {
        children: ReactElement;
    },
    ref: React.Ref<unknown>,
) {
    return <Slide direction="left" ref={ref} {...props} />;
});

interface MenuItem {
    icon: ReactElement;
    text: string;
    path?: string;
    onClick?: () => void;
    badge?: number;
    chip?: string;
    divider?: boolean;
    requireAuth?: boolean;
    hideWhenAuth?: boolean;
}

export default observer(function MobileNavDialog() {
    const { uiStore, authStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();
    const theme = useTheme();
    
    const [searchQuery, setSearchQuery] = useState('');
    const [notificationCount] = useState(3); // Mock notification count

    const handleClose = () => {
        uiStore.setIsMobileMenuOpen(false);
    };

    const handleNavigate = (path: string) => {
        navigate(path);
        handleClose();
    };

    const handleSearch = (event: React.FormEvent) => {
        event.preventDefault();
        if (searchQuery.trim()) {
            navigate(`/search?q=${encodeURIComponent(searchQuery.trim())}`);
            handleClose();
        }
    };

    const handleAddPost = () => {
        if (authStore.isLoggedIn) {
            navigate('/posts/create');
        } else {
            uiStore.showSnackbar(t('pleaseLoginToAddPost'), 'warning', 'center');
            navigate('/login');
        }
        handleClose();
    };

    const handleLogin = () => {
        navigate('/login');
        handleClose();
    };

    const handleLogout = async () => {
        try {
            await authStore.logout();
            uiStore.showSnackbar(t('logoutSuccess'), 'success', 'center');
            navigate('/');
        } catch (error) {
            uiStore.showSnackbar(t('logoutFailed'), 'error', 'center');
        }
        handleClose();
    };

    const toggleTheme = () => {
        uiStore.setTheme(uiStore.themeMode === 'light' ? darkTheme : lightTheme);
    };

    // Menu items configuration
    const menuItems: MenuItem[] = [
        {
            icon: <HomeIcon />,
            text: t('Home'),
            path: '/',
        },
        {
            icon: <CategoryIcon />,
            text: t('categories'),
            path: '/categories',
        },
        {
            icon: <AddIcon />,
            text: t('addPost'),
            onClick: handleAddPost,
            chip: authStore.isLoggedIn ? undefined : t('loginRequired'),
        },
        { divider: true, icon: <></>, text: '' },
        {
            icon: <FavoriteIcon />,
            text: t('favorites'),
            path: '/favorites',
            requireAuth: true,
        },
        {
            icon: <BookmarkIcon />,
            text: t('saved'),
            path: '/saved',
            requireAuth: true,
        },
        {
            icon: <HistoryIcon />,
            text: t('history'),
            path: '/history',
            requireAuth: true,
        },
        {
            icon: <NotificationsIcon />,
            text: t('notifications'),
            path: '/notifications',
            badge: notificationCount,
            requireAuth: true,
        },
        { divider: true, icon: <></>, text: '' },
        {
            icon: <SettingsIcon />,
            text: t('settings'),
            path: '/settings',
            requireAuth: true,
        },
        {
            icon: <HelpIcon />,
            text: t('help'),
            path: '/help',
        },
        {
            icon: <InfoIcon />,
            text: t('about'),
            path: '/about',
        },
        { divider: true, icon: <></>, text: '' },
        {
            icon: <LoginIcon />,
            text: t('login'),
            onClick: handleLogin,
            hideWhenAuth: true,
        },
        {
            icon: <LogoutIcon />,
            text: t('logout'),
            onClick: handleLogout,
            requireAuth: true,
        },
    ];

    const filteredMenuItems = menuItems.filter(item => {
        if (item.requireAuth && !authStore.isLoggedIn) return false;
        if (item.hideWhenAuth && authStore.isLoggedIn) return false;
        return true;
    });

    return (
        <Dialog
            fullScreen
            open={uiStore.isMobileMenuOpen}
            onClose={handleClose}
            TransitionComponent={Transition}
            PaperProps={{
                sx: {
                    background: `linear-gradient(135deg, ${alpha(theme.palette.background.paper, 0.95)} 0%, ${alpha(theme.palette.background.default, 0.98)} 100%)`,
                    backdropFilter: 'blur(20px)',
                }
            }}
        >
            <DialogContent sx={{ p: 0, height: '100%', display: 'flex', flexDirection: 'column' }}>
                {/* Header */}
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'space-between',
                        p: 2,
                        borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                        background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                        color: theme.palette.primary.contrastText,
                    }}
                >
                    <Box display="flex" alignItems="center" gap={2}>
                        <Box
                            sx={{
                                width: 40,
                                height: 40,
                                borderRadius: '50%',
                                background: 'rgba(255, 255, 255, 0.2)',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                color: 'white',
                                fontWeight: 'bold',
                                fontSize: '1.2rem',
                                backdropFilter: 'blur(10px)',
                            }}
                        >
                            B
                        </Box>
                        <Typography variant="h5" fontWeight="bold">
                            Barterly
                        </Typography>
                    </Box>
                    <IconButton
                        onClick={handleClose}
                        sx={{
                            color: 'inherit',
                            '&:hover': {
                                backgroundColor: 'rgba(255, 255, 255, 0.1)',
                                transform: 'rotate(90deg)',
                            },
                            transition: 'all 0.3s ease',
                        }}
                    >
                        <CloseIcon />
                    </IconButton>
                </Box>

                {/* User Section */}
                {authStore.isLoggedIn && (
                    <Box
                        sx={{
                            p: 3,
                            background: `linear-gradient(135deg, ${alpha(theme.palette.primary.main, 0.1)} 0%, ${alpha(theme.palette.secondary.main, 0.1)} 100%)`,
                            borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                        }}
                    >
                        <Box display="flex" alignItems="center" gap={2} mb={2}>
                            <Badge
                                badgeContent={notificationCount}
                                color="error"
                                overlap="circular"
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                            >
                                <Avatar
                                    sx={{
                                        width: 56,
                                        height: 56,
                                        border: `3px solid ${theme.palette.primary.main}`,
                                    }}
                                    src={authStore.user?.profilePicturePath ?? ""}
                                    alt={authStore.user?.firstName}
                                >
                                    {authStore.user?.firstName?.charAt(0) || <PersonIcon />}
                                </Avatar>
                            </Badge>
                            <Box>
                                <Typography variant="h6" fontWeight="bold">
                                    {authStore.user?.firstName} {authStore.user?.lastName}
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    {authStore.user?.email}
                                </Typography>
                                <Chip
                                    label={authStore.user?.role || 'User'}
                                    size="small"
                                    color="primary"
                                    variant="outlined"
                                    sx={{ mt: 0.5 }}
                                />
                            </Box>
                        </Box>
                    </Box>
                )}

                {/* Search Section */}
                <Box sx={{ p: 2, borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}` }}>
                    <form onSubmit={handleSearch}>
                        <TextField
                            fullWidth
                            placeholder={`${t('searchPlaceholder')}...`}
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <SearchIcon color="action" />
                                    </InputAdornment>
                                ),
                                endAdornment: searchQuery && (
                                    <InputAdornment position="end">
                                        <IconButton
                                            size="small"
                                            onClick={() => setSearchQuery('')}
                                            edge="end"
                                        >
                                            <CloseIcon fontSize="small" />
                                        </IconButton>
                                    </InputAdornment>
                                ),
                            }}
                            sx={{
                                '& .MuiOutlinedInput-root': {
                                    borderRadius: 3,
                                    backgroundColor: alpha(theme.palette.background.default, 0.5),
                                    '&:hover': {
                                        backgroundColor: alpha(theme.palette.background.default, 0.8),
                                    },
                                    '&.Mui-focused': {
                                        backgroundColor: alpha(theme.palette.background.default, 1),
                                        boxShadow: `0 0 0 2px ${alpha(theme.palette.primary.main, 0.2)}`,
                                    },
                                },
                            }}
                        />
                    </form>
                </Box>

                {/* Settings Section */}
                <Box sx={{ p: 2, borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}` }}>
                    <Box display="flex" justifyContent="space-between" alignItems="center" mb={1}>
                        <Box display="flex" alignItems="center" gap={1}>
                            {theme.palette.mode === 'dark' ? <DarkModeIcon /> : <LightModeIcon />}
                            <Typography variant="body1">{t('darkMode')}</Typography>
                        </Box>
                        <Switch
                            checked={theme.palette.mode === 'dark'}
                            onChange={toggleTheme}
                            color="primary"
                        />
                    </Box>
                    <Box display="flex" justifyContent="space-between" alignItems="center">
                        <Box display="flex" alignItems="center" gap={1}>
                            <LanguageIcon />
                            <Typography variant="body1">{t('language')}</Typography>
                        </Box>
                        <Chip
                            label={t('currentLanguage') || 'EN'}
                            size="small"
                            color="primary"
                            variant="outlined"
                        />
                    </Box>
                </Box>

                {/* Navigation Menu */}
                <Box sx={{ flex: 1, overflow: 'auto' }}>
                    <List sx={{ py: 1 }}>
                        {filteredMenuItems.map((item, index) => {
                            if (item.divider) {
                                return <Divider key={index} sx={{ my: 1 }} />;
                            }

                            return (
                                <ListItem key={index} disablePadding>
                                    <ListItemButton
                                        onClick={() => {
                                            if (item.onClick) {
                                                item.onClick();
                                            } else if (item.path) {
                                                handleNavigate(item.path);
                                            }
                                        }}
                                        sx={{
                                            py: 1.5,
                                            px: 3,
                                            borderRadius: 2,
                                            mx: 1,
                                            mb: 0.5,
                                            transition: 'all 0.2s ease',
                                            '&:hover': {
                                                backgroundColor: alpha(theme.palette.primary.main, 0.1),
                                                transform: 'translateX(8px)',
                                            },
                                        }}
                                    >
                                        <ListItemIcon sx={{ minWidth: 40, color: theme.palette.primary.main }}>
                                            {item.badge ? (
                                                <Badge badgeContent={item.badge} color="error">
                                                    {item.icon}
                                                </Badge>
                                            ) : (
                                                item.icon
                                            )}
                                        </ListItemIcon>
                                        <ListItemText
                                            primary={item.text}
                                            primaryTypographyProps={{
                                                fontWeight: 500,
                                                fontSize: '1.1rem',
                                            }}
                                        />
                                        {item.chip && (
                                            <Chip
                                                label={item.chip}
                                                size="small"
                                                color="warning"
                                                variant="outlined"
                                            />
                                        )}
                                    </ListItemButton>
                                </ListItem>
                            );
                        })}
                    </List>
                </Box>

                {/* Footer */}
                <Box
                    sx={{
                        p: 2,
                        borderTop: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                        background: alpha(theme.palette.background.default, 0.5),
                        textAlign: 'center',
                    }}
                >
                    <Typography variant="body2" color="text.secondary">
                        © 2024 Barterly. {t('allRightsReserved')}
                    </Typography>
                </Box>
            </DialogContent>
        </Dialog>
    );
});