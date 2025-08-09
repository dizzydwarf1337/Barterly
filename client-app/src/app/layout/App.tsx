import {Box, ThemeProvider} from '@mui/material'
import './index.css'
import {useEffect} from 'react';
import {Outlet} from 'react-router';
import Footer from './footer';
import NavBar from './navBar';
import {observer} from 'mobx-react-lite';
import useStore from '../stores/store';
import CustomSnackbar from './CustromSnackbar';

export default observer(function App() {
    const {uiStore} = useStore();
    const theme = uiStore.getTheme();
    useEffect(() => {
        document.body.style.backgroundColor = theme.palette.background.default;
    }, [theme]);


    return (
        <ThemeProvider theme={theme}>
            <Box display="flex" flexDirection="column" width="100%" height="100%" justifyContent="space-between">
                <NavBar/>
                <Box
                    display="flex"
                    flexDirection="column"
                    minHeight="100vh"
                    width="99vw"
                    mt={uiStore.isMobile ? "50px" : "20px"}
                >
                    <Box
                        flex="1"
                        display="flex"
                        flexDirection="column"
                        m="10px"
                        p={uiStore.isMobile ? "10px" : "36px"}
                        overflow="hidden"
                    >
                        <Outlet/>
                    </Box>

                    <Box
                        sx={{
                            backgroundColor: 'black',
                            color: 'white',
                            mt: "20px",
                            position: 'relative',
                            width: "100%"

                        }}
                    >
                        <Footer/>
                    </Box>

                    <CustomSnackbar
                        message={uiStore.snackbarMessage}
                        severity={uiStore.snackbarSeverity}
                        open={uiStore.snackbarOpen}
                        side={uiStore.snackbarPosition}
                        onClose={() => uiStore.closeSnackbar()}
                    />
                </Box>
            </Box>
        </ThemeProvider>
    )
})


