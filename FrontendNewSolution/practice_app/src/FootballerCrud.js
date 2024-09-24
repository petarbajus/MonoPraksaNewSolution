import React from "react";
import FootballerTableForm from "./FootballerTableForm";
import './CSSFiles/App.css';  // Importing FootballerCrud-specific styles

const FootballerCrud = () => {
    return (
      <div className="footballer-crud-container">
          <h1 className="footballer-crud-title">Footballer CRUD</h1>
          <FootballerTableForm />
      </div>
    );
  };
  
  export default FootballerCrud;
