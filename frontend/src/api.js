const BASE_URL = "/api/users";

const STATUS_MESSAGES = {
  404: "Пользователь не найден — возможно, его уже удалили",
  409: "Пользователь с таким email уже существует",
};

// Сервер отвечает в формате ProblemDetails (RFC 9457)
async function errorMessage(response) {
  if (STATUS_MESSAGES[response.status]) return STATUS_MESSAGES[response.status];
  try {
    const problem = await response.json();
    const firstFieldError = problem.errors && Object.values(problem.errors).flat()[0];
    return firstFieldError || problem.title || `Ошибка сервера (${response.status})`;
  } catch {
    return `Ошибка сервера (${response.status})`;
  }
}

async function request(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    headers: { "Content-Type": "application/json", ...options.headers },
  });

  if (!response.ok) {
    throw new Error(await errorMessage(response));
  }

  return response.status === 204 ? null : response.json();
}

export const usersApi = {
  getAll: () => request(BASE_URL),
  create: (user) => request(BASE_URL, { method: "POST", body: JSON.stringify(user) }),
  update: (id, user) => request(`${BASE_URL}/${id}`, { method: "PUT", body: JSON.stringify(user) }),
  remove: (id) => request(`${BASE_URL}/${id}`, { method: "DELETE" }),
};
