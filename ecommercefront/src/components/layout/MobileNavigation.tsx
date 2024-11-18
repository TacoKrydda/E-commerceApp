import React from "react";
import Styles from "./MobileNavigation.module.css";
import { useNavigate } from "react-router-dom";

interface MobileNavigationProps {
  handleShowSidebar: () => void;
}

export const MobileNavigation: React.FC<MobileNavigationProps> = ({
  handleShowSidebar,
}) => {
const navigate = useNavigate();

const handleNavigateToCategory = (category: string) => {
  navigate(`/products?category=${category}`);
  handleShowSidebar();
};

  // const mobileNavOpenOrClose = (category: string) => {
  //   onCategoryChange(category);
  //   handleShowSidebar();
  // };

  return (
    <div className={Styles.sidebar}>
      <div className={Styles.mobileNavItem} onClick={handleShowSidebar}>
        <a href="#">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            height="48px"
            viewBox="0 -960 960 960"
            width="48px"
            fill="#e8eaed"
          >
            <path d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
          </svg>
        </a>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Pants")}
      >
        <a href="#">Byxor</a>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("T-Shirts & Hoodies")}
      >
        <a href="#">T-Shirt & Hoodies</a>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Jacket")}
      >
        <a href="#">Jackor</a>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Underwear")}
      >
        <a href="#">Underkläder</a>
      </div>
    </div>
  );
};
