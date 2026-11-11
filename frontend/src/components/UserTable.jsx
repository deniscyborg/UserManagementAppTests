function initials(user) {
  return `${user.name?.[0] ?? ""}${user.surname?.[0] ?? ""}`.toUpperCase();
}

function formatDate(value) {
  if (!value) return "—";
  return new Date(value).toLocaleString("en-GB", {
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
            <th>Login</th>
            <th>Name</th>
            <th>Surname</th>
            <th>Email</th>
            <th>Last visit</th>
            <th className="actions-col">
              <span className="visually-hidden">Actions</span>
            </th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id} onDoubleClick={() => onEdit(user)}>
              <td data-label="Login">
                <div className="user-cell">
                  <span className="avatar" aria-hidden="true">
                    {initials(user)}
                  </span>
                  <span className="login">{user.login}</span>
                </div>
              </td>
              <td data-label="Name">{user.name}</td>
              <td data-label="Surname">{user.surname}</td>
              <td data-label="Email" className="muted">{user.email}</td>
              <td data-label="Last visit" className="muted">{formatDate(user.lastVisited)}</td>
              <td className="actions">
                <button
                  type="button"
                  id={`editBtn${user.id}`}
                  className="btn btn-ghost btn-edit"
                  onClick={() => onEdit(user)}
                >
                  Edit
                </button>
                <button
                  type="button"
                  className="btn btn-ghost btn-delete"
                  onClick={() => onDelete(user)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
