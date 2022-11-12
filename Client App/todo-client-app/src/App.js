import React from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import { TodoPage } from './Pages/ToDo/todo-view';

function App() {
  return (
    <div>
      <Router>
        <Routes>
          <Route exact path='/' element={<TodoPage/>}/>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
