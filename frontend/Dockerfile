# Используем официальный Node.js
FROM node:20

WORKDIR /app

# Копируем исходники
COPY package*.json ./
RUN npm ci

COPY . .

# Открываем порт
EXPOSE 5173

# Запускаем Vite dev-сервер (либо npm start, если у тебя так)
CMD [ "npm", "run", "dev", "--", "--host", "0.0.0.0" ]
