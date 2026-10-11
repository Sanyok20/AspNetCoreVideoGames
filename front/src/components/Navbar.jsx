import { useState } from "react";
import { NavLink, Link } from "react-router";
import "./Navbar.css";
import { useSelector } from "react-redux";

export default function Navbar() {
    const [menuOpen, setMenuOpen] = useState(false);
    const { isAuthenticated, user } = useSelector((state) => state.auth);

    const closeMenu = () => setMenuOpen(false);

    return (
        <header className="navbar">
            <div className="navbar-container">
                {/* Logo */}
                <Link to="/" className="navbar-logo" onClick={closeMenu}>
                    <span className="navbar-logo-icon">🎮</span>
                    <span>
                        Game<span>Store</span>
                    </span>
                </Link>

                {/* Navigation */}
                <nav className={`navbar-nav ${menuOpen ? "open" : ""}`}>
                    <NavLink to="/" end onClick={closeMenu}>
                        Головна
                    </NavLink>

                    <NavLink to="/games" onClick={closeMenu}>
                        Каталог
                    </NavLink>

                    <NavLink to="/discounts" onClick={closeMenu}>
                        Знижки
                    </NavLink>

                    <NavLink to="/about" onClick={closeMenu}>
                        Про нас
                    </NavLink>
                </nav>

                {/* Authentication */}
                <div className="navbar-auth">
                    {isAuthenticated ? (
                        <Link to="/profile" className="navbar-profile">
                            <span className="navbar-avatar">
                                {user?.userName?.charAt(0).toUpperCase() || "U"}
                            </span>

                            <span className="navbar-username">
                                {user?.userName || "Профіль"}
                            </span>
                        </Link>
                    ) : (
                        <>
                            <Link to="/login" className="navbar-login">
                                Увійти
                            </Link>

                            <Link to="/register" className="navbar-register">
                                Зареєструватися
                            </Link>
                        </>
                    )}
                </div>

                {/* Mobile toggle */}
                <button
                    type="button"
                    className={`navbar-toggle ${menuOpen ? "open" : ""}`}
                    onClick={() => setMenuOpen(!menuOpen)}
                    aria-label="Відкрити меню"
                    aria-expanded={menuOpen}
                >
                    <span></span>
                    <span></span>
                    <span></span>
                </button>
            </div>
        </header>
    );
}