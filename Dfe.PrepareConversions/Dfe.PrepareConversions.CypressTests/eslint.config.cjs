module.exports = [
  {
    files: [
      ".js", ".jsx", ".ts", ".tsx"
    ],
    plugins: [
      "cypress"
    ],
    rules: {
      "cypress/no-assigning-return-values": "error",
      "cypress/no-unnecessary-waiting": "error",
      "cypress/assertion-before-screenshot": "error",
      "cypress/no-force": "error",
      "cypress/no-async-tests": "error",
      "cypress/no-pause": "error"
    },
    env: {
      "cypress/globals": true,
      "node": true
    },
    extends: [
      "plugin:cypress/recommended",
      "eslint:recommended"
    ],
    parserOptions: {
      "ecmaVersion": 2022
    }
  }
]
