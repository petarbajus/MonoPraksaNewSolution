import React from "react";
import ShowValue from "./ShowValue";
import Button from "./Button";
import './CSSFiles/App.css';

function ClubTable({ clubs, onUpdateClick, onDeleteClick }) {
  return (
    <div className="table-container">
      <table className="table">
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
              <ShowValue value={club.id} />
              <ShowValue value={club.name} />
              <ShowValue value={club.characteristicColor} />
              <ShowValue value={club.foundationDate} />
              <td>
                <Button text="Update" onClick={() => onUpdateClick(club)} />
                <Button text="Delete" onClick={() => onDeleteClick(club.id)} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ClubTable;
