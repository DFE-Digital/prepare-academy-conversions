export class Logger {
    static log(message: string): void {
        cy.task('log', message);
    }
}
