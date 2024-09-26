import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // Import useNavigate
import ClubAddForm from "./ClubAddForm";
import Navbar from "./Navbar";
import './CSSFiles/App.css';
import api from './api/clubsfootballers';

function ClubAdd() {
  const [club, setClub] = useState({
    name: "",
    characteristicColor: "",
    foundationDate: ""
  });
  const navigate = useNavigate(); // Using useNavigate for navigation

  // Handle the submission of new club
  const handleAddClub = async () => {
    try {
      const response = await api.post("api/Club/insertClub", club);
      if (response && response.status === 200) {
        alert("Club successfully added");
        navigate("/clubList"); // Navigate back to the club list
      }
    } catch (err) {
      console.error("Error adding the club", err);
    }
  };

  // Handle form input changes
  const handleInputChange = (event) => {
    setClub({
      ...club,
      [event.target.name]: event.target.value
    });
  };

  return (
    <div className="club-add-container">
      <Navbar />
      <h1 className="form-title">Add New Club</h1>
      <ClubAddForm club={club} onSubmit={handleAddClub} onInputChange={handleInputChange} />
    </div>
  );
}

export default ClubAdd;
