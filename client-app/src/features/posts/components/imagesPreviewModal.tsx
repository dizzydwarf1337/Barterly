import {Box, Modal} from "@mui/material";
import {useEffect, useState} from "react";

import {Carousel} from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import postApi from "../api/postApi";

interface Props {
    postId: string;
    isOpen: boolean;
    onClose: () => void;
}

export default function ImagesPreviewModal({postId, isOpen, onClose}: Props) {

    const [images, setImages] = useState<string[]>([]);

    useEffect(() => {
        const res = postApi.getPostImages({ postId });
        res.then(imageDto => {
            setImages([imageDto?.value.mainImageUrl, ...(imageDto?.value.secondaryImagesUrl || [])])
        })
    }, [postId])


    return (
        <Modal open={isOpen} onClose={(e: React.MouseEvent) => {
            setImages([]);
            e.stopPropagation();
            onClose();
        }}>
            <Box
                onClick={(e) => e.stopPropagation()}
                sx={{
                    position: "absolute",
                    top: "50%",
                    left: "50%",
                    transform: "translate(-50%, -50%)",
                    bgcolor: "background.paper",
                    boxShadow: 24,
                    p: 2,
                    borderRadius: 2,
                    maxWidth: 600,
                    outline: "none"
                }}
            >
                <Carousel
                    showThumbs={false}
                    dynamicHeight={true}

                    emulateTouch
                >
                    {images.map((imgUrl, index) => (
                        <div key={index}>
                            <img
                                src={import.meta.env.VITE_API_URL.replace("api", "") + imgUrl}
                                alt={`Image ${index}`}
                                style={{borderRadius: "8px", maxHeight: "650px", objectFit: "contain"}}
                            />
                        </div>
                    ))}
                </Carousel>
            </Box>
        </Modal>
    )
}