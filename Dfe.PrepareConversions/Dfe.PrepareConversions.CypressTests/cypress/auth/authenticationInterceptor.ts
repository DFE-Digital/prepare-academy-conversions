import { CypressTestSecret, EnvUrl } from '../constants/cypressConstants';

export class AuthenticationInterceptor {
    register() {
        cy.env([CypressTestSecret]).then(({ cypressTestSecret }) => {
            cy.intercept(
                { url: Cypress.expose(EnvUrl) + '/**', middleware: true },
                //Add authorization to all Cypress requests
                (req) => {
                    req.headers['Authorization'] = `Bearer ${cypressTestSecret}`;
                    req.headers['AuthorizationRole'] = 'conversions.create';
                }
            ).as('AuthInterceptor');
        });
    }
}
