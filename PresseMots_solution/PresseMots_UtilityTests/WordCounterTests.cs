using PresseMots_DataModels.Entities;
using PresseMots_Utility;

namespace PresseMots_UtilityTests
{
    public class WordCounterTests
    {
        [Theory]        
        [InlineData("Bonjour monde", 2)]
        [InlineData("Est-ce que", 3)]
        [InlineData("J'ai", 2)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        [InlineData("    ", 0)]
        [InlineData("Bonjour monde du cours", 4)]
        [InlineData("Est-ce que ca va", 5)]
        [InlineData("J'ai super faim", 4)]
        [InlineData("       ", 0)]
        [InlineData("Bonjour monde du cours de web", 6)]
        [InlineData("Est-ce que ca va bien", 6)]
        [InlineData("J'ai super faim et soif", 6)]
       // [InlineData("", 0)] déjà fait. 
        [InlineData("                 ", 0)]
        public void CountTest(string text, int expectedCount)
        {
            //Arrange
            var comment = new Comment() { Content = text };
            var wordCounter = new WordCounter();
            //Act
            var result = wordCounter.Count(comment);
            //Assert
            Assert.Equal(result, expectedCount);
        }
    }
}