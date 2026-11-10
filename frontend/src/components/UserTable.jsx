function initials(user) {
  return `${user.name?.[0] ?? ""}${user.surname?.[0] ?? ""}`.toUpperCase();
}

function formatDate(value) {
  if (!value) return "—";
  return new Date(value).toLocaleString("ru-RU", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
}

export default function UserTable({ users, onEdit, onDelete }) {
  return (
    <div className="table-wrapper">
      <table className="user-table">
        <thead>
          <tr>
            <th>Логин</th>
            <th>Имя</th>
            <th>Фамилия</th>
            <th>Email</th>
            <th>Дата посещения</th>
            <th className="actions-col">
              <span className="visually-hidden">Действия</span>
            </th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id} onDoubleClick={() => onEdit(user)}>
              <td data-label="Логин">
                <div className="user-cell">
                  <span className="avatar" aria-hidden="true">
                    {initials(user)}
                  </span>
                  <span className="login">{user.login}</span>
                </div>
              </td>
              <td data-label="Имя">{user.name}</td>
              <td data-label="Фамилия">{user.surname}</td>
              <td data-label="Email" className="muted">{user.email}</td>
              <td data-label="Дата посещения" className="muted">{formatDate(user.lastVisited)}</td>
              <td className="actions">
                <button
                  type="button"
                  id={`editBtn${user.id}`}
                  className="btn btn-ghost btn-edit"
                  onClick={() => onEdit(user)}
                >
                  Изменить
                </button>
                <button
                  type="button"
                  className="btn btn-ghost btn-delete"
                  onClick={() => onDelete(user)}
                >
                  Удалить
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
