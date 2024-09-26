import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // Import useNavigate
import FootballerAddForm from "./FootballerAddForm";
import Navbar from "./Navbar";
import './CSSFiles/App.css';
import api from './api/clubsfootballers';

function FootballerAdd() {
  const [footballer, setFootballer] = useState({
    name: "",
    dob: "",
    clubId: ""
  });
  const navigate = useNavigate(); // Using useNavigate for navigation

  // Handle the submission of new footballer
  const handleAddFootballer = async () => {
    try {
      const response = await api.post("api/Footballer/insertFootballer", footballer);
      if (response && response.status === 200) {
        alert("Footballer successfully added");
        navigate("/footballerList"); // Navigate back to the footballer list
      }
    } catch (err) {
      console.error("Error adding the footballer", err);
    }
  };

  // Handle form input changes
  const handleInputChange = (event) => {
    setFootballer({
      ...footballer,
      [event.target.name]: event.target.value
    });
  };

  return (
    <div className="footballer-add-container">
      <Navbar />
      <h1 className="form-title" >Add New Footballer</h1>
      <FootballerAddForm footballer={footballer} onSubmit={handleAddFootballer} onInputChange={handleInputChange} />
    </div>
  );
}

export default FootballerAdd;
