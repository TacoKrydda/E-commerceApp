import React from "react";
import { Link } from "react-router-dom";
import Styles from "./Home.module.css";

const Home: React.FC = () => {
  return (
    <div className={Styles.homeContainer}>
      <section className={Styles.introSection}>
        <h1>Välkommen till vår butik</h1>
        <p>
          Utforska vårt breda utbud av kläder för alla stilar och tillfällen. Vi
          erbjuder allt från basplagg till det senaste modet, med hög kvalitet
          och bra priser.
        </p>
      </section>

      <section className={Styles.categoriesSection}>
        <h2>Utforska våra kategorier</h2>
        <div className={Styles.categories}>
          <Link to="/products?category=Pants" className={Styles.categoryLink}>
            <div className={Styles.categoryCard}>
              <p>Byxor</p>
            </div>
          </Link>
          <Link to="/products?category=T-Shirt" className={Styles.categoryLink}>
            <div className={Styles.categoryCard}>
              <p>T-Shirts</p>
            </div>
          </Link>
          <Link to="/products?category=Hoodie" className={Styles.categoryLink}>
            <div className={Styles.categoryCard}>
              <p>Hoodies</p>
            </div>
          </Link>
          <Link to="/products?category=Jacket" className={Styles.categoryLink}>
            <div className={Styles.categoryCard}>
              <p>Jackor</p>
            </div>
          </Link>
          <Link
            to="/products?category=Underwear"
            className={Styles.categoryLink}
          >
            <div className={Styles.categoryCard}>
              <p>Underkläder</p>
            </div>
          </Link>
        </div>
      </section>
    </div>
  );
};

export default Home;
