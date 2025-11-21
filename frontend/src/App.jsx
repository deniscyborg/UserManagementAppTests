import React, { useState, useEffect } from "react";

function formatDate(date) {
  if (!date) return "";
  const d = new Date(date);
  return `${String(d.getDate()).padStart(2, '0')}.${String(d.getMonth()+1).padStart(2, '0')}.${d.getFullYear()} ${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2,'0')}`;
}

function UserForm({ onSubmit, initial, onCancel }) {
  const [name, setName] = useState(initial?.name || "");
  const [surname, setSurname] = useState(initial?.surname || "");
  const [email, setEmail] = useState(initial?.email || "");
  const [error, setError] = useState({});
  function validate() {
    return {
      name: name.length >= 3 ? null : "Минимум 3 символа",
      surname: surname.length >= 3 ? null : "Минимум 3 символа",
      email: /^[^@\s]+@[^@\s]+\.(ru)$/i.test(email)
        ? null
        : "Формат email должен быть @... и заканчиваться на .ru"
    }
  }
  function handleSubmit(e) {
    e.preventDefault();
    const eobj = validate();
    setError(eobj);
    if (Object.values(eobj).some(Boolean)) return;
    onSubmit({ name, surname, email });
  }
  return (
    <form onSubmit={handleSubmit}>
      <div>
        Имя: <input value={name} onChange={e => setName(e.target.value)} />
        {error.name && <div className="error-message">{error.name}</div>}
      </div>
      <div>
        Фамилия: <input value={surname} onChange={e => setSurname(e.target.value)} />
        {error.surname && <div className="error-message">{error.surname}</div>}
      </div>
      <div>
        Email: <input value={email} onChange={e => setEmail(e.target.value)} />
        {error.email && <div className="error-message">{error.email}</div>}
      </div>
      <button type="submit" disabled={Object.values(validate()).some(Boolean)}>
        {initial ? "Сохранить" : "Добавить"}
      </button>
      {onCancel && <button type="button" onClick={onCancel}>Отмена</button>}
    </form>
  );
}

function App() {
  const [users, setUsers] = useState([]);
  const [editing, setEditing] = useState(null);
  const [modal, setModal] = useState(null);
  const [filter, setFilter] = useState("");

  useEffect(() => { 
    fetch("/api/users")
      .then(res => res.json())
      .then(setUsers);
   }, []);
  function handleAdd(user) {
    fetch("/api/users", {
      method: "POST", body: JSON.stringify(user),
      headers: { "Content-Type": "application/json" }
    }).then(res => res.ok
      ? res.json().then(u => setUsers(prev => [...prev, u]))
      : res.text().then(alert));
    setModal(null);
  }
  function handleEdit(user) {
  fetch("/api/users/" + editing.login, {
    method: "PUT", 
    body: JSON.stringify({ ...user, login: editing.login }), // <-- ЭТА СТРОКА ВАЖНА
    headers: { "Content-Type": "application/json" }
  })
  .then(res => res.ok
    ? res.json().then(u => setUsers(prev => prev.map(x => x.login === u.login ? u : x)))
    : res.text().then(alert));
  setEditing(null);
}
  function handleDelete(login) {
    fetch("/api/users/" + login, { method: "DELETE" })
      .then(() => setUsers(users.filter(u => u.login !== login)));
  }
  const filtered = users.filter(u => u.login?.includes(filter));
  return (
    <div className="main-container center">
      <button onClick={() => setModal("add")}>Добавить пользователя</button>
      <input placeholder="Фильтр по логину" value={filter} onChange={e => setFilter(e.target.value)} />
      <table className="user-table">
        <thead>
          <tr>
            <th>Логин</th><th>Имя</th><th>Фамилия</th>
            <th>Email</th><th>Дата посещения</th><th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {filtered.map(u =>
            <tr key={u.login}
                style={editing && editing.login === u.login ? { background: "#cce" } : {}}
                onDoubleClick={() => setEditing(u)}
            >
              <td>{u.login}</td>
              <td>{u.name}</td>
              <td>{u.surname}</td>
              <td>{u.email}</td>
              <td>{formatDate(u.lastVisited)}</td>
              <td>
                <button onClick={() => setEditing(u)}>Редактировать</button>
                <button onClick={() => handleDelete(u.login)}>Удалить</button>
              </td>
            </tr>
          )}
        </tbody>
      </table>

{/* Модальные окна */}
  {editing && (
    <>
      <div className="modal-overlay"></div>
      <div className="modal-content">
        <UserForm initial={editing} onSubmit={handleEdit} onCancel={() => setEditing(null)} />
      </div>
    </>
  )}
  {modal === "add" && (
    <>
      <div className="modal-overlay"></div>
      <div className="modal-content">
        <UserForm onSubmit={handleAdd} onCancel={() => setModal(null)} />
      </div>
    </>
  )}
</div>
);
}

export default App;