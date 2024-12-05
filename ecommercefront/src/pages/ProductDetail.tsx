import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axiosInstance from "../api/axiosInstance";

import Styles from "./ProductDetail.module.css";
import { ImageSlider } from "../components/products/ImageSlider";
import SizeDropdown from "../components/products/SizeDropdown";
import { ImageContainer } from "../components/products/ImageContainer";
import { CartItem } from "../components/layout/Cart";

interface ProductDTO {
  id: number;
  name: string;
  brand: string;
  category: string;
  color: string;
  size: string;
  price: number;
  image:string;
  stock: number;
}

interface ProductDetailProps {
  handleAddToCart: (item: CartItem) => void;
}

const ProductDetail: React.FC<ProductDetailProps> = ({ handleAddToCart }) => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [product, setProduct] = useState<ProductDTO | null>(null);
  const [sizes, setSizes] = useState<string[]>([]);
  const [colors, setColors] = useState<string[]>([]);
  const [allProducts, setAllProducts] = useState<ProductDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProductData = async () => {
      try {
        const productResponse = await axiosInstance.get<ProductDTO>(
          `/Product/${id}`
        );
        setProduct(productResponse.data);

        // Hämta alla produkter med samma namn
        const sizesResponse = await axiosInstance.get<ProductDTO[]>(
          `/Product/by-name?name=${productResponse.data.name}`
        );
        setAllProducts(sizesResponse.data);

        // Filtrera produkter med samma färg som den aktuella
        const filteredProducts = sizesResponse.data.filter(
          (p) => p.color === productResponse.data.color
        );

        // Hämta unika storlekar från de filtrerade produkterna
        const uniqueSizes = Array.from(
          new Set(filteredProducts.map((p) => p.size))
        );
        setSizes(uniqueSizes);

        // Hämta färger exklusive den aktuella färgen
        const uniqueColors = Array.from(
          new Set(sizesResponse.data.map((p) => p.color))
        ).filter((color) => color !== productResponse.data.color);
        setColors(uniqueColors);
      } catch (err) {
        setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchProductData();
  }, [id]);

  const handleBuyClick = () => {
    if (product) {
      const cartItem: CartItem = {
        id: product.id,
        name: product.name,
        color: product.color,
        size: product.size,
        price: product.price,
        quantity: 1,
        stock: product.stock,
      };
      handleAddToCart(cartItem); // Lägg till produkten i kundvagnen
    }
  };

  const handleColorClick = (color: string) => {
    // Hitta rätt produkt baserat på färg
    const selectedProduct = allProducts.find((p) => p.color === color);

    if (selectedProduct) {
      // Navigera till rätt produkt-ID
      navigate(`/product/${selectedProduct.id}`);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!product) return <p>No product found</p>;
  console.log(colors);

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
            <h3>Färg: {product.color}</h3>
            <h2>{product.price} kr</h2>
            <h3>Färger</h3>
            <div className={Styles.colorOptions}>
              {colors.map((color, index) => (
                <button
                  key={index}
                  style={{
                    backgroundColor: color,
                    border: "1px solid #000",
                    width: "30px",
                    height: "30px",
                    margin: "5px",
                    borderRadius: "50%",
                    cursor: "pointer",
                  }}
                  aria-label={`Select ${color}`}
                  onClick={() => handleColorClick(color)}
                />
              ))}
            </div>
            <SizeDropdown size={sizes} />
            <div className={Styles.buySection}>
              <button onClick={handleBuyClick}>Handla</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductDetail;
