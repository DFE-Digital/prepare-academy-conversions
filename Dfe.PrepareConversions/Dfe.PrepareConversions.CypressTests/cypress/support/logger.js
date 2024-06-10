export class Logger
{
    static log(message)
    {
        cy.task("log", message);
    }
}