import { useCallback, useEffect, useMemo, useState } from "react";
import { usersApi } from "./api";
import Modal from "./components/Modal";
import Toast from "./components/Toast";
import UserForm from "./components/UserForm";
import UserTable from "./components/UserTable";

export default function App() {
  const [users, setUsers] = useState([]);
  const [status, setStatus] = useState("loading"); // loading | ready | error
  const [filter, setFilter] = useState("");
  const [modal, setModal] = useState(null); // { mode: "add" } | { mode: "edit", user }
  const [toast, setToast] = useState(null);

  const notify = useCallback((type, message) => setToast({ type, message }), []);
  const closeToast = useCallback(() => setToast(null), []);
  const closeModal = useCallback(() => setModal(null), []);

  const fetchUsers = useCallback(
    () =>
      usersApi
        .getAll()
        .then((data) => {
          setUsers(data);
          setStatus("ready");
        })
        .catch(() => setStatus("error")),
    []
  );

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  function retryLoad() {
    setStatus("loading");
    fetchUsers();
  }

  const filtered = useMemo(() => {
    const query = filter.trim().toLowerCase();
    return query ? users.filter((u) => u.login.toLowerCase().includes(query)) : users;
  }, [users, filter]);

  async function handleAdd(data) {
    try {
      const created = await usersApi.create(data);
      setUsers((prev) => [...prev, created]);
      setModal(null);
      notify("success", "User added");
    } catch (e) {
      notify("error", e.message);
    }
  }

  async function handleEdit(data) {
    try {
      const updated = await usersApi.update(modal.user.id, data);
      setUsers((prev) => prev.map((u) => (u.id === updated.id ? updated : u)));
      setModal(null);
      notify("success", "Changes saved");
    } catch (e) {
      notify("error", e.message);
    }
  }

  async function handleDelete(user) {
    try {
      await usersApi.remove(user.id);
      setUsers((prev) => prev.filter((u) => u.id !== user.id));
      notify("success", `User ${user.login} deleted`);
    } catch (e) {
      notify("error", e.message);
    }
  }

  function renderContent() {
    if (status === "loading") {
      return <div className="state">Loading…</div>;
    }
    if (status === "error") {
      return (
        <div className="state">
          <p>Could not load users.</p>
          <button type="button" className="btn btn-secondary" onClick={retryLoad}>
            Retry
          </button>
        </div>
      );
    }
    if (users.length === 0) {
      return (
        <div className="state">
          <p className="state-title">No users yet</p>
          <p>Add the first one — it will appear in this table.</p>
        </div>
      );
    }
    if (filtered.length === 0) {
      return <div className="state">No users match “{filter}”</div>;
    }
    return (
      <UserTable
        users={filtered}
        onEdit={(user) => setModal({ mode: "edit", user })}
        onDelete={handleDelete}
      />
    );
  }

  return (
    <div className="page">
      <main className="card">
        <header className="card-header">
          <div>
            <h1>Users</h1>
            <p className="subtitle">
              {status === "ready" ? `Total: ${users.length}` : "Account management"}
            </p>
          </div>
          <button type="button" className="btn btn-primary" onClick={() => setModal({ mode: "add" })}>
            <span aria-hidden="true">+</span> Add user
          </button>
        </header>

        <div className="toolbar">
          <input
            type="search"
            className="filter-input"
            placeholder="Search by login"
            aria-label="Search by login"
            value={filter}
            onChange={(e) => setFilter(e.target.value)}
          />
        </div>

        {renderContent()}
      </main>

      {modal && (
        <Modal
          title={modal.mode === "add" ? "New user" : "Edit user"}
          onClose={closeModal}
        >
          <UserForm
            initial={modal.user}
            submitLabel={modal.mode === "add" ? "Add" : "Save"}
            onSubmit={modal.mode === "add" ? handleAdd : handleEdit}
            onCancel={closeModal}
          />
        </Modal>
      )}

      <Toast toast={toast} onClose={closeToast} />
    </div>
  );
}
