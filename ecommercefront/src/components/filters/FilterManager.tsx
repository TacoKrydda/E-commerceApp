import React, { useEffect, useState } from "react";
import FiltersSection from "../filters/FiltersSection";
import ActiveFilters from "../filters/ActiveFilters";
import { useSearchParams } from "react-router-dom";

const FilterManager: React.FC = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [checkedFilters, setCheckedFilters] = useState<{
    [key: string]: string[];
  }>({
    size: [],
    brand: [],
    price: [],
    color: [],
    length: [],
    fit: [],
  });

  // Hantera ändringar när en användare väljer/avmarkerar ett filter
  const handleCheckboxChange = (filterKey: string, option: string) => {
    const currentOptions = checkedFilters[filterKey] || [];
    const isOptionSelected = currentOptions.includes(option);

    // Uppdatera checkedFilters med det nya valet
    const updatedOptions = isOptionSelected
      ? currentOptions.filter((item) => item !== option) // Ta bort om markerad
      : [...currentOptions, option]; // Lägg till om inte markerad

    setCheckedFilters((prevState) => ({
      ...prevState,
      [filterKey]: updatedOptions,
    }));

    // Uppdatera URL-parametrarna baserat på det nya valet
    searchParams.delete(filterKey);
    updatedOptions.forEach((opt) => searchParams.append(filterKey, opt));
    setSearchParams(searchParams);
  };

  // Ta bort ett filter från URL och state
  const removeActiveFilter = (filterKey: string, option: string) => {
    const updatedOptions = checkedFilters[filterKey].filter(
      (item) => item !== option
    );

    setCheckedFilters((prevState) => ({
      ...prevState,
      [filterKey]: updatedOptions,
    }));

    // Uppdatera URL-parametrarna
    searchParams.delete(filterKey);
    updatedOptions.forEach((opt) => searchParams.append(filterKey, opt));
    setSearchParams(searchParams);
  };

  // Nollställ filter när kategorin ändras
  useEffect(() => {
    const category = searchParams.get("category");

    setCheckedFilters({
      size: [],
      brand: [],
      price: [],
      color: [],
      length: [],
      fit: [],
    });

    // Ta bort alla filter-relaterade URL-parametrar, men behåll kategorin
    Array.from(searchParams.keys()).forEach((key) => {
      if (key !== "category") {
        searchParams.delete(key);
      }
    });
    setSearchParams(searchParams);
  }, [searchParams.get("category")]); // Kör när `category` ändras

  return (
    <>
      <FiltersSection
        checkedFilters={checkedFilters}
        handleCheckboxChange={handleCheckboxChange}
      />
      <ActiveFilters
        activeFilters={Array.from(searchParams.entries())}
        removeActiveFilter={removeActiveFilter}
      />
    </>
  );
};

export default FilterManager;
