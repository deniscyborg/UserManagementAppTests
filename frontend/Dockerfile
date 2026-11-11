# Images come from Google's Docker Hub mirror: anonymous pulls from Docker Hub
# are rate-limited per IP, and the server kept hitting the limit (429 Too Many Requests)
ARG NODE_IMAGE=mirror.gcr.io/library/node:22-alpine

# Stage 1: build
FROM ${NODE_IMAGE} AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Stage 2: static server
FROM ${NODE_IMAGE} AS serve
WORKDIR /app
RUN npm install -g serve
COPY --from=build /app/dist ./dist
EXPOSE 3000
CMD ["serve", "-s", "dist"]
