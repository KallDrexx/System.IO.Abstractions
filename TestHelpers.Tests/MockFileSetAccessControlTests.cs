﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Security.AccessControl;
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockFileSetAccessControlTests
    {
        [TestCase(" ")]
        [TestCase("   ")]
        public void MockFile_SetAccessControl_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path)
        {
            if (MockUnixSupport.IsUnixPlatform()) 
            {
                Assert.Inconclusive("Unix does not support ACLs.");
            }
            
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileSecurity = new FileSecurity();

            // Act
            TestDelegate action = () => fileSystem.File.SetAccessControl(path, fileSecurity);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.That(exception.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void MockFile_SetAccessControl_ShouldThrowFileNotFoundExceptionIfFileDoesNotExistInMockData()
        {
            if (MockUnixSupport.IsUnixPlatform()) 
            {
                Assert.Inconclusive("Unix does not support ACLs.");
            }

            // Arrange
            var fileSystem = new MockFileSystem();
            var expectedFileName = XFS.Path(@"c:\a.txt");
            var fileSecurity = new FileSecurity();

            // Act
            TestDelegate action = () => fileSystem.File.SetAccessControl(expectedFileName, fileSecurity);

            // Assert
            var exception = Assert.Throws<FileNotFoundException>(action);
            Assert.That(exception.FileName, Is.EqualTo(expectedFileName));
        }

        [Test]
        public void MockFile_SetAccessControl_ShouldReturnAccessControlOfFileData()
        {
            if (MockUnixSupport.IsUnixPlatform()) 
            {
                Assert.Inconclusive("Unix does not support ACLs.");
            }

            // Arrange
            var filePath = XFS.Path(@"c:\a.txt");
            var fileData = new MockFileData("Test content");

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                { filePath, fileData }
            });

            // Act
            var expectedAccessControl = new FileSecurity();
            expectedAccessControl.SetAccessRuleProtection(false, false);
            fileSystem.File.SetAccessControl(filePath, expectedAccessControl);

            // Assert
            var accessControl = fileSystem.File.GetAccessControl(filePath);
            Assert.That(accessControl, Is.EqualTo(expectedAccessControl));
        }
    }
}
