import React from "react";
import ShowValue from "./ShowValue";
import Button from "./Button";
import './CSSFiles/App.css';

function FootballerTable({ footballers, onUpdateClick, onDeleteClick }) {
    return (
      <div className="footballer-table-container">
        <table className="table">
          <thead>
            <tr>
              <th>Footballer Id</th>
              <th>Footballer Name</th>
              <th>Date of Birth</th>
              <th>Club Id</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {footballers.map((footballer) => (
              <tr key={footballer.id}>
                <ShowValue value={footballer.id} />
                <ShowValue value={footballer.name} />
                <ShowValue value={footballer.dob} />
                <ShowValue value={footballer.clubId} />
                <td>
                  <Button text="Update" onClick={() => onUpdateClick(footballer)} />
                  <Button text="Delete" onClick={() => onDeleteClick(footballer.id)} />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  }
  
  export default FootballerTable;
