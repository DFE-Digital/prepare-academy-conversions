FROM cypress/base:16.18.1

COPY ./cypress ./cypress
COPY cypress.config.ts .
COPY package-lock.json .
COPY package.json .
COPY tsconfig.json .

RUN npm install

ENTRYPOINT ["npm", "run", "cy:run"]