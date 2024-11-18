import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axiosInstance from "../api/axiosInstance";

import Styles from "./ProductDetail.module.css";
import { ImageSlider } from "../components/products/ImageSlider";
import SizeDropdown from "../components/products/SizeDropdown";
import { ImageContainer } from "../components/products/ImageContainer";

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

const ProductDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<ProductDTO | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const response = await axiosInstance.get<ProductDTO>(`/Product/${id}`);
        setProduct(response.data);
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchProduct();
  }, [id]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!product) return <p>No product found</p>;

  return (
    <div className={Styles.detailsContainer}>
      <div className={Styles.productView}>
        <ImageSlider imageUrls={ImageContainer()} />
        <div className={Styles.infoSection}>
          <div className={Styles.infoContent}>
            <h1>{product.name}</h1>
            <h3>
              Varumärke: <a>{product.brand}</a>
            </h3>
            <h2>{product.price} kr</h2>
            <h3>Färger</h3>
            <div className={Styles.colorOptions}>{product.color}</div>
            <SizeDropdown size={[""]} />
            <div className={Styles.buySection}>
              <button>Handla</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductDetail;
