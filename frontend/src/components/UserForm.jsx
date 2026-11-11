import { useState } from "react";

const EMAIL_PATTERN = /^[^@\s]+@[^@\s]+\.ru$/i;

const FIELDS = [
  { name: "name", label: "Name", type: "text", placeholder: "John", autoComplete: "given-name" },
  { name: "surname", label: "Surname", type: "text", placeholder: "Smith", autoComplete: "family-name" },
  { name: "email", label: "Email", type: "email", placeholder: "example@mail.ru", autoComplete: "email" },
];

function validate({ name, surname, email }) {
  return {
    name: name.trim().length >= 3 ? null : "At least 3 characters",
    surname: surname.trim().length >= 3 ? null : "At least 3 characters",
    email: EMAIL_PATTERN.test(email.trim())
      ? null
      : "Email must contain @ and end with .ru",
  };
}

export default function UserForm({ initial, submitLabel, onSubmit, onCancel }) {
  const [values, setValues] = useState({
    name: initial?.name ?? "",
    surname: initial?.surname ?? "",
    email: initial?.email ?? "",
  });
  const [touched, setTouched] = useState({});
  const [submitting, setSubmitting] = useState(false);

  const errors = validate(values);
  const hasErrors = Object.values(errors).some(Boolean);

  function handleChange(e) {
    const { name, value } = e.target;
    setValues((prev) => ({ ...prev, [name]: value }));
  }

  function handleBlur(e) {
    const { name } = e.target;
    setTouched((prev) => ({ ...prev, [name]: true }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    if (hasErrors) {
      setTouched({ name: true, surname: true, email: true });
      return;
    }

    setSubmitting(true);
    try {
      await onSubmit({
        name: values.name.trim(),
        surname: values.surname.trim(),
        email: values.email.trim(),
      });
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className="user-form" onSubmit={handleSubmit} noValidate>
      {FIELDS.map(({ name, label, ...inputProps }, index) => {
        const error = touched[name] ? errors[name] : null;
        const errorId = `${name}-error`;
        return (
          <div className="form-group" key={name}>
            <label htmlFor={`field-${name}`}>{label}</label>
            <input
              id={`field-${name}`}
              name={name}
              value={values[name]}
              onChange={handleChange}
              onBlur={handleBlur}
              aria-invalid={Boolean(error)}
              aria-describedby={error ? errorId : undefined}
              autoFocus={index === 0}
              {...inputProps}
            />
            {error && (
              <div className="error-message" id={errorId}>
                {error}
              </div>
            )}
          </div>
        );
      })}

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          Cancel
        </button>
        <button type="submit" className="btn btn-primary" disabled={submitting}>
          {submitting ? "Saving…" : submitLabel}
        </button>
      </div>
    </form>
  );
}
