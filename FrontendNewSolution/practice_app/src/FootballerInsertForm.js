import React from 'react';
import Button from './Button';
import './CSSFiles/App.css';

function FootballerInsertForm({ footballer, onSubmit, onInputChange }) {
  return (
    <form className="footballer-insert-form" onSubmit={onSubmit}>
      <div className="form-group">
        <label>Footballer Name:</label>
        <input
          type="text"
          name="name"
          value={footballer.name}
          onChange={onInputChange}
          placeholder="Enter Footballer Name"
        />
      </div>
      <div className="form-group">
        <label>Date of Birth:</label>
        <input
          type="date"
          name="DOB"
          value={footballer.DOB}
          onChange={onInputChange}
          placeholder="Enter Date of Birth"
        />
      </div>
      <div className="form-group">
        <label>Club ID:</label>
        <input
          type="text"
          name="ClubId"
          value={footballer.ClubId}
          onChange={onInputChange}
          placeholder="Enter Club ID"
        />
      </div>
      <Button text="Submit" />
    </form>
  );
}

export default FootballerInsertForm;
