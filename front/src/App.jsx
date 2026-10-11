import { Route, Routes } from "react-router";
import "./App.css";
import Navbar from "./components/navbar/Navbar";
import Login from "./pages/auth/login/Login";
import Profile from "./pages/auth/profile/Profile";
import { useEffect } from "react";
import { getCookie } from "./services/cookieService";
import { useDispatch } from "react-redux";
import { jwtDecode } from "jwt-decode";
import Settings from "./pages/auth/profile/Settings";

function App() {
  const dispatch = useDispatch();

  useEffect(() => {
    const token = getCookie("ujt");
    if (token) {
      const decoded = jwtDecode(token);
      delete decoded.exp;
      delete decoded.iss;
      delete decoded.aud;
      dispatch({ type: "LOGIN", payload: decoded });
    }
  }, []);

  return (
    <>
      <Navbar />
      <Routes>
        <Route path="login" element={<Login />} />
        <Route path="profile">
          <Route index element={<Profile />} />
          <Route path="settings" element={<Settings />} />
        </Route>
      </Routes>
    </>
  );
}

export default App;