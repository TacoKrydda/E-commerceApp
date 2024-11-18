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
  stock: number;
}

interface ProductListProps {
  handleShowSidebar: () => void;
  showSidebar: boolean;
}

const ProductList: React.FC<ProductListProps> = ({
  handleShowSidebar,
  showSidebar,
}) => {
  const [products, setProducts] = useState<ProductDTO[]>([]);
  const [filteredProducts, setFilteredProducts] = useState<ProductDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await axiosInstance.get<ProductDTO[]>("/Product");
        setProducts(response.data);
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  useEffect(() => {
    // Gruppera alla filterval efter nyckel
    const activeFilters: { [key: string]: string[] } = {};
    searchParams.forEach((value, key) => {
      if (!activeFilters[key]) {
        activeFilters[key] = [];
      }
      activeFilters[key].push(value);
    });

    setFilteredProducts(
      products.filter((product) =>
        Object.entries(activeFilters).every(([filterKey, options]) => {
          if (filterKey === "category") {
            return options.includes(product.category);
          }
          if (filterKey === "size") {
            return options.includes(product.size);
          }
          if (filterKey === "brand") {
            return options.includes(product.brand);
          }
          if (filterKey === "color") {
            return options.includes(product.color);
          }
          return true;
        })
      )
    );
  }, [searchParams, products]);

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
              <GridProduct products={filteredProducts} />
            </section>
          </main>
        </div>
      )}
    </div>
  );
};

export default ProductList;
