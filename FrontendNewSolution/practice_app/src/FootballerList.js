import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import FootballerTable from "./FootballerTable";
import Navbar from "./Navbar";
import FootballerDetailsModal from "./FootballerDetailsModal"; // Modal for details
import './CSSFiles/App.css';
import api from './api/clubsfootballers';

function FootballerList() {
  const [footballers, setFootballers] = useState([]);
  const [selectedFootballer, setSelectedFootballer] = useState(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const navigate = useNavigate();

  const fetchFootballers = async () => {
    try {
      const response = await api.get("api/Footballer/getFootballers", {
        params: {
          sortBy: "FootballerId",
          sortDirection: "DESC",
          recordsPerPage: 6,
          currentPage: 1,
        }
      });
      if (response && response.data) setFootballers(response.data);
    } catch (err) {
      console.error("Error fetching footballers", err);
    }
  };

  useEffect(() => {
    fetchFootballers();
  }, []);

  async function handleDeleteClick(footballerId) {
    try {
      const response = await api.delete(`api/Footballer/deleteFootballerById/${footballerId}`);
      if (response && response.status === 200) {
        alert("Footballer successfully deleted");
        fetchFootballers(); 
      }
    } catch (err) {
      console.error("Error deleting the footballer", err);
    }
  }

  const handleAddFootballerClick = () => {
    navigate("/footballerAdd");
  };

  const handleUpdateClick = (footballerId) => {
    navigate(`/footballerUpdate/${footballerId}`);
  };

  const handleDetailsClick = (footballer) => {
    setSelectedFootballer(footballer);
    setIsModalVisible(true);
  };

  return (
    <div className="footballer-list-container">
      <Navbar />
      <h1 className="page-title">Footballer List</h1>
      <div className="add-button-container">
        <button className="add-button" onClick={handleAddFootballerClick}>Add New Footballer</button>
      </div>
      <FootballerTable 
        footballers={footballers} 
        onUpdateClick={handleUpdateClick} 
        onDeleteClick={handleDeleteClick}
        onDetailsClick={handleDetailsClick} 
      />
      {isModalVisible && 
        <FootballerDetailsModal footballer={selectedFootballer} onClose={() => setIsModalVisible(false)} />
      }
    </div>
  );
}

export default FootballerList;
