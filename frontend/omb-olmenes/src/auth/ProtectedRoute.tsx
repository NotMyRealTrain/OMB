import { Navigate } from "react-router-dom";
import { getUser, hasRole, type Role } from "./auth";

type Props = {
  children: React.ReactNode;
  requireRole?: Role;
};

export default function ProtectedRoute({ children, requireRole }: Props) {
  const user = getUser();

  // if the user is not logged in, redirect to login page
  if (!user) return <Navigate to="/login" replace />;

  // if user does not have the required role, redirect to no access page
  if (requireRole && !hasRole(user, requireRole)) {
    return <Navigate to="/no-access" replace />;
  }

  return <>{children}</>;
}
