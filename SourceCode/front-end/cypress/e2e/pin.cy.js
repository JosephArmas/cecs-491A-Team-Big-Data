/// <reference types="cypress" />

describe('pin e2e tests: Reputable User', () => {

  beforeEach(() => {
    // Arrange
    cy.visit('http://localhost:3000');
    cy.get('div[id=map]').should('not.visible')
    cy.get('button[id=login]').click()
    cy.get('#email').type('reputable@gmail.com')
    cy.get('#password').type('password')
    cy.get('button[id=sub-login]').click().wait(500)
    cy.get('#otp-display').invoke('text').then((text) => {
      cy.get('#otp-input').type(text)
      cy.get('button[id=otp-submit]').click()
    }).wait(500)
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
      cy.get('#map').click(100,120);
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

  it('reputable user creates a litter pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,100);
      cy.stub(win, 'prompt').onCall(0).returns('1')
      .onCall(1).returns('Test Litter Pin')
      .onCall(2).returns('cypress test')
    })
    cy.get('#map').wait(500).click(100,90);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Litter Pin')
  })

  it('reputable user creates a group pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,200);
      cy.stub(win, 'prompt').onCall(0).returns('2')
      .onCall(1).returns('Test Group Pin')
      .onCall(2).returns('cypress test');
    });
    cy.get('#map').wait(500).click(100,190);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Group Pin')
  })

  it('reputable user creates a junk pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,300);
      cy.stub(win, 'prompt').onCall(0).returns('3')
      .onCall(1).returns('Test Junk Pin')
      .onCall(2).returns('cypress test');
    });
    cy.get('#map').wait(500).click(100,290);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Junk Pin')
  })

  it('reputable user creates a Abandoned pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,400);
      cy.stub(win, 'prompt').onCall(0).returns('4')
      .onCall(1).returns('Test Abandoned Pin')
      .onCall(2).returns('cypress test');
    });
    cy.get('#map').wait(500).click(100,390);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Abandoned Pin')
  })

  it('reputable user creates a Vandalism pin', () => {
    // Act
    cy.window().then(win => {
      cy.get('#map').click(100,500);
      cy.stub(win, 'prompt').onCall(0).returns('5')
      .onCall(1).returns('Test Vandalism Pin')
      .onCall(2).returns('cypress test')
    });
    cy.get('#map').wait(500).click(100,490);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Vandalism Pin')
  })

  it('reputable user completes pin', () => {
    // Act
    cy.get('#map').wait(500).click(100,490);
    cy.get('#completePin').click()
    // Assert
    cy.get('.gm-style-iw-d').should('not.exist')
  })

  it('reputable user modify pin type', () => {
    // Act
    cy.window().wait(500).then(win => {
      cy.get('#map').click(100,190)
      cy.get('#modifyPin').click()
      cy.stub(win, 'prompt').onCall(0).returns('1')
      .onCall(1).returns('5')
    })
    cy.get('#map').wait(500).click(100,190);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Group Pin')
  })

  it('reputable user modify pin content', () => {
    // Act
    cy.window().wait(500).then(win => {
      cy.wait(500).get('#map').click(100,190)
      cy.get('#modifyPin').click()
      cy.stub(win, 'prompt').onCall(0).returns('2')
      .onCall(1).returns('Modified to be a Junk Pin')
      .onCall(2).returns('Changed description')
    })
    cy.get('#map').wait(500).click(100,190);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Modified to be a Junk Pin')
  })

  it('reputable user deletes own pin', () => {
    // Act
    cy.window().then(win => {
      cy.wait(500).get('#map').click(100,190)
      cy.get('#modifyPin').click()
      cy.stub(win, 'prompt').onCall(0).returns('3')
    });
    // Assert
    cy.get('.gm-style-iw-d').should('not.exist')
  })
}),

describe('pin e2e tests: Regular User', () => {
  
  beforeEach(() => {
    // Arrange
    cy.visit('http://localhost:3000');
    cy.get('div[id=map]').should('not.visible')
    cy.get('button[id=login]').click()
    cy.get('#email').type('regular@gmail.com')
    cy.get('#password').type('password')
    cy.get('button[id=sub-login]').click().wait(500)
    cy.get('#otp-display').invoke('text').then((text) => {
      cy.get('#otp-input').type(text)
      cy.get('button[id=otp-submit]').click()
    }).wait(500)
  })

  it('regular user fails to add a pin', () => {
    // Act
    cy.window().then(win => {
      cy.wait(500).get('#map')
      .click(50,190)
      .click(200,190)
      cy.stub(win, 'prompt').as('prompt')
    });
    // Assert
    cy.get('@prompt').should('not.have.been.called')
  })

  it('regular user completes pin', () => {
    // Act
    cy.get('#map').wait(500).click(100,90);
    cy.get('#completePin').click()
    // Assert
    cy.get('.gm-style-iw-d').should('not.exist')
  })
}),

describe('pin e2e tests: Admin User', () => {

  beforeEach(() => {
    // Arrange
    cy.visit('http://localhost:3000');
    cy.get('div[id=map]').should('not.visible')
    cy.get('button[id=login]').click()
    cy.get('#email').type('admin@gmail.com')
    cy.get('#password').type('password')
    cy.get('button[id=sub-login]').click().wait(500)
    cy.get('#otp-display').invoke('text').then((text) => {
      cy.get('#otp-input').type(text)
      cy.get('button[id=otp-submit]').click()
    }).wait(500)
  })

  it('admin user modify any pin\'s type', () => {
    // Act
    cy.window().wait(500).then(win => {
      cy.get('#map').click(100,290)
      cy.get('#modifyPin').click()
      cy.stub(win, 'prompt').onCall(0).returns('1')
      .onCall(1).returns('5')
    })
    cy.get('#map').wait(500).click(100,290);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Test Junk Pin')
  })

  it('admin user modify pin content', () => {
    // Act
    cy.window().wait(500).then(win => {
      cy.wait(500).get('#map').click(100,390)
      cy.get('#modifyPin').click();
      cy.stub(win, 'prompt').onCall(0).returns('2')
      .onCall(1).returns('Admin Modified')
      .onCall(2).returns('Changed description')
    })
    cy.get('#map').wait(500).click(100,390).wait(500);
    
    // Assert
    cy.get('.gm-style-iw-d').should('contain', 'Admin Modified')
  })

  it('admin user deletes pin', () => {
    // Act
    cy.window().then(win => {
      cy.wait(500).get('#map').click(100,290)
      cy.get('#modifyPin').click()
      cy.stub(win, 'prompt').onCall(0).returns('3')
    });
    // Assert
    cy.get('.gm-style-iw-d').should('not.exist')
  })

  it('admin user deletes another pin', () => {
    // Act
    cy.window().then(win => {
      cy.wait(500).get('#map').click(100,390)
      cy.get('#modifyPin').click()
      cy.stub(win, 'prompt').onCall(0).returns('3')
    });
    // Assert
    cy.get('.gm-style-iw-d').should('not.exist')
  })
})
  