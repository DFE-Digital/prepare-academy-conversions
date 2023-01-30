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
        "no-mixed-spaces-and-tabs": 0, // disable rule
        "no-constant-condition": 0,
        "no-unused-vars": 0,
        "no-undef": 1,
        "no-useless-escape": 0,
        "cypress/no-assigning-return-values": 0
      }
  };