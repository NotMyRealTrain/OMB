import "./ProfilePicture.css";
import avatar from "../assets/avatar.jpg";

function ProfilePicture() {
  return (
    <button className="profile-picture">
      <img src={avatar} alt="Profile picture" />
    </button>
  );
}

export default ProfilePicture;