const eslintPluginCypress = require('eslint-plugin-cypress');
const eslintPluginPrettier = require('eslint-plugin-prettier');
const eslintConfigPrettier = require('eslint-config-prettier');
const typescriptParser = require('@typescript-eslint/parser');

module.exports = [
    {
        ignores: [
            'node_modules/**',
            'cypress/reports/**',
            'cypress/screenshots/**',
            'cypress/videos/**',
            'cypress/downloads/**',
        ],
    },
    eslintConfigPrettier,
    {
        files: ['**/*.{ts,tsx}'],
        languageOptions: {
            parser: typescriptParser,
            parserOptions: {
                ecmaVersion: 2022,
                sourceType: 'module',
            },
            globals: {
                cy: 'readonly',
                Cypress: 'readonly',
                describe: 'readonly',
                it: 'readonly',
                before: 'readonly',
                beforeEach: 'readonly',
                after: 'readonly',
                afterEach: 'readonly',
                expect: 'readonly',
                context: 'readonly',
                process: 'readonly',
                require: 'readonly',
                module: 'readonly',
                __dirname: 'readonly',
                console: 'readonly',
            },
        },
        plugins: {
            cypress: eslintPluginCypress,
            prettier: eslintPluginPrettier,
        },
        rules: {
            // Cypress specific rules (manually configured, not using recommended)
            'cypress/no-assigning-return-values': 'error',
            'cypress/no-unnecessary-waiting': 'warn',
            'cypress/assertion-before-screenshot': 'warn',
            'cypress/no-force': 'warn',
            'cypress/no-async-tests': 'error',
            'cypress/no-pause': 'error',
            'cypress/unsafe-to-chain-command': 'warn',

            // Prettier integration - shows formatting issues as ESLint errors
            'prettier/prettier': 'error',

            // General best practices
            'no-unused-vars': ['warn', { argsIgnorePattern: '^_' }],
            'no-console': 'off',
            'prefer-const': 'error',
            'no-var': 'error',
            eqeqeq: ['warn', 'always'],
        },
    },
    {
        // Type declaration files - disable unused vars checking
        files: ['**/*.d.ts'],
        rules: {
            'no-unused-vars': 'off',
        },
    },
    {
        files: ['**/*.{js,jsx,cjs}'],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: 'module',
            globals: {
                require: 'readonly',
                module: 'readonly',
                __dirname: 'readonly',
                console: 'readonly',
                process: 'readonly',
            },
        },
        plugins: {
            prettier: eslintPluginPrettier,
        },
        rules: {
            'prettier/prettier': 'error',
            'no-unused-vars': ['warn', { argsIgnorePattern: '^_' }],
            'no-console': 'off',
            'prefer-const': 'error',
            'no-var': 'error',
        },
    },
];
