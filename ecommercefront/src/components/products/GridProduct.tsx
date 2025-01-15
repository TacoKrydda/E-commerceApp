import React from "react";
import { Link } from "react-router-dom";
import Styles from "./GridProduct.module.css";

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

interface ProductGridProps {
  products: ProductDTO[];
}

export const GridProduct: React.FC<ProductGridProps> = ({ products }) => {
  return (
    <section className={Styles.products}>
      <div className={Styles.list}>
        {products.map((product) => (
          <div key={product.id} className={Styles.item}>
            <Link to={`/product/${product.id}`}>
              <img
                className={Styles.image}
                src={`${process.env.REACT_APP_API_URL}/${product.imagePath}`}
                alt={product.name}
              />
            </Link>

            <div className={Styles.productInfo}>
              <Link to={`/product/${product.id}`}>
                <p>{product.name}</p>
              </Link>
              <p>{product.brand}</p>
              <p>{product.price}</p>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
};
