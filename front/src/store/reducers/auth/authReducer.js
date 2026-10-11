const initState = {
    isAuthenticated: false,
    user: null,
};

export const authReducer = (state = initState, action) => {
    switch (action.type) {
        case "LOGIN":
            return { ...state, isAuthenticated: true, user: action.payload };
        case "LOGOUT":
            return { ...state, isAuthenticated: false, user: null };
        default:
            return state;
    }
};