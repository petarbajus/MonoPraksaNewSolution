import React from 'react';
import { Routes, Route } from 'react-router-dom'; // No need to import BrowserRouter here
import Home from './Home';
import ClubAdd from './ClubAdd';
import ClubList from './ClubList';
import ClubUpdate from './ClubUpdate';
import FootballerAdd from './FootballerAdd';
import FootballerList from './FootballerList';
import FootballerUpdate from './FootballerUpdate';

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/clubList" element={<ClubList />} />
        <Route path="/clubAdd" element={<ClubAdd />} />
        <Route path="/clubUpdate/:id" element={<ClubUpdate />} />
        <Route path="/footballerList" element={<FootballerList />} />
        <Route path="/footballerAdd" element={<FootballerAdd />} />
        <Route path="/footballerUpdate/:id" element={<FootballerUpdate />} />
      </Routes>
    </div>
  );
}

export default App;
