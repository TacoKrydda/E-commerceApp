import React from "react";
import Styles from "./ActiveFilters.module.css";

interface ActiveFiltersProps {
  activeFilters: [string, string][];
  removeActiveFilter: (filterKey: string, option: string) => void;
}

const ActiveFilters: React.FC<ActiveFiltersProps> = ({
  activeFilters,
  removeActiveFilter,
}) => {
  // Filtrera bort kategori innan vi renderar aktiva filter
  const filteredActiveFilters = activeFilters.filter(
    ([filterKey]) => filterKey !== "category"
  );

  return (
    <section className={Styles.activeFilterSection}>
      {filteredActiveFilters.length > 0 && (
        <div className={Styles.activeFilters}>
          {filteredActiveFilters.map(([filterKey, option], index) => (
            <p
              key={index}
              onClick={() => removeActiveFilter(filterKey, option)}
            >
              {`${filterKey}: ${option}`}
            </p>
          ))}
        </div>
      )}
    </section>
  );
};

export default ActiveFilters;
