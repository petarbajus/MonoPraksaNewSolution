import React, { useState, useEffect } from "react";
import ClubInsertForm from "./ClubInsertForm";
import ClubTable from "./ClubTable";
import ClubTableUpdateForm from "./ClubTableUpdateForm";
import './CSSFiles/App.css'; // Importing CSS for styling
import api from './api/clubsfootballers'; 

function ClubTableForm() {
  const [club, setClub] = useState({ id: "", name: "", characteristicColor: "", foundationDate: "" });
  const [clubs, setClubs] = useState([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedClub, setSelectedClub] = useState(null);

  const fetchClubs = async() => {
    try {
      const response = await api.get("api/Club/getClubs", {
        params: {
          sortBy: "ClubId",
          sortDirection: "DESC",
          recordsPerPage: 6,
          currentPage: 1,
        }
      });
      if(response && response.data) setClubs(response.data);
    } catch(err) {
      console.error("Error fetching clubs", err);
    }
  };

  useEffect(() => {
    fetchClubs();
  }, []);
    
  async function handleSubmit(event) {
  event.preventDefault();

  const newClub = {
    name: club.name,
    characteristicColor: club.characteristicColor,
    foundationDate: club.foundationDate
  };

  try {
    const response = await api.post("api/Club/insertClub", newClub);
    if (response && response.status) {
      alert("Club successfully added")
      fetchClubs();
    }
  } catch (err) {
    console.error("Error adding the club", err);
  }

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

  async function handleUpdateSubmit(updatedClub) {
    try {
      const response = await api.put(`api/Club/updateClubById/${updatedClub.id}`, updatedClub);
      if (response && response.status) {
        alert("Club successfully updated");
        fetchClubs();
      }
    } catch (err) {
      console.error("Error updating the club", err);
    }
  }

  function handleClose() {
    setSelectedClub(null);
    setIsModalVisible(false);
  }

  async function handleDeleteClick(clubId) {
    try {
      const response = await api.delete(`api/Club/deleteClubById/${clubId}`);
      if (response && response.status) {
        alert("Club successfully deleted");
        fetchClubs();
      }
    } catch (err) {
      console.error("Error deleting the club", err);
    }
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