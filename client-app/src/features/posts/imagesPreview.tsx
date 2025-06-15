import { Box } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
import { useState } from "react";
import ImagesPreviewModal from "./imagesPreviewModal";


interface Props {
    mainImageUrl: string;
    postId: string;
}

export default function ImagesPreview({ mainImageUrl, postId }: Props) {

    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const onCloseModal = () => setIsModalOpen(false);
    const handleViewPhotos = (e: React.MouseEvent) => {
        e.stopPropagation();
        e.preventDefault();
        setIsModalOpen(true);
    }
    return (
        <Box
            flex="0"
            sx={{
                position: 'relative',
                width: '150px',
                height: '150px',
                borderRadius: '8px',

                cursor: 'pointer',
                '&:hover .overlay': {
                    opacity: 1,
                },
            }}

        >
            <img
                src={import.meta.env.VITE_API_URL.replace("api", "") + mainImageUrl}
                width="150px"
                height="150px"
      
            />
            <Box
                className="overlay"
                onClick={handleViewPhotos}
                sx={{
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    width: '100%',
                    height: '100%',
                    backgroundColor: 'rgba(0, 0, 0, 0.4)',
                    opacity: 0,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    transition: 'opacity 0.3s ease',

                }}
     
            >
                <SearchIcon sx={{ color: 'white', fontSize: 40, opacity:2}} />
            </Box>
            {isModalOpen && (
                <ImagesPreviewModal isOpen={isModalOpen} onClose={onCloseModal} postId={postId} />
            )
            }
        </Box>    
        )
    }