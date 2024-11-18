import React from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import Styles from "./DesktopNavigation.module.css";

export const DesktopNavigation: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const selectedCategory = searchParams.get("category");

  const handleNavigateToCategory = (category: string) => {
    navigate(`/products?category=${category}`);
  };

  return (
    <div className={Styles.leftAside}>
      {["Pants", "T-Shirt", "Hoodie", "Jacket", "Underwear"].map(
        (category) => (
          <nav
            key={category}
            onClick={() => handleNavigateToCategory(category)}
            className={`${Styles.category} ${
              selectedCategory === category ? Styles.active : ""
            }`}
          >
            <p>{category}</p>
          </nav>
        )
      )}
    </div>
  );
};
