import React, { useState, useRef, useEffect } from "react";
import Styles from "./FilterItem.module.css";
import { useMediaQuery } from "react-responsive";

interface FilterItemProps {
  filterKey: string;
  label: string;
  options: string[];
  checkedFilters: string[];
  handleCheckboxChange: (filterKey: string, option: string) => void;
}

const FilterItem: React.FC<FilterItemProps> = ({
  filterKey,
  label,
  options,
  checkedFilters,
  handleCheckboxChange,
}) => {
  const isMobile = useMediaQuery({ maxWidth: 767 });
  const [showDropdown, setShowDropdown] = useState<boolean>(false);
  const filterRef = useRef<HTMLDivElement>(null);

  const toggleDropdown = () => setShowDropdown((prev) => !prev);

  const handleOptionChange = (option: string) => {
    handleCheckboxChange(filterKey, option); // Skicka uppdatering för valt alternativ
    setShowDropdown(false);
  };

  const closeMobileFilter = () => {
    setShowDropdown(false);
  };

  // Stäng dropdown vid klick utanför
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        filterRef.current &&
        !filterRef.current.contains(event.target as Node)
      ) {
        setShowDropdown(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div ref={filterRef} className={Styles.filterItem}>
      <h3 onClick={toggleDropdown}>{label}</h3>
      {showDropdown && (
        <div className={Styles.filterDropDown}>
          <div
            onClick={closeMobileFilter}
            className={Styles.moblieCloseDropdown}
          >
            <a>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                height="3rem"
                viewBox="0 -960 960 960"
                width="3rem"
                fill="#e8eaed"
              >
                <path d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
              </svg>
            </a>
          </div>
          {options.map((option) => (
            <div
              key={option}
              className={Styles.option}
              onClick={() => handleOptionChange(option)}
            >
              <input
                type="checkbox"
                checked={checkedFilters.includes(option)} // Kontrollera om alternativet är markerat
                onChange={() => handleOptionChange(option)}
              />
              <label>{option}</label>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default FilterItem;
