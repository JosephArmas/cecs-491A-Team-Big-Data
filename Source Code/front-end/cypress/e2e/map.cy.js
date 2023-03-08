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

describe('map test cases', () => {
    beforeEach(() => {
      // Arrange
      cy.visit('http://localhost:3000')
    })
  
    it('authenticated user loads map', () => {
      // Act
      cy.get('div[id=map]').should('not.visible')
      cy.get('button[id=login]').click()
      cy.get('button[id=sub-login]').click()
      cy.get('button[id=otpSubmit]').click()

      // Assert
      // Checks if map css has loaded in
      cy.get('div[id=map]').should('have.css', 'background-color', 'rgb(0, 0, 0)')
    })

    it('authenticated user have a restricted in zoom limits', () => {
        // Act
        cy.get('div[id=map]').should('not.visible')
        cy.get('button[id=login]').click()
        cy.get('button[id=sub-login]').click()
        cy.get('button[id=otpSubmit]').click()
        cy.get('button[title="Zoom in"]')
        .click()
        .click()
        .click()

        // Assert
        // Checks if zoom in button is disabled at max zoom
        cy.get('button[title="Zoom in"]').should('be.disabled')
    })

    it('authenticated user have a restricted out zoom limits', () => {
        // Act
        cy.get('div[id=map]').should('not.visible')
        cy.get('button[id=login]').click()
        cy.get('button[id=sub-login]').click()
        cy.get('button[id=otpSubmit]').click()
        cy.get('button[title="Zoom out"]')
        .click()
        .click()
        .click()
        .click()
        .click()
        .click()
        .click()

        // Assert
        // Checks if zoom out button is disabled at max zoom
        cy.get('button[title="Zoom out"]').should('be.disabled')
    })

    it('authenticated user can move in the map', () => {
        // Act
        cy.get('div[id=map]').should('not.visible')
        cy.get('button[id=login]').click()
        cy.get('button[id=sub-login]').click()
        cy.get('button[id=otpSubmit]').click()
  
        // Assert
        cy.get('.map').scrollTo('bottom')
    })

    /*it('unauthenticated user fails to load map', () => {
        // Act
        cy.get('div[id=map]').should('not.visible')
        cy.get('button[id=login]').click()
        cy.get('button[id=sub-login]').click()
        cy.get('button[id=otpSubmit]').click()
  
        // Assert
        cy.get('div[id=map]').should('not.be.visible')
      })*/
  })
  