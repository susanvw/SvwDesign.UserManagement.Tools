using System;
using System.Reflection;
using Xunit;

namespace SvwDesign.UserManagement.Data.Tests
{
    public class UserStoreFactoryTest
    {
        private static string InvokeNormalizeConnectionString(string connectionString)
        {
            var method = typeof(UserStoreFactory)
                .GetMethod("NormalizeConnectionString", BindingFlags.NonPublic | BindingFlags.Static);
            return (string)method.Invoke(null, new object[] { connectionString });
        }

        [Fact]
        public void NormalizeConnectionString_TrimsWhitespace()
        {
            var input = "   Server=myServer;Database=myDb;   ";
            var expected = "Server=myServer;Database=myDb;";
            var result = InvokeNormalizeConnectionString(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NormalizeConnectionString_ReturnsSameStringIfNoWhitespace()
        {
            var input = "Server=myServer;Database=myDb;";
            var result = InvokeNormalizeConnectionString(input);
            Assert.Equal(input, result);
        }

        [Fact]
        public void NormalizeConnectionString_ThrowsArgumentNullException_WhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => InvokeNormalizeConnectionString(null));
        }

        [Fact]
        public void NormalizeConnectionString_TrimsOnlyLeadingWhitespace()
        {
            var input = "   Server=myServer;";
            var expected = "Server=myServer;";
            var result = InvokeNormalizeConnectionString(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NormalizeConnectionString_TrimsOnlyTrailingWhitespace()
        {
            var input = "Server=myServer;   ";
            var expected = "Server=myServer;";
            var result = InvokeNormalizeConnectionString(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NormalizeConnectionString_EmptyString_ReturnsEmptyString()
        {
            var input = "";
            var result = InvokeNormalizeConnectionString(input);
            Assert.Equal("", result);
        }

        [Fact]
        public void NormalizeConnectionString_WhitespaceOnly_ReturnsEmptyString()
        {
            var input = "    ";
            var result = InvokeNormalizeConnectionString(input);
            Assert.Equal("", result);
        }
    }
}