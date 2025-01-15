import React, { useState, useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import Styles from "./ProductList.module.css";
import { DesktopNavigation } from "../components/layout/DesktopNavigation";
import { GridProduct } from "../components/products/GridProduct";
import { MobileNavigation } from "../components/layout/MobileNavigation";
import axiosInstance from "../api/axiosInstance";
import FilterManager from "../components/filters/FilterManager";

interface ProductDTO {
  id: number;
  name: string;
  brand: string;
  category: string;
  color: string;
  size: string;
  price: number;
  imagePath: string;
  stock: number;
}

interface ProductListProps {
  handleShowSidebar: () => void;
  showSidebar: boolean;
}

const fetchFilteredProducts = async (filters: { [key: string]: string[] }) => {
  const params = new URLSearchParams();

  Object.entries(filters).forEach(([key, values]) => {
    values.forEach((value) => params.append(key, value));
  });

  const response = await axiosInstance.get<ProductDTO[]>("/Product", {
    params,
  });

  return response.data;
};

const ProductList: React.FC<ProductListProps> = ({
  handleShowSidebar,
  showSidebar,
}) => {
  const [filteredProducts, setFilteredProducts] = useState<ProductDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const activeFilters: { [key: string]: string[] } = {};
        searchParams.forEach((value, key) => {
          if (!activeFilters[key]) {
            activeFilters[key] = [];
          }
          activeFilters[key].push(value);
        });

        const data = await fetchFilteredProducts(activeFilters);
        setFilteredProducts(data);
        setError(null); // Nollställ eventuellt tidigare fel
      } catch (err) {
        setError((err as Error).message);
        setFilteredProducts([]); // Säkerställ att listan är tom vid fel
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, [searchParams]);

  return (
    <div className={Styles.productContainer}>
      {loading && <p>Loading...</p>}
      {error && <p>Error: {error}</p>}
      {!loading && !error && (
        <div className={Styles.one}>
          <aside className={Styles.left}>
            <DesktopNavigation />
          </aside>
          {showSidebar && (
            <MobileNavigation handleShowSidebar={handleShowSidebar} />
          )}
          <main className={Styles.right}>
            <section className={Styles.rightContent}>
              <FilterManager />
              {filteredProducts.length > 0 ? (
                <GridProduct products={filteredProducts} />
              ) : (
                <p>No products found. Try adjusting your filters.</p>
              )}
            </section>
          </main>
        </div>
      )}
    </div>
  );
};

export default ProductList;
