import { useNavigate } from "react-router-dom";
import { setUser, type Role } from "../auth/auth";
import "./LoginPage.css";
import olmenesLogo from "../assets/olmenesLogo.svg";
import background from "../assets/background.jpg";

function loginAs(role: Role) {
  setUser({ displayName: "Test User", roles: [role] });
}

export default function LoginPage() {
  const navigate = useNavigate();

  const handleLogin = (role: Role) => {
    loginAs(role);

    // redirect based on role
    if (role === "ADMIN") navigate("/admin");
    else if (role === "KITCHEN") navigate("/kitchen");
    else navigate("/house");
  };

  return (
    <div className="login-page">
      <img className="login-background" src={background} alt="food-background" />
      <div className="login-card">
        <img className="login-logo" src={olmenesLogo} alt="Omb Olmenes Logo" />
        <h1>Login</h1>
        <p>This needs to be replaced with Microsoft Entra.</p>

        <button className="login-button" onClick={() => handleLogin("HOUSE")}>Login as HOUSE</button>{" "}
        <button className="login-button" onClick={() => handleLogin("KITCHEN")}>Login as KITCHEN</button>{" "}
        <button className="login-button" onClick={() => handleLogin("ADMIN")}>Login as ADMIN</button> {" "}
      </div>
    </div>
  );


}
