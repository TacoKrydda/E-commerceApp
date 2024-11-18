import React, { useState, useRef, useEffect } from "react";
import { useMediaQuery } from "react-responsive";
import Styles from "./ImageSlider.module.css";
import ScrollToTop from "../layout/ScrollToTop";

interface ImageSliderProps {
  imageUrls: string[];
}

export const ImageSlider: React.FC<ImageSliderProps> = ({ imageUrls }) => {
  const [imageIndex, setImageIndex] = useState(0);
  const isDesktop = useMediaQuery({ minWidth: "48rem" });
  const isMobile = useMediaQuery({ maxWidth: 767 });
  const [isFullscreen, setIsFullscreen] = useState(false);
  const [isZoomed, setIsZoomed] = useState(false);

  const imageRefs = useRef<(HTMLImageElement | null)[]>([]);

  const [startPosition, setStartPosition] = useState<number | null>(null);
  const [currentTranslate, setCurrentTranslate] = useState(0);
  const [dragging, setDragging] = useState(false);
  const galleryRef = useRef<HTMLDivElement | null>(null);

  const handleMouseDown = (e: React.MouseEvent | React.TouchEvent) => {
    const startPos = "touches" in e ? e.touches[0].clientX : e.clientX;
    setStartPosition(startPos);
    setDragging(true);
  };

  const handleMouseMove = (e: React.MouseEvent | React.TouchEvent) => {
    if (!dragging || startPosition === null) return;

    const currentPos = "touches" in e ? e.touches[0].clientX : e.clientX;
    const delta = currentPos - startPosition;
    setCurrentTranslate(delta);
  };

  const handleMouseUp = () => {
    if (!dragging) return;

    if (currentTranslate > 50 && imageIndex > 0) {
      setImageIndex(imageIndex - 1); // Dra åt höger för föregående bild
    } else if (currentTranslate < -50 && imageIndex < imageUrls.length - 1) {
      setImageIndex(imageIndex + 1); // Dra åt vänster för nästa bild
    }

    // Återställ till utgångsläget
    setStartPosition(null);
    setCurrentTranslate(0);
    setDragging(false);
  };

  const handleTouchEnd = handleMouseUp;

  // Scrolla till aktuell bild i fullskärmsläge
  useEffect(() => {
    if (isFullscreen && imageRefs.current[imageIndex]) {
      imageRefs.current[imageIndex]?.scrollIntoView({
        behavior: "smooth",
        block: "center",
      });
    }
  }, [isFullscreen, imageIndex]);

  const toggleZoom = () => {
    setIsZoomed(!isZoomed);
  };

  const handleFullscreen = () => {
    setIsZoomed(false);
    setIsFullscreen(!isFullscreen);
  };

  return (
    <div className={Styles.galleryContainer}>
      <div className={Styles.imageScetion}>
        <div className={Styles.miniGallery}>
          {imageUrls.map((_, index) => (
            <img
              key={index}
              src={imageUrls[index]}
              onClick={() => setImageIndex(index)}
            />
          ))}
        </div>
        <div
          className={Styles.gallery}
          ref={galleryRef}
          onMouseDown={handleMouseDown}
          onMouseMove={handleMouseMove}
          onMouseUp={handleMouseUp}
          onMouseLeave={handleMouseUp}
          onTouchStart={handleMouseDown}
          onTouchMove={handleMouseMove}
          onTouchEnd={handleTouchEnd}
        >
          {imageUrls.map((_, index) => (
            <img
              key={index}
              src={imageUrls[index]}
              style={{ translate: `${-100 * imageIndex}%` }}
              onClick={isDesktop ? handleFullscreen : undefined}
            />
          ))}
        </div>
        <div className={Styles.galleryJump}>
          {imageUrls.map((_, index) => (
            <button
              key={index}
              onClick={() => setImageIndex(index)}
              className={index === imageIndex ? Styles.active : ""}
            ></button>
          ))}
        </div>
      </div>
      {isFullscreen && (
        <>
          <ScrollToTop />
          <a href="#" className={Styles.closeButton} onClick={handleFullscreen}>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              height="100%"
              viewBox="0 -960 960 960"
              width="100%"
              fill="#e8eaed"
            >
              <path d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
            </svg>
          </a>
          <div
            className={`${Styles.fullScreen} ${
              isZoomed ? Styles.zoomedIn : ""
            }`}
          >
            {imageUrls.map((url, index) => (
              <img
                key={index}
                src={url}
                onClick={toggleZoom}
                ref={(el) => (imageRefs.current[index] = el)}
              />
            ))}
          </div>
        </>
      )}
    </div>
  );
};
