class DownloadPage {
    public slug = 'advisory-board/download';

    public downloadProjectTemplate(): this {
        cy.get('h1').should('contain.text', 'Download project template');

        cy.intercept('GET', '**/advisory-board/download/GenerateDocument').as('downloadRequest');

        cy.get('[data-test="download-htb"]').click();

        cy.wait('@downloadRequest').then((interception) => {
            if (interception.response) {
                expect(interception.response.statusCode).to.eq(200); // Ensure the request is successful
                expect(interception.response.headers['content-disposition']).to.include('attachment'); // Check if it's a download response
                expect(interception.response.headers['content-type']).to.include(
                    'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
                ); // Ensure it's a .docx file
            }
        });

        return this;
    }
}

const downloadPage = new DownloadPage();

export default downloadPage;
