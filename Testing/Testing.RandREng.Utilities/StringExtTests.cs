using RandREng.Utility;
using Xunit;

namespace Testing.RandREng.Utilities
{
    public class StringExtTests
    {
        [Fact]
        public void SafeTrimTest1()
        {
            string test = null;
            string results = test.SafeTrim();
            Assert.Equal("", results);
        }

        [Fact]
        public void SafeTrimTest2()
        {
            string test = "Test               ";
            string results = test.SafeTrim();
            Assert.Equal("Test", results);
        }

        [Fact]
        public void SafeTrimMaxTest1()
        {
            string test = null;
            string results = test.SafeTrimMax(8);
            Assert.Equal("", results);
        }

        [Fact]
        public void SafeTrimMaxTest2()
        {
            string test = "Test               ";
            string results = test.SafeTrimMax(8);
            Assert.Equal("Test", results);
        }

        [Fact]
        public void SafeTrimMaxTest3()
        {
            string test = "TestTestTest               ";
            string results = test.SafeTrimMax(8);
            Assert.Equal("TestTest", results);
        }

        [Fact]
        public void IsValidTest1()
        {
            string test = null;
            Assert.False(test.IsValid());
        }

        [Fact]
        public void IsValidTest2()
        {
            string test = "";
            Assert.False(test.IsValid());
        }

        [Fact]
        public void IsValidTest3()
        {
            string test = "              \r\n\r\n\t";
            Assert.False(test.IsValid());
        }

        [Fact]
        public void IsValidTest4()
        {
            string test = "   a           \r\n\r\n\t";
            Assert.True(test.IsValid());
        }


        [Fact]
        public void IsEmptyTest1()
        {
            string test = null;
            Assert.True(test.IsEmpty());
        }

        [Fact]
        public void IsEmptyTest2()
        {
            string test = "";
            Assert.True(test.IsEmpty());
        }

        [Fact]
        public void IsEmptyTest3()
        {
            string test = "              \r\n\r\n\t";
            Assert.True(test.IsEmpty());
        }

        [Fact]
        public void IsEmptyTest4()
        {
            string test = "   a           \r\n\r\n\t";
            Assert.False(test.IsEmpty());
        }

        [Fact]
        public void RemoveExtraSpacesTest1()
        {
            string test = "This is a test";
            string result = test.RemoveExtraSpaces();
            Assert.Equal(test, result);
        }

        [Fact]
        public void RemoveExtraSpacesTest2()
        {
            string test = "This    is    a    test";
            string expected = "This is a test";
            string result = test.RemoveExtraSpaces();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RemoveExtraSpacesTest3()
        {
            string test = "This\tis \r\n    a    test";
            string expected = "This is a test";
            string result = test.RemoveExtraSpaces();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoesNotContainTest1()
        {
            string test = null;
            Assert.True(test.DoesNotContain("test"));
        }

        [Fact]
        public void DoesNotContainTest2()
        {
            string test = "";
            Assert.True(test.DoesNotContain("test"));
        }

        [Fact]
        public void DoesNotContainTest3()
        {
            string test = "this should pass";
            Assert.True(test.DoesNotContain("fail"));
        }

        [Fact]
        public void DoesNotContainTest4()
        {
            string test = "this should fail";
            Assert.False(test.DoesNotContain("fail"));
        }


        [Fact]
        public void ContainsNumbersTest1()
        {
            string test = null;
            Assert.False(test.ContainsNumbers());
        }

        [Fact]
        public void ContainsNumbersTest2()
        {
            string test = "";
            Assert.False(test.ContainsNumbers());
        }

        [Fact]
        public void ContainsNumbersTest3()
        {
            string test = "this should faile";
            Assert.False(test.ContainsNumbers());
        }

        [Fact]
        public void ContainsNumbersTest4()
        {
            string test = "this should pass 123";
            Assert.True(test.ContainsNumbers());
        }

        [Fact]
        public void ContainsNumbersTest5()
        {
            string test = "123this should pass";
            Assert.True(test.ContainsNumbers());
        }

        [Fact]
        public void ContainsNumbersTest6()
        {
            string test = "this 123 should pass";
            Assert.True(test.ContainsNumbers());
        }

    }
}
