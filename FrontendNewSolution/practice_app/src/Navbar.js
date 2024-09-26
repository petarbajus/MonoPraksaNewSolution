import React from 'react';
import { Link } from 'react-router-dom';
import './CSSFiles/App.css';

const Navbar = () => {
  return (
    <nav className="navbar">
      <ul>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/clubList">Club List</Link></li>
        <li><Link to="/clubAdd">Add Club</Link></li>
        <li><Link to="/footballerList">Footballer List</Link></li>
        <li><Link to="/footballerAdd">Add Footballer</Link></li>
      </ul>
    </nav>
  );
};

export default Navbar;
