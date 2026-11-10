const BASE_URL = "/api/users";

async function request(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    headers: { "Content-Type": "application/json", ...options.headers },
  });

  if (!response.ok) {
    let message = `Ошибка сервера (${response.status})`;
    try {
      const body = await response.json();
      if (body?.error) message = body.error;
    } catch {
      // тело ответа не JSON — оставляем сообщение по коду
    }
    throw new Error(message);
  }

  return response.status === 204 ? null : response.json();
}

export const usersApi = {
  getAll: () => request(BASE_URL),
  create: (user) => request(BASE_URL, { method: "POST", body: JSON.stringify(user) }),
  update: (id, user) => request(`${BASE_URL}/${id}`, { method: "PUT", body: JSON.stringify(user) }),
  remove: (id) => request(`${BASE_URL}/${id}`, { method: "DELETE" }),
};
