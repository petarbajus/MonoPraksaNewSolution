import React, { useState, useEffect } from "react";
import FootballerInsertForm from "./FootballerInsertForm";
import FootballerTable from "./FootballerTable.js";
import FootballerTableUpdateForm from "./FootballerTableUpdateForm";
import './CSSFiles/App.css'; 
import api from './api/clubsfootballers'; 

function FootballerTableForm() {
    const [footballer, setFootballer] = useState({ id: "", name: "", dob: "", clubId: "" });
    const [footballers, setFootballers] = useState([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [selectedFootballer, setSelectedFootballer] = useState(null);
  
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

    async function handleSubmit(event) {
      debugger
      event.preventDefault();
  
      const newFootballer = {
        name: footballer.name,
        dob: footballer.dob,
        clubId: footballer.clubId
      };
      try {
        console.log(newFootballer);
        const response = await api.post("api/Footballer/insertFootballer", newFootballer);
        if (response && response.status) {
          alert("Footballer successfully added");
          fetchFootballers(); // Refresh footballer list after addition
        }
      } catch (err) {
        console.error("Error adding the footballer", err);
      }
  
      setFootballer({ id: "", name: "", dob: "", clubId: "" });
    }
  
    function handleInputChange(event) {
      setFootballer({
        ...footballer,
        [event.target.name]: event.target.value,
      });
    }
  
    function handleUpdateClick(footballer) {
      setSelectedFootballer(footballer);
      setIsModalVisible(true);
    }

    async function handleUpdateSubmit(updatedFootballer) {
      try {
        const response = await api.put(`api/Footballer/updateFootballerById/${updatedFootballer.id}`, updatedFootballer);
        if (response && response.status) {
          alert("Footballer successfully updated");
          fetchFootballers();
        }
      } catch (err) {
        console.error("Error updating the footballer", err);
      }
    }
  
    function handleClose() {
      setSelectedFootballer(null);
      setIsModalVisible(false);
    }
  
    async function handleDeleteClick(footballerId) {
      try {
        const response = await api.delete(`api/Footballer/deleteFootballerById/${footballerId}`);
        if (response && response.status) {
          alert("Footballer successfully deleted");
          fetchFootballers();
        }
      } catch (err) {
        console.error("Error deleting the footballer", err);
      }
    }
  
    return (
      <div className="footballer-table-form">
        <FootballerInsertForm footballer={footballer} onSubmit={handleSubmit} onInputChange={handleInputChange} />
        <FootballerTable footballers={footballers} onUpdateClick={handleUpdateClick} onDeleteClick={handleDeleteClick} />
        <FootballerTableUpdateForm
          isVisible={isModalVisible}
          footballer={selectedFootballer}
          onClose={handleClose}
          onSubmit={handleUpdateSubmit}
        />
      </div>
    );
  }
  
  export default FootballerTableForm;
