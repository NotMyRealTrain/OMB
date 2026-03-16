import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import HousePage from "./pages/HousePage";
import KitchenPage from "./pages/KitchenPage";
import AdminPage from "./pages/AdminPage";
import CareResidentPage from "./pages/CareResidentPage";
import NoAccessPage from "./pages/NoAccessPage";
import ProtectedRoute from "./auth/ProtectedRoute";
import { getUser } from "./auth/auth";

function HomeRedirect() {
  const user = getUser();

  if (!user) 
    return <Navigate to="/login" replace />;
  if (user.roles.includes("ADMIN")) 
    return <Navigate to="/admin" replace />;
  if (user.roles.includes("CARE_SPECIALIST")) 
    return <Navigate to="/care" replace />;
  if (user.roles.includes("KITCHEN")) 
    return <Navigate to="/kitchen" replace />;
  if (user.roles.includes("HOUSE")) 
    return <Navigate to="/house" replace />;

  return <Navigate to="/no-access" replace />;
}

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomeRedirect />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/no-access" element={<NoAccessPage />} />

        <Route
          path="/house"
          element={
            <ProtectedRoute requireRole="HOUSE">
              <HousePage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/kitchen"
          element={
            <ProtectedRoute requireRole="KITCHEN">
              <KitchenPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/admin"
          element={
            <ProtectedRoute requireRole="ADMIN">
              <AdminPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/care"
          element={
            <ProtectedRoute requireRole="CARE_SPECIALIST">
              <CareResidentPage />
            </ProtectedRoute>
          }
        />

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}