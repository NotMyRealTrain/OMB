import { Link } from "react-router-dom";

export default function NoAccessPage() {
  return (
    <div style={{ padding: 16 }}>
      <h1>No access</h1>
      <p>You don’t have permission for this page.</p>
      <Link to="/login">Go to login</Link>
    </div>
  );
}
