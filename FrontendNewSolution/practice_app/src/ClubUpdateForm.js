import React, { useState, useEffect } from "react";
import Button from "./Button";
import './CSSFiles/App.css';

function ClubUpdateForm({ club, onSubmit }) {
  const [updatedClub, setUpdatedClub] = useState({
    name: "",
    characteristicColor: "",
    foundationDate: "",
  });

  useEffect(() => {
    if (club) {
      setUpdatedClub({
        name: club.name || "",
        characteristicColor: club.characteristicColor || "",
        foundationDate: club.foundationDate || "",
      });
    }
  }, [club]);

  function handleInputChange(event) {
    setUpdatedClub({
      ...updatedClub,
      [event.target.name]: event.target.value,
    });
  }

  function handleSubmit(event) {
    event.preventDefault();
    onSubmit(updatedClub);
  }

  return (
    <div className="universal-form-container">
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Club Name:</label>
          <input
            type="text"
            name="name"
            value={updatedClub.name}
            onChange={handleInputChange}
            placeholder="Enter Club Name"
          />
        </div>
        <div className="form-group">
          <label>Characteristic Color:</label>
          <input
            type="text"
            name="characteristicColor"
            value={updatedClub.characteristicColor}
            onChange={handleInputChange}
            placeholder="Enter Club Color"
          />
        </div>
        <div className="form-group">
          <label>Foundation Date:</label>
          <input
            type="date"
            name="foundationDate"
            value={updatedClub.foundationDate}
            onChange={handleInputChange}
          />
        </div>
        <Button text="Update Club" />
      </form>
    </div>
  );
}

export default ClubUpdateForm;
