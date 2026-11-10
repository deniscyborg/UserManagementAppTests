import { useEffect } from "react";

const TOAST_DURATION_MS = 3500;

export default function Toast({ toast, onClose }) {
  useEffect(() => {
    if (!toast) return undefined;
    const timer = setTimeout(onClose, TOAST_DURATION_MS);
    return () => clearTimeout(timer);
  }, [toast, onClose]);

  if (!toast) return null;

  return (
    <div
      id={toast.type === "success" ? "successMsg" : "errorMsg"}
      className={`toast toast-${toast.type}`}
      role={toast.type === "error" ? "alert" : "status"}
    >
      {toast.message}
    </div>
  );
}
