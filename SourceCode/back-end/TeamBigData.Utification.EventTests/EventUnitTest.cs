using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TeamBigData.Utification.EventTests;

[TestClass]
public class EventUnitTest
{
    [TestMethod]
    public void CheckInvalidTitle()
    {
        // Arrange
        var eventManager = new EventManager.EventManager();
        string title = "Beach";

        // Act
        var result = eventManager.IsValidTitle(title);
        
        // Assert
        Assert.IsFalse(result);
        
    }

    [TestMethod]
    public void CheckInvalidDescription()
    {
        // Arrange
        var eventManager = new EventManager.EventManager();
        string description = "*";

        // Act
        var result = eventManager.IsValidDescription(description);
        
        // Assert
        Assert.IsFalse(result); 
    }

    [TestMethod]
    public void ValidNewEvent()
    {
        // Arrange 
        var eventManager = new EventManager.EventManager();
        string title = "Beach Clean up";
        string description = "This is a beach clean up at xyz Beach";
        
        // Act
        var result = eventManager.CreateNewEvent(title, description);
        
        // Assert
        Assert.IsTrue(result.isSuccessful);  
    }
    
    
    
}