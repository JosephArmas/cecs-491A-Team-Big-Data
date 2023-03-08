describe('Map Loading Success', function() {
    before(browser => browser.navigateTo('http://localhost:3000'));
  

    it('Login with test user', function(browser) {

      let duration;

      browser
        
        .click('button[id="login"]')
        .click('button[id="sub-login"]')
        .click('button[onclick="regView()"]')
        .perform(function () {
            duration = Date.now();
        })
        .waitForElementVisible('#banner', function() {
            duration = (Date.now() - duration) / 1000;
        })
        .perform(function () {
            if(duration > 10) {
                throw new Error('NFR failed')
            }
        })
    });
  
    after(browser => browser.end());
  });