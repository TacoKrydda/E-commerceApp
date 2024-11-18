import React from "react";
import FilterItem from "./FilterItem";
import Styles from "./FiltersSection.module.css";

interface FiltersSectionProps {
  checkedFilters: { [key: string]: string[] };
  handleCheckboxChange: (filterKey: string, option: string) => void;
}

// Import from backend instead of here
export const sizes = ["S", "M", "L", "XL"];
export const brands = ["Brand 1", "Brand 2", "Brand 3"];
export const colors = ["Red", "Green", "Blue"];
export const lengths = ["Short", "Medium", "Long"];
export const fits = ["Slim", "Regular", "Loose"];

const FiltersSection: React.FC<FiltersSectionProps> = ({
  checkedFilters,
  handleCheckboxChange,
}) => (
  <section className={Styles.filter}>
    <FilterItem
      filterKey="size"
      label="Size"
      options={sizes}
      checkedFilters={checkedFilters.size || []}
      handleCheckboxChange={handleCheckboxChange}
    />
    <FilterItem
      filterKey="brand"
      label="Brand"
      options={brands}
      checkedFilters={checkedFilters.brand || []}
      handleCheckboxChange={handleCheckboxChange}
    />
    <FilterItem
      filterKey="color"
      label="Colors"
      options={colors}
      checkedFilters={checkedFilters.color || []}
      handleCheckboxChange={handleCheckboxChange}
    />
    <FilterItem
      filterKey="length"
      label="Lengths"
      options={lengths}
      checkedFilters={checkedFilters.length || []}
      handleCheckboxChange={handleCheckboxChange}
    />
    <FilterItem
      filterKey="fit"
      label="Fits"
      options={fits}
      checkedFilters={checkedFilters.fit || []}
      handleCheckboxChange={handleCheckboxChange}
    />
  </section>
);

export default FiltersSection;
