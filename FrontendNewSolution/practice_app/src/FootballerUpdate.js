import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import FootballerUpdateForm from "./FootballerUpdateForm";
import Navbar from "./Navbar";
import './CSSFiles/App.css'; 
import api from './api/clubsfootballers';

function FootballerUpdate() {
  const { id } = useParams(); // Get the footballer ID from the URL params
  const [footballer, setFootballer] = useState({
    name: "",
    dob: "",
    clubId: ""
  });
  const navigate = useNavigate();

  // Fetch the footballer details using the ID from the URL
  useEffect(() => {
    const fetchFootballer = async () => {
      try {
        const response = await api.get(`api/Footballer/getFootballerById/${id}`);
        if (response && response.data) {
          setFootballer(response.data);
        }
      } catch (err) {
        console.error("Error fetching the footballer", err);
      }
    };
    fetchFootballer();
  }, [id]);

  // Handle the submission of updated data
  const handleUpdateSubmit = async (updatedFootballer) => {
    try {
      const response = await api.put(`api/Footballer/updateFootballerById/${id}`, updatedFootballer);
      if (response && response.status === 200) {
        alert("Footballer successfully updated");
        navigate("/footballerList"); // Navigate back to the footballer list after update
      }
    } catch (err) {
      console.error("Error updating the footballer", err);
    }
  };

  return (
    <div className="footballer-update-container">
      <Navbar />
      <h1 className="form-title">Update Footballer</h1>
      <FootballerUpdateForm footballer={footballer} onSubmit={handleUpdateSubmit} />
    </div>
  );
}

export default FootballerUpdate;
