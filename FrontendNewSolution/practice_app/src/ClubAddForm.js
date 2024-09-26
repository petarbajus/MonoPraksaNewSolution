import React from "react";
import Button from "./Button";
import { useNavigate } from "react-router-dom"; 
import './CSSFiles/App.css';

function ClubAddForm({ club, onSubmit, onInputChange }) {
  const navigate = useNavigate();

  const handleFormSubmit = async (event) => {
    event.preventDefault();
    await onSubmit();
    navigate("/clubList");
  };

  return (
    <div className="universal-form-container">
      <form onSubmit={handleFormSubmit}>
        <div className="form-group">
          <label>Club Name:</label>
          <input
            type="text"
            name="name"
            value={club.name}
            onChange={onInputChange}
            placeholder="Enter Club Name"
            required
          />
        </div>
        <div className="form-group">
          <label>Characteristic Color:</label>
          <input
            type="text"
            name="characteristicColor"
            value={club.characteristicColor}
            onChange={onInputChange}
            placeholder="Enter Club Color"
            required
          />
        </div>
        <div className="form-group">
          <label>Foundation Date:</label>
          <input
            type="date"
            name="foundationDate"
            value={club.foundationDate}
            onChange={onInputChange}
            required
          />
        </div>
        <Button text="Submit" />
      </form>
    </div>
  );
}

export default ClubAddForm;
