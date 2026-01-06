import { observer } from "mobx-react-lite";
import { useState, SyntheticEvent } from "react";
import { Box, Tab, Paper } from "@mui/material";
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import { useTranslation } from "react-i18next";
import PersonIcon from "@mui/icons-material/Person";
import PostAddIcon from "@mui/icons-material/PostAdd";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import UserProfilePage from "./UserProfilePage";
import UserPostsPage from "./UserPostsPage";
import UserOrderPage from "./UserOrderPage";

const UserPage = () => {
    const { t } = useTranslation();
    const [currentTab, setCurrentTab] = useState("1");

    const handleTabChange = (_event: SyntheticEvent, newValue: string) => {
        setCurrentTab(newValue);
    };

    return (
        <Box sx={{ display: "flex", gap: 2 }}>
            <TabContext value={currentTab}>
                <Paper
                    elevation={2}
                    sx={{
                        width: 250,
                        height: "fit-content",
                    }}
                >
                    <TabList
                        orientation="vertical"
                        onChange={handleTabChange}
                        sx={{
                            "& .MuiTab-root": {
                                alignItems: "flex-start",
                                textAlign: "left",
                                minHeight: 60,
                                py: 2,
                                px: 3,
                            },
                        }}
                    >
                        <Tab
                            icon={<PersonIcon />}
                            iconPosition="start"
                            label={t("user:ProfilePage")}
                            value="1"
                            sx={{ justifyContent: "flex-start" }}
                        />
                        <Tab
                            icon={<PostAddIcon />}
                            iconPosition="start"
                            label={t("user:PostsPage")}
                            value="2"
                            sx={{ justifyContent: "flex-start" }}
                        />
                        <Tab
                            icon={<ShoppingCartIcon />}
                            iconPosition="start"
                            label={t("user:OrdersPage")}
                            value="3"
                            sx={{ justifyContent: "flex-start" }}
                        />
                    </TabList>
                </Paper>

                <Box sx={{ flexGrow: 1 }}>
                    <TabPanel value="1">
                        <UserProfilePage/>
                    </TabPanel>
                    <TabPanel value="2">
                        <UserPostsPage/>                        
                    </TabPanel>
                    <TabPanel value="3">
                        <UserOrderPage/>
                    </TabPanel>
                </Box>
            </TabContext>
        </Box>
    );
};

export default observer(UserPage);