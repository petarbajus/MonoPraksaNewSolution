import React from "react";
import Button from "./Button";
import './CSSFiles/App.css';

function ClubInsertForm({ club, onSubmit, onInputChange }) {
  return (
    <div className="club-insert-form">
      <form onSubmit={onSubmit}>
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
        <Button text="Submit" className="submit-button" />
      </form>
    </div>
  );
}

export default ClubInsertForm;