import React from "react";
import ShowValue from "./ShowValue";
import Button from "./Button";
import { useNavigate } from "react-router-dom";
import './CSSFiles/App.css';

function FootballerTable({ footballers, onUpdateClick, onDeleteClick, onDetailsClick }) {
  const navigate = useNavigate();

  return (
    <div className="universal-table-container">
      <table className="universal-table">
        <thead>
          <tr>
            <th>Footballer Id </th>
            <th>Footballer Name</th>
            <th>Date of Birth</th>
            <th>Club ID</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {footballers.map((footballer) => (
            <tr key={footballer.id}>
              <td className="table-cell">
                <ShowValue value={footballer.id} />
              </td>
              <td className="table-cell">
                <ShowValue value={footballer.name} />
              </td>
              <td className="table-cell">
                <ShowValue value={footballer.dob} />
              </td>
              <td className="table-cell">
                <ShowValue value={footballer.clubId} />
              </td>
              <td className="table-cell">
                <div className="action-buttons">
                  <Button text="Update" onClick={() => navigate(`/footballerUpdate/${footballer.id}`)} />
                  <Button text="Delete" onClick={() => onDeleteClick(footballer.id)} />
                  <Button text="Details" onClick={() => onDetailsClick(footballer)} />
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default FootballerTable;
