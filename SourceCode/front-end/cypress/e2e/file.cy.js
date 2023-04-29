/// <reference types="cypress" />

describe('File Upload to Pin E2E Test: Reputable User', () => {
    beforeEach(() => {
      // Arrange
      cy.visit('http://localhost:3000');
      cy.get('button[id=login]').click()
      cy.get('#email').type('reputable@gmail.com')
      cy.get('#password').type('password')
      cy.get('button[id=sub-login]').click().wait(500)
      cy.get('#otp-display').invoke('text').then((text) => {
        cy.get('#otp-input').type(text)
        cy.get('button[id=otp-submit]').click()
      }).wait(1000)
    })
  
    it('User can upload File', () => {
        cy.fixture('Schedule.PNG').then(file => {
            cy.get('#fileSelector').selectFile('cypress/fixtures/Schedule.PNG');
        })
        cy.get('#map').click(100,95);
        cy.get('button[id=uploadPic').click().wait(1000);
        cy.get('#PinPic').should('be.visible')
    })

    it('User can update Own File', () => {
        cy.fixture('Schedule2.PNG').then(file => {
            cy.get('#fileSelector').selectFile('cypress/fixtures/Schedule2.PNG');
        })
        cy.get('#map').click(100,95).wait(1000);
        cy.get('button[id=updatePic]').click().wait(1000);
        cy.get('#PinPic').should('be.visible')
    })

    it('User can Delete Own File', () => {
        cy.get('#map').click(100,95).wait(1000);
        cy.get('button[id=deletePic]').click().wait(1000);
        cy.get('#PinPic').should('not.exist')
    })
})

describe('File Upload to Pin E2E Test: Admin User', () => {
    beforeEach(() => {
      // Arrange
      cy.visit('http://localhost:3000');
      cy.get('button[id=login]').click()
      cy.get('#email').type('reputable@gmail.com')
      cy.get('#password').type('password')
      cy.get('button[id=sub-login]').click().wait(500)
      cy.get('#otp-display').invoke('text').then((text) => {
        cy.get('#otp-input').type(text)
        cy.get('button[id=otp-submit]').click()
      }).wait(1000)
    })
  
    it('Admin can upload File to Another Pin', () => {
        cy.fixture('Schedule.PNG').then(file => {
            cy.get('#fileSelector').selectFile('cypress/fixtures/Schedule.PNG');
        })
        cy.get('#map').click(100,95);
        cy.get('button[id=uploadPic').click().wait(1000);
        cy.get('#PinPic').should('be.visible')
    })

    it('Admin can update File on Another Pin', () => {
        cy.fixture('Schedule2.PNG').then(file => {
            cy.get('#fileSelector').selectFile('cypress/fixtures/Schedule2.PNG');
        })
        cy.get('#map').click(100,95).wait(1000);
        cy.get('button[id=updatePic]').click().wait(1000);
        cy.get('#PinPic').should('be.visible')
    })

    it('Admin can Delete File on Another Pin', () => {
        cy.get('#map').click(100,95).wait(1000);
        cy.get('button[id=deletePic]').click().wait(1000);
        cy.get('#PinPic').should('not.exist')
    })
})

describe('File Upload to Profile E2E Test: Reputable User', () => {
    beforeEach(() => {
      // Arrange
      cy.visit('http://localhost:3000');
      cy.get('button[id=login]').click()
      cy.get('#email').type('reputable@gmail.com')
      cy.get('#password').type('password')
      cy.get('button[id=sub-login]').click().wait(500)
      cy.get('#otp-display').invoke('text').then((text) => {
        cy.get('#otp-input').type(text)
        cy.get('button[id=otp-submit]').click()
      });
      cy.get('button[id=profileBtn]').click().wait(1000);
    })
  
    it('User can upload File to Profile', () => {
        cy.fixture('Schedule.PNG').then(file => {
            cy.get('#p-fileSelector').selectFile('cypress/fixtures/Schedule.PNG');
        })
        cy.get('button[id=p-AddPic').click().wait(500);
        cy.get('#profilePic').should('be.visible')
    })

    it('User can update Own File', () => {
        cy.fixture('Schedule2.PNG').then(file => {
            cy.get('#p-fileSelector').selectFile('cypress/fixtures/Schedule2.PNG');
        })
        cy.get('button[id=p-UpdatePic]').click().wait(1000);
        cy.get('#profilePic').should('be.visible')
    })

    it('User can Delete Own File', () => {
        cy.get('button[id=p-DeletePic]').click().wait(1000);
        cy.get('#profilePic').should('be.visible')
    })
})