import React, { useState, useEffect } from "react";
import FootballerInsertForm from "./FootballerInsertForm";
import FootballerTable from "./FootballerTable.js";
import FootballerTableUpdateForm from "./FootballerTableUpdateForm";
import './CSSFiles/App.css'; 

function FootballerTableForm() {
    const [footballer, setFootballer] = useState({ id: "", name: "", DOB: "", ClubId: "" });
    const [footballers, setFootballers] = useState([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [selectedFootballer, setSelectedFootballer] = useState(null);
  
    useEffect(() => {
      const storedFootballers = JSON.parse(localStorage.getItem("footballers")) || [];
      setFootballers(storedFootballers);
    }, []);
  
    function handleSubmit(event) {
      event.preventDefault();
      
      const storedFootballers = JSON.parse(localStorage.getItem("footballers")) || [];
      const lastFootballerId = JSON.parse(localStorage.getItem("LastFootballerId")) || 0;
      const newFootballerId = lastFootballerId + 1;
      
      const updatedFootballers = [...storedFootballers, { ...footballer, id: newFootballerId }];
      
      localStorage.setItem("footballers", JSON.stringify(updatedFootballers));
      localStorage.setItem("LastFootballerId", JSON.stringify(newFootballerId));
      
      setFootballers(updatedFootballers);
      setFootballer({ id: "", name: "", DOB: "", ClubId: "" });
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
  
    function handleUpdateSubmit(updatedFootballer) {
      const storedFootballers = JSON.parse(localStorage.getItem("footballers")) || [];
      const updatedFootballers = storedFootballers.map((footballer) =>
        footballer.id === updatedFootballer.id ? updatedFootballer : footballer
      );
      localStorage.setItem("footballers", JSON.stringify(updatedFootballers));
      setFootballers(updatedFootballers);
      setIsModalVisible(false);
    }
  
    function handleClose() {
      setSelectedFootballer(null);
      setIsModalVisible(false);
    }
  
    function handleDeleteClick(footballerId) {
      const storedFootballers = JSON.parse(localStorage.getItem("footballers")) || [];
      const updatedFootballers = storedFootballers.filter((footballer) => footballer.id !== footballerId);
      localStorage.setItem("footballers", JSON.stringify(updatedFootballers));
      setFootballers(updatedFootballers);
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
