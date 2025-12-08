import './App.css';
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
      email: /^[^\@\s]+@[^\@\s]+\.(ru)$/i.test(email)
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
    <form onSubmit={handleSubmit} className="user-form">
      <div className="form-group">
        <label>Имя:</label>
        <input 
          type="text"
          name="name"
          value={name} 
          onChange={e => setName(e.target.value)} 
          placeholder="Введите имя"
        />
        {error.name && <div className="error-message">{error.name}</div>}
      </div>
      
      <div className="form-group">
        <label>Фамилия:</label>
        <input 
          type="text"
          name="surname"
          value={surname} 
          onChange={e => setSurname(e.target.value)} 
          placeholder="Введите фамилию"
        />
        {error.surname && <div className="error-message">{error.surname}</div>}
      </div>
      
      <div className="form-group">
        <label>Email:</label>
        <input 
          type="email"
          name="email"
          value={email} 
          onChange={e => setEmail(e.target.value)} 
          placeholder="example@mail.ru"
        />
        {error.email && <div className="error-message">{error.email}</div>}
      </div>
      
      <div className="form-actions">
        <button 
          type="submit" 
          disabled={Object.values(validate()).some(Boolean)}
          className="btn-primary"
        >
          {initial ? "Сохранить" : "Добавить"}
        </button>
        {onCancel && (
          <button 
            type="button" 
            onClick={onCancel}
            className="btn-secondary"
          >
            Отмена
          </button>
        )}
      </div>
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
      method: "POST", 
      body: JSON.stringify(user),
      headers: { "Content-Type": "application/json" }
    })
    .then(res => res.ok
      ? res.json().then(u => setUsers(prev => [...prev, u]))
      : res.text().then(alert));
    setModal(null);
  }
  
  function handleEdit(user) {
    fetch("/api/users/" + editing.login, {
      method: "PUT", 
      body: JSON.stringify({ ...user, login: editing.login }),
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
    <div className="main-container">
      <div className="controls">
        <button 
          onClick={() => setModal("add")}
          className="btn-primary"
        >
          Добавить пользователя
        </button>
        <input 
          placeholder="Фильтр по логину" 
          value={filter} 
          onChange={e => setFilter(e.target.value)}
          className="filter-input"
        />
      </div>
      
      <table className="user-table">
        <thead>
          <tr>
            <th>Логин</th>
            <th>Имя</th>
            <th>Фамилия</th>
            <th>Email</th>
            <th>Дата посещения</th>
            <th>Действия</th>
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
                <button 
                  onClick={() => setEditing(u)}
                  className="btn-edit"
                >
                  Редактировать
                </button>
                <button 
                  onClick={() => handleDelete(u.login)}
                  className="btn-delete"
                >
                  Удалить
                </button>
              </td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Модальное окно редактирования */}
      {editing && (
        <>
          <div className="modal-overlay" onClick={() => setEditing(null)}></div>
          <div className="modal-content">
            <h2>Редактирование пользователя</h2>
            <UserForm 
              initial={editing} 
              onSubmit={handleEdit} 
              onCancel={() => setEditing(null)} 
            />
          </div>
        </>
      )}
      
      {/* Модальное окно добавления */}
      {modal === "add" && (
        <>
          <div className="modal-overlay" onClick={() => setModal(null)}></div>
          <div className="modal-content">
            <h2>Добавить нового пользователя</h2>
            <UserForm 
              onSubmit={handleAdd} 
              onCancel={() => setModal(null)} 
            />
          </div>
        </>
      )}
    </div>
  );
}

export default App;