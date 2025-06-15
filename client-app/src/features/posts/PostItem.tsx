import { Box, Typography } from "@mui/material";
import { PostPreview } from "../../app/models/postPreview";
import useStore from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { PostType } from "../../app/enums/postType";
import { ContractType } from "../../app/enums/contractType";
import { WorkloadType } from "../../app/enums/workLoadType";
import { useTranslation } from "react-i18next";
import MoneyIcon from '@mui/icons-material/Money';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import { PostCurrency } from "../../app/enums/postCurrency";
import { WorkLocationType } from "../../app/enums/WorkLocationType";
import { PostPromotionType } from "../../app/enums/postPromotionType";
import { useNavigate } from "react-router";
import HomeIcon from '@mui/icons-material/Home';
import { RentObjectType } from "../../app/enums/rentObjectType";
import { PostPriceType } from "../../app/enums/postPriceType";
import ImagesPreview from "./imagesPreview";

interface Props {
    post: PostPreview;
}

export default observer(function PostItem({ post }: Props) {
    const { uiStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();

    const renderPriceAndType = () => {
        const currencySymbol = post.currency ? PostCurrency[post.currency] : '';
        const priceTypeTranslation = post.priceType != null ? t(PostPriceType[post.priceType]) : '';

        if (post.postType === PostType.Work) {
            let salaryText = '';
            if (post.minSalary && post.maxSalary) {
                salaryText = `${post.minSalary} - ${post.maxSalary}`;
            } else if (post.minSalary) {
                salaryText = `${t('from')} ${post.minSalary}`;
            } else if (post.maxSalary) {
                salaryText = `${t('upTo')} ${post.maxSalary}`;
            } else {
                salaryText = t('negotiable');
            }

            return (
                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                    <AccountBalanceWalletIcon color="success" />
                    <Typography variant="body1">
                        {salaryText} {currencySymbol} {priceTypeTranslation ? `/ ${priceTypeTranslation}` : ''}
                    </Typography>
                </Box>
            );
        } else if (post.postType === PostType.Rent) {
            return (
                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                    <MoneyIcon color="success" />
                    <Typography variant="body1">
                        {post.price != null ? `${post.price} ${currencySymbol}` : t('negotiable')}
                        {priceTypeTranslation ? ` / ${priceTypeTranslation}` : ''}
                    </Typography>
                </Box>
            );
        } else { // For other PostTypes
            return (
                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                    {post.price != null && post.price > 0 && post.priceType !== PostPriceType.Free ? (
                        <>
                            <MoneyIcon color="success" />
                            <Typography variant="body1">
                                {post.price} {currencySymbol} {priceTypeTranslation ? `/ ${priceTypeTranslation}` : ''}
                            </Typography>
                        </>
                    ) : (
                        <Typography variant="body1">{t('Free')}</Typography>
                    )}
                </Box>
            );
        }
    };

    return (
        <Box
            display="flex"
            onClick={() => navigate(`posts/${post.id}`)}
            height="150px"
            flexDirection="row"
            width="95%"
            margin="10px"
            padding="10px"
            gap="20px"
            sx={{
                backgroundColor: "background.paper",
                borderRadius: "10px",
                color: `primary.contrastText`,
                ':hover': {
                    boxShadow: `0px 2px 4px ${uiStore.theme.palette.primary.contrastText}`,
                    transform: 'translateY(-2px)'
                },
                cursor: "pointer",
                transition: "0.2s ease-out all"
            }}
        >
            {post.mainImageUrl && (
                <ImagesPreview mainImageUrl={post.mainImageUrl} postId={post.id} />
            )}
            <Box display="flex" flexDirection="column" justifyContent="space-between" flex="auto" padding="10px">
                <Box position="relative" flex="1" display="flex" flexDirection="row" justifyContent="space-between" alignItems="flex-start">
                    <Typography variant="h5"> {post.title}</Typography>
                    {post.postPromotionType !== null && post.postPromotionType !== PostPromotionType.None && (
                        <Box position="absolute" top="0" right="-20px" padding="2px" width="100px" borderRadius="5px 0px 0px 5px" sx={{ backgroundColor: "secondary.main" }}>
                            <Typography textAlign="center">
                                {PostPromotionType[post.postPromotionType]}
                            </Typography>
                        </Box>
                    )}
                </Box>
                <Box display="flex" flexDirection="row" gap="10px" flex="2">
                    <Box display="flex" flexDirection="column" justifyContent="space-between" flex="1" padding="5px">
                        <Box display="flex" gap="5px" alignItems="center">
                            <LocationOnIcon color="primary" />
                            <Typography variant="body1">
                                {post.city}
                                {post.street && `, ${post.street}`}
                                {post.houseNumber && ` ${post.houseNumber}`}
                                {post.workLocation && ` - ${t(WorkLocationType[post.workLocation])}`}
                            </Typography>
                        </Box>
                        {post.postType === PostType.Work && (
                            <Box>
                                <Typography variant="subtitle1">
                                    {post.experienceRequired ? t('ExperienceRequired') : t('ExperienceNotRequired')}
                                </Typography>
                            </Box>
                        )}
                        {post.postType === PostType.Rent && (
                            <Box display="flex" flexDirection="column">
                                <Box display="flex" flexDirection="row" alignItems="center" gap="5px">
                                    <HomeIcon color="primary" />
                                    <Typography variant="subtitle1">
                                        {t(RentObjectType[post.rentObjectType!])}{" "}
                                        {post.area && (
                                            <>
                                                {post.area} m<sup>2</sup>
                                            </>
                                        )}
                                    </Typography>
                                </Box>
                                <Typography>
                                    {post.numberOfRooms && `${t('rooms')}: ${post.numberOfRooms} `}
                                    {post.floor && `${t('floor')}: ${post.floor}`}
                                </Typography>
                            </Box>
                        )}
                        <Box>
                            <Typography variant="caption">{post.shortDescription}</Typography>
                        </Box>
                    </Box>
                    {/* This Box controls the position of the price/salary */}
                    <Box display="flex" flexDirection="column" justifyContent="flex-end" alignItems="flex-end" padding="5px" flex="1">
                        {post.postType === PostType.Work && (
                            <>
                                <Typography>{t(ContractType[post.contract!])}</Typography>
                                <Typography>{t(WorkloadType[post.workload!])}</Typography>
                            </>
                        )}
                        {renderPriceAndType()}
                    </Box>
                </Box>
            </Box>
        </Box>
    );
});