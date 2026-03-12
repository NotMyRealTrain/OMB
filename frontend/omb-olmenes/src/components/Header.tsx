import "./Header.css";
import ProfilePicture from "./ProfilePicture";

function Header() {
  return (
    <header className="header">
      <div className="header-left">
        <p>Welkom</p>
      </div>

      <div className="header-inner">

      </div>
      <div className="nav-top-right">
          <ProfilePicture />
      </div>
    </header>
  );
}

export default Header;