import { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router";
import "./Profile.css";
import { getCookie } from "../../../services/cookieService";
import axios from "axios";

export default function Profile() {
    const [isEditing, setIsEditing] = useState(false);
    const [user, setUser] = useState(null);
    const [form, setForm] = useState({
        userName: "",
        firstName: "",
        lastName: "",
        phone: "",
    });

    const navigate = useNavigate();

    useEffect(() => {
        const fetchProfile = async () => {
            const url = `https://localhost:5000/api/auth/profile`;
            const token = getCookie("ujt");
            try {
                const response = await axios.get(url, {
                    headers: {
                        Authorization: token,
                    },
                });
                const { data } = response;
                setUser(data.payload);
            } catch (error) {
                navigate("/login");
            }
        };

        fetchProfile();
    }, []);

    if (!user) {
        return <h1 style={{ textAlign: "center" }}>Завантаження</h1>;
    }

    const handleChange = (e) => {
        const { name, value } = e.target;

        setForm((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleCancel = () => {
        setForm({
            userName: user.userName ?? "",
            firstName: user.firstName ?? "",
            lastName: user.lastName ?? "",
            phone: user.phone ?? "",
        });

        setIsEditing(false);
    };

    const handleSave = (e) => {
        e.preventDefault();

        // Тут буде PUT/PATCH запит до API
        console.log("Updated profile:", form);

        // Після успішного запиту онови user у своєму state/context
        setIsEditing(false);
    };

    const initials =
        `${user.firstName?.[0] ?? ""}${user.lastName?.[0] ?? ""}`.toUpperCase() ||
        user.userName?.[0]?.toUpperCase() ||
        "U";

    const fullName =
        [user.firstName, user.lastName].filter(Boolean).join(" ") ||
        user.userName;

    return (
        <main className="profile-page">
            <div className="profile-container">
                <div className="profile-heading">
                    <h1>Мій профіль</h1>
                    <p>Керуйте своїм акаунтом та особистими даними</p>
                </div>

                <div className="profile-layout">
                    {/* Sidebar */}
                    <aside className="profile-sidebar">
                        <div className="profile-sidebar-user">
                            <div className="profile-sidebar-avatar">
                                {user.image ? (
                                    <img src={user.image} alt="Аватар" />
                                ) : (
                                    initials
                                )}
                            </div>

                            <div>
                                <strong>{fullName}</strong>
                                <span>@{user.userName}</span>
                            </div>
                        </div>

                        <nav className="profile-sidebar-nav">
                            <NavLink to="/profile" end>
                                <span>👤</span> Мій профіль
                            </NavLink>

                            <NavLink to="/profile/orders">
                                <span>📦</span> Мої покупки
                            </NavLink>

                            <NavLink to="/profile/favorites">
                                <span>❤️</span> Обране
                            </NavLink>

                            <NavLink to="/profile/settings">
                                <span>⚙️</span> Налаштування
                            </NavLink>
                        </nav>
                    </aside>

                    {/* Content */}
                    <div className="profile-content">
                        <div className="profile-card">
                            <div className="profile-cover" />

                            <div className="profile-user-header">
                                <div className="profile-avatar">
                                    {user.image ? (
                                        <img
                                            src={user.image}
                                            alt="Фото профілю"
                                        />
                                    ) : (
                                        initials
                                    )}
                                </div>

                                <div className="profile-user-info">
                                    <h2>{fullName}</h2>
                                    <p>@{user.userName}</p>
                                </div>

                                {!isEditing && (
                                    <button
                                        type="button"
                                        className="profile-edit-btn"
                                        onClick={() => setIsEditing(true)}
                                    >
                                        ✎ Редагувати
                                    </button>
                                )}
                            </div>

                            <div className="profile-section">
                                <div className="profile-section-header">
                                    <div>
                                        <h3>Особиста інформація</h3>
                                        <p>
                                            Основні дані вашого облікового
                                            запису
                                        </p>
                                    </div>
                                </div>

                                <form onSubmit={handleSave}>
                                    <div className="profile-fields">
                                        <div className="profile-field">
                                            <label>Ім'я</label>
                                            <input
                                                name="firstName"
                                                value={form.firstName}
                                                onChange={handleChange}
                                                disabled={!isEditing}
                                                placeholder="Не вказано"
                                            />
                                        </div>

                                        <div className="profile-field">
                                            <label>Прізвище</label>
                                            <input
                                                name="lastName"
                                                value={form.lastName}
                                                onChange={handleChange}
                                                disabled={!isEditing}
                                                placeholder="Не вказано"
                                            />
                                        </div>

                                        <div className="profile-field">
                                            <label>Ім'я користувача</label>
                                            <input
                                                name="userName"
                                                value={form.userName}
                                                onChange={handleChange}
                                                disabled={!isEditing}
                                            />
                                        </div>

                                        <div className="profile-field">
                                            <label>Номер телефону</label>
                                            <input
                                                name="phone"
                                                type="tel"
                                                value={form.phone}
                                                onChange={handleChange}
                                                disabled={!isEditing}
                                                placeholder="Не вказано"
                                            />
                                        </div>
                                    </div>

                                    {isEditing && (
                                        <div className="profile-form-actions">
                                            <button
                                                type="button"
                                                className="profile-cancel-btn"
                                                onClick={handleCancel}
                                            >
                                                Скасувати
                                            </button>

                                            <button
                                                type="submit"
                                                className="profile-save-btn"
                                            >
                                                Зберегти зміни
                                            </button>
                                        </div>
                                    )}
                                </form>
                            </div>
                        </div>

                        {/* Account details */}
                        <div className="profile-card profile-account-card">
                            <div className="profile-section-header">
                                <div>
                                    <h3>Інформація про акаунт</h3>
                                    <p>Електронна пошта та безпека</p>
                                </div>
                            </div>

                            <div className="profile-account-row">
                                <div>
                                    <span className="profile-account-label">
                                        Електронна пошта
                                    </span>

                                    <strong>{user.email}</strong>
                                </div>

                                <span
                                    className={`profile-status ${
                                        user.emailConfirmed
                                            ? "confirmed"
                                            : "unconfirmed"
                                    }`}
                                >
                                    {user.emailConfirmed
                                        ? "✓ Підтверджено"
                                        : "Не підтверджено"}
                                </span>
                            </div>

                            <div className="profile-account-row">
                                <div>
                                    <span className="profile-account-label">
                                        Ідентифікатор користувача
                                    </span>

                                    <strong>#{user.id}</strong>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    );
}