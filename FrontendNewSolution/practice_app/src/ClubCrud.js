import React from "react";
import ClubTableForm from "./ClubTableForm";
import './CSSFiles/App.css';  // Importing ClubCrud-specific styles

const ClubCrud = () => {
  return (
    <div className="club-crud-container">
        <h1 className="club-crud-title">Club CRUD</h1>
        <ClubTableForm />
    </div>
  );
};

export default ClubCrud;
