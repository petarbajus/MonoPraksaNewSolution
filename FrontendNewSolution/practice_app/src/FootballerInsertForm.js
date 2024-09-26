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
          required
        />
      </div>
      <div className="form-group">
        <label>Date of Birth:</label>
        <input
          type="date"
          name="dob"
          value={footballer.dob}
          onChange={onInputChange}
          placeholder="Enter Date of Birth"
          required
        />
      </div>
      <div className="form-group">
        <label>Club ID:</label>
        <input
          type="text"
          name="clubId"
          value={footballer.clubId}
          onChange={onInputChange}
          placeholder="Enter Club ID"
          required
        />
      </div>
      <Button text="Submit" />
    </form>
  );
}

export default FootballerInsertForm;
