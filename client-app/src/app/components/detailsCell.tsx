import PreviewIcon from '@mui/icons-material/Preview';
import { IconButton } from '@mui/material';
import { CellContext } from '@tanstack/react-table';
import { useNavigate } from 'react-router';

export const DetailsCell = <T extends { id: string }>(props: CellContext<T, unknown>) => {
    const navigate = useNavigate();
    return (
        <IconButton
            aria-label="Go to details"
            {...props}
            onClick={() => {
                navigate(`${props.row.original.id}/details`);
            }}
        >
            <PreviewIcon />
        </IconButton>
    );
};