using System;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void CorrectPasswordCreateObjectOfPasswordClassWithCorrectHash()
        {
            //arrange
            var correctPass = "qwertyui";

            //act
            var pass = new Password(correctPass);

            //assert
            Assert.IsNotNull(pass);
            Assert.IsTrue(pass.Pass == @"22d7fe8c185003c98f97e5d6ced420c7");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void InvalidPasswordThrowsArgumentException()
        {
            //arrange
            var tooShortPass = "short";
            var tooLongPass = "thisismotherfuckingtoolongpasspord";

            //act
            var errPass1 = new Password(tooShortPass);
            var errPass2 = new Password(tooLongPass);

            //assert
        }
    }
}