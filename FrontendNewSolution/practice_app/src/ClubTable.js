import React, { useState } from "react";
import ShowValue from "./ShowValue";
import Button from "./Button";
import { useNavigate } from "react-router-dom";
import './CSSFiles/App.css';
import ClubDetailsModal from "./ClubDetailsModal";

function ClubTable({ clubs, onUpdateClick, onDeleteClick }) {
  const [selectedClub, setSelectedClub] = useState(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const navigate = useNavigate();

  const handleDetailsClick = (club) => {
    setSelectedClub(club);
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setSelectedClub(null);
  };

  return (
    <div className="universal-table-container">
      <table className="universal-table">
        <thead>
          <tr>
            <th>Club Id</th>
            <th>Club Name</th>
            <th>Characteristic Color</th>
            <th>Foundation Date</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {clubs.map((club) => (
            <tr key={club.id}>
              <td className="table-cell">
                <ShowValue value={club.id} />
              </td>
              <td className="table-cell">
                <ShowValue value={club.name} />
              </td>
              <td className="table-cell">
                <ShowValue value={club.characteristicColor} />
              </td>
              <td className="table-cell">
                <ShowValue value={club.foundationDate} />
              </td>
              <td className="table-cell">
                <div className="action-buttons">
                  <Button text="Details" onClick={() => handleDetailsClick(club)} />
                  <Button text="Update" onClick={() => navigate(`/clubUpdate/${club.id}`)} />
                  <Button text="Delete" onClick={() => onDeleteClick(club.id)} />
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {isModalVisible && (
        <ClubDetailsModal club={selectedClub} onClose={closeModal} />
      )}
    </div>
  );
}

export default ClubTable;
