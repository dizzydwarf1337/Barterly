import { observer } from "mobx-react-lite";
import { Typography, Card, CardContent } from "@mui/material";
import useStore from "../../../app/stores/store";
import { Category } from "../types/categoryTypes";

interface Props {
    category: Category;
}

export default observer(function CategoryListItem({ category }: Props) {
    const { uiStore } = useStore();

    const handleClick = () => {
        console.log("Navigate to category:", category.id);
        // navigate(`/categories/${category.id}`);
    };

    return (
        <Card
            onClick={handleClick}
            sx={{
                minWidth: uiStore.isMobile ? "120px" : "150px",
                minHeight: "80px",
                cursor: "pointer",
                transition: "all 0.2s ease-in-out",
                backgroundColor: "background.paper",
                '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: (theme) => theme.shadows[8],
                    backgroundColor: (theme) => theme.palette.action.hover,
                },
                '&:active': {
                    transform: 'translateY(-2px)',
                },
            }}
        >
            <CardContent
                sx={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    textAlign: "center",
                    height: "100%",
                    p: 2,
                    '&:last-child': {
                        pb: 2,
                    },
                }}
            >
                <Typography
                    variant={uiStore.isMobile ? "body2" : "body1"}
                    fontWeight="medium"
                    color="text.primary"
                    sx={{
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                        display: "-webkit-box",
                        WebkitLineClamp: 2,
                        WebkitBoxOrient: "vertical",
                    }}
                >
                    {uiStore.lang === "en" ? category.nameEN : category.namePL}
                </Typography>
            </CardContent>
        </Card>
    );
});