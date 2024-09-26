import React, { useState, useEffect } from "react";
import Button from "./Button";
import './CSSFiles/App.css';
import api from './api/clubsfootballers'; // Assuming your API setup is here

function FootballerUpdateForm({ footballer, onSubmit }) {
  const [updatedFootballer, setUpdatedFootballer] = useState({
    name: "",
    dob: "",
    clubId: "",
  });
  const [clubs, setClubs] = useState([]); // State to hold list of clubs

  // Fetch clubs when the component mounts
  useEffect(() => {
    const fetchClubs = async () => {
      try {
        const response = await api.get("api/Club/getClubs", {
          params: {
            sortBy: "ClubId",
            sortDirection: "DESC",
            recordsPerPage: 6,
            currentPage: 1,
          },
        });
        if (response && response.data) {
          setClubs(response.data);
        }
      } catch (err) {
        console.error("Error fetching clubs", err);
      }
    };

    fetchClubs();
  }, []);

  // Pre-fill the form with existing footballer's information
  useEffect(() => {
    if (footballer) {
      setUpdatedFootballer({
        name: footballer.name || "",
        dob: footballer.dob || "",
        clubId: footballer.clubId || "",
      });
    }
  }, [footballer]);

  // Handle input changes
  function handleInputChange(event) {
    setUpdatedFootballer({
      ...updatedFootballer,
      [event.target.name]: event.target.value,
    });
  }

  // Handle form submit
  function handleSubmit(event) {
    event.preventDefault();
    onSubmit(updatedFootballer);
  }

  return (
    <div className="universal-form-container">
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Footballer Name:</label>
          <input
            type="text"
            name="name"
            value={updatedFootballer.name}
            onChange={handleInputChange}
            placeholder="Enter Footballer Name"
          />
        </div>
        <div className="form-group">
          <label>Date of Birth:</label>
          <input
            type="date"
            name="dob"
            value={updatedFootballer.dob}
            onChange={handleInputChange}
          />
        </div>
        <div className="form-group">
          <label>Select Club:</label>
          <select name="clubId" value={updatedFootballer.clubId} onChange={handleInputChange}>
            <option value="">Select Club</option>
            {clubs.map((club) => (
              <option key={club.id} value={club.id}>
                {club.name}
              </option>
            ))}
          </select>
        </div>
        <Button text="Update Footballer" />
      </form>
    </div>
  );
}

export default FootballerUpdateForm;
