import React from "react";
import ClubCrud from "./ClubCrud"; // Import the Club CRUD functionality
import FootballerCrud from "./FootballerCrud";
import './CSSFiles/App.css';  // Importing Crud-specific styles

const Crud = () => {
  return (
    <div className="crud-container">
      <h1 className="title">CRUD Practice</h1>
      <div className="crud-sections">
        <ClubCrud />
        <FootballerCrud />
      </div>
    </div>
  );
};

export default Crud;
