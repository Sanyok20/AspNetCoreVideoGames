export function setCookie(name, value, expires = null) {
    let cookie = `${encodeURIComponent(name)}=${encodeURIComponent(value)}; path=/; SameSite=Lax`;

    if (expires !== null) {
        const date = new Date(expires * 1000);
        cookie += `; expires=${date.toUTCString()}`;
    }

    if (window.location.protocol === "https:") {
        cookie += "; Secure";
    }

    document.cookie = cookie;
}

export function getCookie(name) {
    const encodedName = `${encodeURIComponent(name)}=`;

    const cookie = document.cookie
        .split("; ")
        .find((row) => row.startsWith(encodedName));

    if (!cookie) return null;

    return decodeURIComponent(cookie.substring(encodedName.length));
}

export function removeCookie(name) {
    document.cookie = `${encodeURIComponent(name)}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Lax`;
}