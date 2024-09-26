import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom"; 
import ClubTable from "./ClubTable";
import Navbar from "./Navbar";
import './CSSFiles/App.css';
import api from './api/clubsfootballers';

function ClubList() {
  const [clubs, setClubs] = useState([]);
  const navigate = useNavigate();

  // Fetch all clubs on component mount
  const fetchClubs = async () => {
    try {
      const response = await api.get("api/Club/getClubs", {
        params: {
          sortBy: "ClubId",
          sortDirection: "DESC",
          recordsPerPage: 6,
          currentPage: 1,
        }
      });
      if (response && response.data) setClubs(response.data);
    } catch (err) {
      console.error("Error fetching clubs", err);
    }
  };

  useEffect(() => {
    fetchClubs();
  }, []);

  // Handle Delete Club
  async function handleDeleteClick(clubId) {
    try {
      const response = await api.delete(`api/Club/deleteClubById/${clubId}`);
      if (response && response.status === 200) {
        alert("Club successfully deleted");
        fetchClubs(); // Refresh the list after deletion
      }
    } catch (err) {
      console.error("Error deleting the club", err);
    }
  }

  // Navigate to Add Club Page
  const handleAddClubClick = () => {
    navigate("/clubAdd"); 
  };

  // Navigate to Update Club Page
  const handleUpdateClick = (clubId) => {
    navigate(`/clubUpdate/${clubId}`); 
  };

  return (
    <div className="club-list-container"> {/* Container for ClubList */}
      <Navbar />
      <h1 className="page-title">Club List</h1> {/* Aligned with CSS for centered title */}
      <div className="add-button-container"> {/* Centered Add Club Button */}
        <button className="add-button" onClick={handleAddClubClick}>Add New Club</button>
      </div>
      <ClubTable 
        clubs={clubs} 
        onUpdateClick={handleUpdateClick} 
        onDeleteClick={handleDeleteClick} 
      />
    </div>
  );
}

export default ClubList;
