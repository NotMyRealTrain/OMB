import {useState} from "react";
import Header from "../components/Header";
import "./AdminPage.css";
import olmenesLogo from "../assets/olmenesLogo.svg";

export default function AdminPage() {
  const [activeView, setActiveView] = useState("users");

  return (
    <div className="admin-page">
      <Header></Header>
      <aside className="sidebar">
        <div className="brand">
          <img className="sidebar-logo" src={olmenesLogo} alt="Omb Olmenes" />
        </div>
        <nav>
          <button 
            className={`nav-item ${activeView === "users" ? "active" : ""}`}
            onClick={() => setActiveView("users")}
          >
            Gebruikers
          </button>
          <button 
            className={`nav-item ${activeView === "houses" ? "active" : ""}`}
            onClick={() => setActiveView("houses")}
          >
            Woonhuizen
          </button>
        </nav>
      </aside>
      <div className="admin-content">
        {activeView === "users" && <h1>Users Table Placeholder</h1>}
        {activeView === "houses" && <h1>Houses Table Placeholder</h1>}
      </div>
    </div>
  );
}
