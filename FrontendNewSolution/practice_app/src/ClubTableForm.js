import React, { useState, useEffect } from "react";
import ClubInsertForm from "./ClubInsertForm";
import ClubTable from "./ClubTable";
import ClubTableUpdateForm from "./ClubTableUpdateForm";
import './CSSFiles/App.css'; // Importing CSS for styling

function ClubTableForm() {
  const [club, setClub] = useState({ id: "", name: "", characteristicColor: "", foundationDate: "" });
  const [clubs, setClubs] = useState([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedClub, setSelectedClub] = useState(null);

  // Load clubs from localStorage when component mounts
  useEffect(() => {
    const storedClubs = JSON.parse(localStorage.getItem("clubs")) || [];
    setClubs(storedClubs);
  }, []);

  function handleSubmit(event) {
    event.preventDefault();
    
    const storedClubs = JSON.parse(localStorage.getItem("clubs")) || [];

    const lastClubId = JSON.parse(localStorage.getItem("LastClubId")) || 0;
    
    const newClubId = lastClubId + 1;
    
    const updatedClubs = [...storedClubs, { ...club, id: newClubId }];
    
    localStorage.setItem("clubs", JSON.stringify(updatedClubs));
    localStorage.setItem("LastClubId", JSON.stringify(newClubId));
    
    setClubs(updatedClubs);
    
    setClub({ id: "", name: "", characteristicColor: "", foundationDate: "" });
  }

  function handleInputChange(event) {
    setClub({
      ...club,
      [event.target.name]: event.target.value,
    });
  }

  function handleUpdateClick(club) {
    setSelectedClub(club);
    setIsModalVisible(true);
  }

  function handleUpdateSubmit(updatedClub) {
    const storedClubs = JSON.parse(localStorage.getItem("clubs")) || [];
    const updatedClubs = storedClubs.map((club) =>
      club.id === updatedClub.id ? updatedClub : club
    );
    localStorage.setItem("clubs", JSON.stringify(updatedClubs));
    setClubs(updatedClubs);
    setIsModalVisible(false);
  }

  function handleClose() {
    setSelectedClub(null);
    setIsModalVisible(false);
  }

  function handleDeleteClick(clubId) {
    const storedClubs = JSON.parse(localStorage.getItem("clubs")) || [];
    const updatedClubs = storedClubs.filter((club) => club.id !== clubId);
    localStorage.setItem("clubs", JSON.stringify(updatedClubs));
    setClubs(updatedClubs);
  }

  return (
    <div className="club-table-form">
      <ClubInsertForm club={club} onSubmit={handleSubmit} onInputChange={handleInputChange} />
      <ClubTable clubs={clubs} onUpdateClick={handleUpdateClick} onDeleteClick={handleDeleteClick} />
      <ClubTableUpdateForm
        isVisible={isModalVisible}
        club={selectedClub}
        onClose={handleClose}
        onSubmit={handleUpdateSubmit}
      />
    </div>
  );
}

export default ClubTableForm;