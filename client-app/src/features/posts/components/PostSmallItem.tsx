import {Box, Typography} from "@mui/material";
import MoneyIcon from '@mui/icons-material/Money'; // Example: using an icon

interface Props {
    post: PostPreview;
}

export default function PostSmallItem({post}: Props) {

    const {uiStore} = useStore();
    const {t} = useTranslation();
    const navigate = useNavigate();

    const renderPriceOrSalary = () => {
        if (post.postType === PostType.Work) {
            let salaryText = '';
            if (post.minSalary && post.maxSalary) {
                salaryText = `${post.minSalary} - ${post.maxSalary}`;
            } else if (post.minSalary) {
                salaryText = `${post.minSalary}`;
            } else if (post.maxSalary) {
                salaryText = `${post.maxSalary}`;
            }
            return (
                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                    <MoneyIcon fontSize="small" color="success"/>
                    <Typography variant="body1">
                        {salaryText} {post.currency ? PostCurrency[post.currency] : ''}
                    </Typography>
                </Box>
            );
        } else if (post.postType === PostType.Rent) {
            return (
                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                    <MoneyIcon fontSize="small" color="success"/>
                    <Typography variant="body1">
                        {post.price != null ? `${post.price} ${post.currency ? PostCurrency[post.currency] : ''}` : t('negotiable')}
                    </Typography>
                </Box>
            );
        } else {
            return (
                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                    {post.price != null && post.price > 0 && post.priceType !== PostPriceType.Free ? (
                        <>
                            <MoneyIcon fontSize="small" color="success"/>
                            <Typography
                                variant="body1">{post.price} {post.currency ? PostCurrency[post.currency] : ''}</Typography>
                        </>
                    ) : (
                        <Typography variant="body1">{t("Free")}</Typography>
                    )}
                </Box>
            );
        }
    };

    return (
        <Box display="flex" onClick={() => {
            navigate(`posts/${post.id}`)
        }} height={uiStore.isMobile ? "125px" : "100px"} flexDirection="row" width="95%" margin="5px" padding="5px"
             gap="5px"
             sx={{
                 backgroundColor: "background.paper",
                 borderRadius: "10px",
                 color: `primary.contrastText`,
                 ':hover': {
                     boxShadow: `0px 2px 4px ${uiStore.theme.palette.primary.contrastText}`,
                     translate: '0px -2px'
                 },
                 cursor: "pointer",
                 transition: "0.2s ease-out all"
             }}>
            <Box display="flex" flexDirection="column" justifyContent="space-between" flex="auto" padding="8px 12px">
                <Box position="relative" flex="1" display="flex" flexDirection="row" alignItems="center"
                     justifyContent="space-between">
                    <Typography variant="body2"> {post.title}</Typography>
                    <Typography variant="caption" color="textSecondary">{post.city}</Typography>
                </Box>
                <Box display="flex" flexDirection="row" gap="10px" flex="2">
                    <Box display="flex" flexDirection="column" justifyContent="flex-end" flex="1" padding="5px">
                        <Box>
                            <Typography variant="caption">{post.shortDescription}</Typography>
                        </Box>
                    </Box>
                    <Box justifySelf="center" alignSelf="flex-end">
                        {renderPriceOrSalary()}
                    </Box>
                </Box>
            </Box>
        </Box>
    );
}