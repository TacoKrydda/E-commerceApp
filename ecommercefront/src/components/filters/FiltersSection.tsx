import React, { useEffect, useState } from "react";
import axiosInstance from "../../api/axiosInstance";
import FilterItem from "./FilterItem";
import Styles from "./FiltersSection.module.css";

interface FilterOption {
  id: number;
  name: string;
}

interface FiltersSectionProps {
  checkedFilters: { [key: string]: string[] };
  handleCheckboxChange: (filterKey: string, option: string) => void;
}

const FiltersSection: React.FC<FiltersSectionProps> = ({
  checkedFilters,
  handleCheckboxChange,
}) => {
  const [sizes, setSizes] = useState<FilterOption[]>([]);
  const [brands, setBrands] = useState<FilterOption[]>([]);
  const [colors, setColors] = useState<FilterOption[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFilters = async () => {
      try {
        const [sizesResponse, brandsResponse, colorsResponse] =
          await Promise.all([
            axiosInstance.get<FilterOption[]>("/filters/sizes"),
            axiosInstance.get<FilterOption[]>("/filters/brands"),
            axiosInstance.get<FilterOption[]>("/filters/colors"),
          ]);
        setSizes(sizesResponse.data);
        setBrands(brandsResponse.data);
        setColors(colorsResponse.data);
      } catch (err) {
        setError("Failed to load filters");
      } finally {
        setLoading(false);
      }
    };

    fetchFilters();
  }, []);

  if (loading) return <p>Loading filters...</p>;
  if (error) return <p>Error: {error}</p>;
  console.log(sizes);

  return (
    <section className={Styles.section}>
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
    </section>
  );
};

export default FiltersSection;
