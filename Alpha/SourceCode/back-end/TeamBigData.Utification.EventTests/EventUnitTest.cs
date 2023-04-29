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
    public void ValidPinBound()
    {
        // Arrange
        var eventManager = new EventManager.EventManager();
        
        // Huntington Beach
        var lat = 33.6603000;
        var lng = -117.9992300;

        // Act
        var result = eventManager.IsValidPinBound(lat, lng);
        
        // Assert
        Assert.IsTrue(result);  
    }

    [TestMethod]
    public void InvalidPinBound()
    {
        // Arrange
        var eventManager = new EventManager.EventManager();
        
        // Las Vegas
        var lat = 36.181271;
        var lng = -115.134132;

        // Act
        var result = eventManager.IsValidPinBound(lat, lng);
        
        // Assert
        Assert.IsFalse(result);   
    }
    
    
    
}