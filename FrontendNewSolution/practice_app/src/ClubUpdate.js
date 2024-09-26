import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom"; // Import useNavigate
import ClubUpdateForm from "./ClubUpdateForm";
import Navbar from "./Navbar";
import './CSSFiles/App.css'; 
import api from './api/clubsfootballers';

function ClubUpdate() {
  const { id } = useParams();
  const [club, setClub] = useState({
    name: "",
    characteristicColor: "",
    foundationDate: ""
  });
  const navigate = useNavigate(); // Using useNavigate for navigation

  // Fetch the club details using the id from the URL
  useEffect(() => {
    const fetchClub = async () => {
      try {
        const response = await api.get(`api/Club/getClubById/${id}`);
        if (response && response.data) {
          setClub(response.data);
        }
      } catch (err) {
        console.error("Error fetching the club", err);
      }
    };
    fetchClub();
  }, [id]);

  // Handle the submission of updated data
  const handleUpdateSubmit = async (updatedClub) => {
    try {
      const response = await api.put(`api/Club/updateClubById/${id}`, updatedClub);
      if (response && response.status === 200) {
        alert("Club successfully updated");
        navigate("/clubList"); // Navigate back to the club list
      }
    } catch (err) {
      console.error("Error updating the club", err);
    }
  };

  return (
    <div className="club-update-container">
      <Navbar />
      <h1 className="form-title">Update Club</h1>
      <ClubUpdateForm club={club} onSubmit={handleUpdateSubmit} />
    </div>
  );
}

export default ClubUpdate;
