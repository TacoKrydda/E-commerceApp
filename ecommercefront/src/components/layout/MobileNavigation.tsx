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

  return (
    <div className={Styles.sidebar}>
      <div className={Styles.mobileNavItem} onClick={handleShowSidebar}>
        <div className={Styles.navItemContent}>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            height="48px"
            viewBox="0 -960 960 960"
            width="48px"
            fill="#e8eaed"
          >
            <path d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
          </svg>
        </div>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Pants")}
      >
        <div className={Styles.navItemContent}>Byxor</div>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("T-Shirt")}
      >
        <div className={Styles.navItemContent}>T-Shirt</div>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Hoodie")}
      >
        <div className={Styles.navItemContent}>Hoodies</div>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Jacket")}
      >
        <div className={Styles.navItemContent}>Jackor</div>
      </div>
      <div
        className={Styles.mobileNavItem}
        onClick={() => handleNavigateToCategory("Underwear")}
      >
        <div className={Styles.navItemContent}>Underkläder</div>
      </div>
    </div>
  );
};
