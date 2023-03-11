/// <reference types="cypress" />

// Welcome to Cypress!
//
// This spec file contains a variety of sample tests
// for a todo list app that are designed to demonstrate
// the power of writing tests in Cypress.
//
// To learn more about how Cypress works and
// what makes it such an awesome testing tool,
// please read our getting started guide:
// https://on.cypress.io/introduction-to-cypress

describe('pin e2e test cases', () => {
    beforeEach(() => {
      // Arrange
      cy.visit('http://localhost:3000');
    })
  
    it('authenticated user creates a litter pin', () => {
      // Act
      cy.get('div[id=map]').should('not.visible');
      cy.get('button[id=login]').click();
      cy.get('button[id=sub-login]').click();
      cy.get('button[id=otpSubmit]').click();
      cy.window().then(win => {
        cy.get('div[id=map]').click('center');
        cy.stub(win, 'prompt').returns(1).returns("Test");
        //cy.get('OK').click();
        //cy.stub(win, 'prompt').returns("Test Litter Pin");
        //cy.get('OK').click();
      });

      // Assert
      // Checks if map css has loaded in
      cy.get('div[id=map]').should('have.css', 'background-color', 'rgb(0, 0, 0)')
    })
  })
  