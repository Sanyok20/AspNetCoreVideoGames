import { useState } from "react";
import { Link, useNavigate } from "react-router";
import "./Login.css";
import axios from "axios";
import { setCookie } from "../../../services/cookieService";
import { jwtDecode } from "jwt-decode";
import { useDispatch } from "react-redux";

export default function Login() {
    const [form, setForm] = useState({
        login: "",
        password: "",
        rememberMe: false,
    });

    const [showPassword, setShowPassword] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;

        setForm((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        if (!form.login.trim() || !form.password) {
            setError("Будь ласка, заповніть усі поля");
            return;
        }

        setLoading(true);

        try {
            // Тут буде запит до API авторизації
            const loginData = {
                login: form.login,
                password: form.password,
                rememberMe: form.rememberMe,
            };
            const response = await axios.post(
                "https://localhost:5000/api/auth/login",
                loginData,
            );

            const { data } = response;
            const token = data.payload;
            const decoded = jwtDecode(token);
            setCookie("ujt", token, loginData.rememberMe ? decoded.exp : null);
            delete decoded.exp;
            delete decoded.iss;
            delete decoded.aud;
            dispatch({ type: "LOGIN", payload: decoded });
            navigate("/");
        } catch {
            setError("Не вдалося виконати вхід. Спробуйте ще раз.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <main className="login-page">
            <div className="login-card">
                <div className="login-header">
                    <div className="login-icon">🎮</div>

                    <h1>З поверненням!</h1>
                    <p>Увійдіть у свій акаунт GameStore</p>
                </div>

                {error && (
                    <div className="login-error" role="alert">
                        {error}
                    </div>
                )}

                <form className="login-form" onSubmit={handleSubmit}>
                    <div className="login-field">
                        <label htmlFor="login">Логін</label>

                        <input
                            id="login"
                            type="text"
                            name="login"
                            placeholder="example"
                            autoComplete="login"
                            value={form.login}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className="login-field">
                        <div className="login-label-row">
                            <label htmlFor="password">Пароль</label>

                            <Link to="/forgot-password">Забули пароль?</Link>
                        </div>

                        <div className="login-password-wrapper">
                            <input
                                id="password"
                                type={showPassword ? "text" : "password"}
                                name="password"
                                placeholder="Введіть пароль"
                                autoComplete="current-password"
                                value={form.password}
                                onChange={handleChange}
                                required
                            />

                            <button
                                type="button"
                                className="login-show-password"
                                onClick={() => setShowPassword((prev) => !prev)}
                                aria-label={
                                    showPassword
                                        ? "Приховати пароль"
                                        : "Показати пароль"
                                }
                            >
                                {showPassword ? "Приховати" : "Показати"}
                            </button>
                        </div>
                    </div>

                    <label className="login-remember">
                        <input
                            type="checkbox"
                            name="rememberMe"
                            checked={form.rememberMe}
                            onChange={handleChange}
                        />
                        <span>Запам'ятати мене</span>
                    </label>

                    <button
                        className="login-submit"
                        type="submit"
                        disabled={loading}
                    >
                        {loading ? "Вхід..." : "Увійти"}
                    </button>
                </form>

                <div className="login-divider">
                    <span>Новий користувач?</span>
                </div>

                <Link to="/register" className="login-register">
                    Створити акаунт
                </Link>

                <p className="login-footer">
                    Продовжуючи, ви погоджуєтесь з нашими{" "}
                    <Link to="/terms">Умовами використання</Link> та{" "}
                    <Link to="/privacy">Політикою конфіденційності</Link>.
                </p>
            </div>
        </main>
    );
}