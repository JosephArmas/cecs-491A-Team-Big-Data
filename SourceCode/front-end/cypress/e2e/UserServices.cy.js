describe('UserServices e2e tests: Service Creation', () => {
  beforeEach(() => {
    // Arrange
    cy.visit('http://localhost:3000');
    cy.get('div[id=map]').should('not.visible')
    cy.get('button[id=login]').click()
    cy.get('#email').type('endTest@gmail.com')
    cy.get('#password').type('password')
    cy.get('button[id=sub-login]').click().wait(500)
    cy.get('#otp-display').invoke('text').then((text) => {
      cy.get('#otp-input').type(text)
      cy.get('button[id=otp-submit]').click()
    }).wait(500)
  })

  it('User can create service account', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type(Math.random().toString())
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('15625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Successfully inserted into database')
    
  })

  it('User can update service account', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type(Math.random().toString())
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('15625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Successfully Updated Service')
    
  })

  it('User can delete service account', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('e2eTestingBGone')
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('15625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#serviceDeleteBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Service Successfully Deleted and Role was updated to regular')
    
  })

  it('User gets stopped because invalid character for service account title', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('$tfdsfdsfds')
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('15625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Syntax Error. Invalid Title Characters')
    
  })

  it('User gets stopped because length is too short for service account title', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('red')
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('15625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Syntax Error. Title under 4 characters')
    
  })

  it('User gets stopped because length is too long for service account title', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('qwertyqwertyqwertyqwertyqwertydre')
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('15625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Syntax Error. Title over 30 Character Limit')
    
  })

  it('User gets stopped because area or country code is wrong for service account phone', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('Phone')
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('25625388999')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('phone area or country code')
    
  })

  it('User gets stopped because length is wrong for service account phone', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('Phone')
cy.get('#serviceDesc').type('Less good tests')
cy.get('#servicePhone').type('1562538899977777776777777')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('phone length')
    
  })

  it('User gets stopped because invalid characters for service account description', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('Phone')
cy.get('#serviceDesc').type('$erererererere')
cy.get('#servicePhone').type('1561234567')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Syntax Error. Invalid Description Characters')
    
  })

  it('User gets stopped because words exceed 150 for service account description', () => {
    // Act
    cy.get('#home-serviceBtn')
    .click()
    cy.get('#servCrea').click()
cy.get('#serviceTitle').type('Phone')
cy.get('#serviceDesc').type('                                                                                                                                                                  ')
cy.get('#servicePhone').type('1561234567')
cy.get('#serviceWebsite').type('www.somewhere.com')
cy.get('#submitBtn').click()

cy.wait(500)
    
    // Assert
    cy.get('#responseBox').contains('Syntax Error. Description is over 150 word limit')
    
  })

})