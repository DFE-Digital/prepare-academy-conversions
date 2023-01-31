module.exports = {
    root: true,
    plugins: [
      'cypress',
    ],
    extends: [
      'eslint:recommended',
      'plugin:cypress/recommended',
    ],
    "rules": {
        "no-mixed-spaces-and-tabs": 1, // disable rule
        "no-constant-condition": 1,
        "no-unused-vars": 1,
        "no-undef": 1,
        "no-useless-escape": 1,
        "cypress/no-assigning-return-values": 1
      }
  };
  