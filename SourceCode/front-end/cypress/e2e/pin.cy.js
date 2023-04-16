/// <reference types="cypress" />

describe('pin e2e test cases', () => {
  beforeEach(() => {
    // Arrange
    cy.visit('http://localhost:3000');
    cy.get('div[id=map]').should('not.visible')
    cy.get('button[id=login]').click()
    cy.get('#email').type('test@gmail.com')
    cy.get('#password').type('password')
    cy.get('button[id=sub-login]').click().wait(500)
    cy.get('#otp-display').invoke('text').then((text) => {
      cy.get('#otp-input').type(text)
      cy.get('button[id=otp-submit]').click()
    })
  })

  it('user can only make pins within California bounds', () => {
    // Act
    cy.get('button[title="Zoom out"]')
    .click()
    .click()
    .click()
    .click()
    .click()
    .click()
    .click()
    cy.get('#map').click(100,500);
    
    // Assert
    cy.get('#errors').invoke('text').should('eq','Pin is placed out of bounds... ')
    
  })

  it('pin type validation', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,100);
      // Only valid inputs 1-5
      cy.stub(win, 'prompt').onCall(0).returns('G')
    })
    
    // Assert
    cy.get('#errors').should('contain', 'Invalid Pin Input...')
  })

  it('pin title validation', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,100);
      cy.stub(win, 'prompt').onCall(0).returns('1')
      // Invalid characters are inserted
      .onCall(1).returns('#!@()*&()%*!')
    })
    
    // Assert
    cy.get('#errors').should('contain', 'Invalid Title Input...')
  })

  it('pin description validation', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,50);
      cy.stub(win, 'prompt').onCall(0).returns('1')
      .onCall(1).returns('Test Litter Pin')
      .onCall(2).returns('#!@()*&()%*!')
    })
    
    // Assert
    cy.get('#errors').should('contain', 'Invalid Description Input...')
  })

  it('authenticated user creates a litter pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,100);
      cy.stub(win, 'prompt').onCall(0).returns('1')
      .onCall(1).returns('Test Litter Pin')
      .onCall(2).returns('cypress test')
    })
    cy.get('#map').wait(500).click(500,260);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Litter Pin')
  })
  it('authenticated user creates a group pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,200);
      cy.stub(win, 'prompt').onCall(0).returns('2')
      .onCall(1).returns('Test Group Pin')
      .onCall(2).returns('cypress test');
    });
    cy.get('#map').wait(500).click(500,260);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Group Pin')
  })
  it('authenticated user creates a junk pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,300);
      cy.stub(win, 'prompt').onCall(0).returns('3')
      .onCall(1).returns('Test Junk Pin')
      .onCall(2).returns('cypress test');
    });
    cy.get('#map').wait(500).click(500,260);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Junk Pin')
  })
  it('authenticated user creates a Abandoned pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,400);
      cy.stub(win, 'prompt').onCall(0).returns('4')
      .onCall(1).returns('Test Abandoned Pin')
      .onCall(2).returns('cypress test');
    });
    cy.get('#map').wait(500).click(500,260);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Abandoned Pin')
  })
  it('authenticated user creates a Vandalism pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,500);
      cy.stub(win, 'prompt').onCall(0).returns('5')
      .onCall(1).returns('Test Vandalism Pin')
      .onCall(2).returns('cypress test')
    });
    cy.get('#map').wait(500).click(500,260);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Vandalism Pin')
  })

})
  