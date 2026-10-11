import { useState } from "react";
import { useNavigate } from "react-router";
import "./Settings.css";
import { useDispatch } from "react-redux";
import { removeCookie } from "../../../services/cookieService";

export default function Settings({ user, onLogout }) {
    const navigate = useNavigate();

    const [passwords, setPasswords] = useState({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
    });

    const [showPasswords, setShowPasswords] = useState(false);
    const [message, setMessage] = useState("");
    const [error, setError] = useState("");
    const [isLoggingOut, setIsLoggingOut] = useState(false);

    const dispatch = useDispatch();

    const handlePasswordChange = (e) => {
        const { name, value } = e.target;

        setPasswords((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleChangePassword = async (e) => {
        e.preventDefault();

        setMessage("");
        setError("");

        if (passwords.newPassword.length < 8) {
            setError("Пароль повинен містити щонайменше 8 символів.");
            return;
        }

        if (passwords.newPassword !== passwords.confirmPassword) {
            setError("Нові паролі не збігаються.");
            return;
        }

        try {
            // TODO: Виклик API для зміни пароля.
            // Після успішної відповіді:
            // setMessage("Пароль успішно змінено.");
            // setPasswords({
            //   currentPassword: "",
            //   newPassword: "",
            //   confirmPassword: "",
            // });

            setMessage("Форма перевірена. Підключіть API зміни пароля.");
        } catch {
            setError("Не вдалося змінити пароль.");
        }
    };

    const handleLogout = async () => {
        setIsLoggingOut(true);
        setError("");

        try {
            // onLogout має завершувати сесію через API (за потреби),
            // очищати токени й оновлювати стан авторизації.
            removeCookie("ujt");
            dispatch({type: "LOGOUT"});
            navigate("/login", { replace: true });
        } catch {
            setError("Не вдалося вийти з акаунта.");
        } finally {
            setIsLoggingOut(false);
        }
    };

    return (
        <div className="settings-page">
            <div className="settings-heading">
                <h2>Налаштування</h2>
                <p>Керуйте безпекою та параметрами свого акаунта</p>
            </div>

            {error && (
                <div className="settings-alert error" role="alert">
                    {error}
                </div>
            )}

            {message && (
                <div className="settings-alert success" role="status">
                    {message}
                </div>
            )}

            {/* Account */}
            <section className="settings-card">
                <div className="settings-card-header">
                    <div className="settings-card-icon">👤</div>

                    <div>
                        <h3>Обліковий запис</h3>
                        <p>Основна інформація про ваш акаунт</p>
                    </div>
                </div>

                <div className="settings-account-row">
                    <div>
                        <span>Ім'я користувача</span>
                        <strong>{user?.userName || "Не вказано"}</strong>
                    </div>
                </div>

                <div className="settings-account-row">
                    <div>
                        <span>Електронна пошта</span>
                        <strong>{user?.email || "Не вказано"}</strong>
                    </div>

                    {user && (
                        <span
                            className={`settings-status ${
                                user.emailConfirmed
                                    ? "confirmed"
                                    : "unconfirmed"
                            }`}
                        >
                            {user.emailConfirmed
                                ? "✓ Підтверджено"
                                : "Не підтверджено"}
                        </span>
                    )}
                </div>
            </section>

            {/* Change password */}
            <section className="settings-card">
                <div className="settings-card-header">
                    <div className="settings-card-icon">🔒</div>

                    <div>
                        <h3>Зміна пароля</h3>
                        <p>Регулярно оновлюйте пароль для захисту акаунта</p>
                    </div>
                </div>

                <form
                    className="settings-password-form"
                    onSubmit={handleChangePassword}
                >
                    <div className="settings-field">
                        <label htmlFor="currentPassword">Поточний пароль</label>

                        <input
                            id="currentPassword"
                            name="currentPassword"
                            type={showPasswords ? "text" : "password"}
                            placeholder="Введіть поточний пароль"
                            autoComplete="current-password"
                            value={passwords.currentPassword}
                            onChange={handlePasswordChange}
                            required
                        />
                    </div>

                    <div className="settings-field">
                        <label htmlFor="newPassword">Новий пароль</label>

                        <input
                            id="newPassword"
                            name="newPassword"
                            type={showPasswords ? "text" : "password"}
                            placeholder="Введіть новий пароль"
                            autoComplete="new-password"
                            value={passwords.newPassword}
                            onChange={handlePasswordChange}
                            minLength={8}
                            required
                        />
                    </div>

                    <div className="settings-field">
                        <label htmlFor="confirmPassword">
                            Підтвердження нового пароля
                        </label>

                        <input
                            id="confirmPassword"
                            name="confirmPassword"
                            type={showPasswords ? "text" : "password"}
                            placeholder="Повторіть новий пароль"
                            autoComplete="new-password"
                            value={passwords.confirmPassword}
                            onChange={handlePasswordChange}
                            required
                        />
                    </div>

                    <label className="settings-checkbox">
                        <input
                            type="checkbox"
                            checked={showPasswords}
                            onChange={(e) => setShowPasswords(e.target.checked)}
                        />
                        Показати паролі
                    </label>

                    <div className="settings-form-actions">
                        <button type="submit" className="settings-primary-btn">
                            Змінити пароль
                        </button>
                    </div>
                </form>
            </section>

            {/* Logout */}
            <section className="settings-card settings-danger-card">
                <div className="settings-card-header">
                    <div className="settings-card-icon danger">🚪</div>

                    <div>
                        <h3>Завершення сеансу</h3>
                        <p>
                            Вийдіть зі свого облікового запису на цьому пристрої
                        </p>
                    </div>
                </div>

                <div className="settings-logout-row">
                    <div>
                        <strong>Вийти з акаунта</strong>
                        <p>
                            Для повторного входу потрібно буде авторизуватися.
                        </p>
                    </div>

                    <button
                        type="button"
                        className="settings-logout-btn"
                        onClick={handleLogout}
                        disabled={isLoggingOut}
                    >
                        {isLoggingOut ? "Вихід..." : "Вийти"}
                    </button>
                </div>
            </section>
        </div>
    );
}