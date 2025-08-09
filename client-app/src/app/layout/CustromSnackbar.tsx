import React from 'react';
import {Alert, Slide, SlideProps, Snackbar, Typography} from '@mui/material';
import {observer} from 'mobx-react-lite';

interface CustomSnackbarProps {
    open: boolean;
    onClose: () => void;
    message: string;
    severity?: 'success' | 'error' | 'warning' | 'info';
    autoHideDuration?: number;
    side?: 'right' | 'left' | 'center';
}

function SlideTransition(props: SlideProps) {
    return <Slide {...props} direction="up"/>;
}

const CustomSnackbar: React.FC<CustomSnackbarProps> = ({
                                                           open,
                                                           onClose,
                                                           message,
                                                           severity = 'info',
                                                           autoHideDuration = 3000,
                                                           side = 'right',
                                                       }) => {
    return (
        <Snackbar
            open={open}
            autoHideDuration={autoHideDuration}
            onClose={onClose}
            TransitionComponent={SlideTransition}
            anchorOrigin={{vertical: "bottom", horizontal: side}}
        >
            <Alert onClose={onClose} variant="filled" severity={severity}
                   sx={{width: '100%', backgroundColor: `${severity}.main`}}>
                <Typography variant="button">{message}</Typography>
            </Alert>
        </Snackbar>
    );
};

export default observer(CustomSnackbar);