# Образ берём с зеркала Docker Hub от Google: анонимные скачивания с Docker Hub
# ограничены по IP, и сервер упирался в лимит (429 Too Many Requests)
ARG NODE_IMAGE=mirror.gcr.io/library/node:22-alpine

# Этап 1: Сборка
FROM ${NODE_IMAGE} AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Этап 2: Статический сервер
FROM ${NODE_IMAGE} AS serve
WORKDIR /app
RUN npm install -g serve
COPY --from=build /app/dist ./dist
EXPOSE 3000
CMD ["serve", "-s", "dist"]
