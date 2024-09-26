import React, { useState, useEffect } from 'react';
import Button from "./Button";
import api from './api/clubsfootballers';

function FootballerAddForm({ footballer, onSubmit, onInputChange }) {
  const [clubs, setClubs] = useState([]);

  // Fetch clubs when the component mounts
  useEffect(() => {
    const fetchClubs = async () => {
      try {
        const response = await api.get("api/Club/getClubs", {
          params: {
            sortBy: "ClubId",            // Sorting by ClubId
            sortDirection: "DESC",       // Sorting in descending order
            recordsPerPage: 6,           // Limit of 6 clubs per page
            currentPage: 1,              // Always fetch the first page
          }
        });

        if (response && response.data) {
          setClubs(response.data);       // Store the fetched clubs in state
        }
      } catch (err) {
        console.error("Error fetching clubs", err);
      }
    };

    fetchClubs(); // Call the fetch function when the component mounts
  }, []);

  const handleFormSubmit = async (event) => {
    event.preventDefault();
    await onSubmit();
  };

  return (
    <div className="universal-form-container">
      <form onSubmit={handleFormSubmit}>
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
            required
          />
        </div>
        <div className="form-group">
          <label>Select Club:</label>
          <select name="clubId" value={footballer.clubId} onChange={onInputChange}>
            <option value="">Select Club</option>
            {clubs.map((club) => (
              <option key={club.id} value={club.id}>
                {club.name}
              </option>
            ))}
          </select>
        </div>
        <Button text="Submit" />
      </form>
    </div>
  );
}

export default FootballerAddForm;
