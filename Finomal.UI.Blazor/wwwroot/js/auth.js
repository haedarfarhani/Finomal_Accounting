// JS Helper برای localStorage (در فایل wwwroot/js/auth.js یا مستقیم در Razor)
window.getLocalStorageItem = (key) => {
    return localStorage.getItem(key);
};

window.setLocalStorageItem = (key, value) => {
    localStorage.setItem(key, value);
};

window.removeLocalStorageItem = (key) => {
    localStorage.removeItem(key);
};

function logToken(key) {
    const token = localStorage.getItem(key);
    console.log(`Token for ${key}:`, token ? `Length ${token.length}` : 'Not found');
}