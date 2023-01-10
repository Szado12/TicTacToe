FROM node:14-alpine as builder

WORKDIR /app

COPY ./package.json .
COPY ./package-lock.json .

RUN npm install

COPY . .

RUN npm run build

FROM nginx
EXPOSE 3000
COPY nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=builder /app/build /usr/sahre/nginx/html