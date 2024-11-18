import React from "react";
import { useNavigate } from "react-router-dom";

export const StartPage = () => {
  const navigate = useNavigate();

  const handleNavigateToCategory = (category: string) => {
    navigate(`/test2?category=${category}`);
  };

  return (
    <div>
      <button onClick={() => handleNavigateToCategory("T-Shirt")}>
        Visa T-Shirts
      </button>
      <button onClick={() => handleNavigateToCategory("Pants")}>
        Visa Byxor
      </button>
    </div>
  );
};
