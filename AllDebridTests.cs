using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibAllDebrid
{
    [TestClass]
    public class AllDebridTest
    {

        private string username = "username";
        private string password = "password";

        [TestMethod()]
        public void TestLoginSuccess()
        {
            AllDebrid all = new AllDebrid(username: this.username, password: this.password);
            PrivateObject obj = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(all);
            var result = obj.Invoke("Login", this.username, this.password);
            var attrCookie = obj.GetFieldOrProperty("_cookie");
            Assert.IsNotNull(attrCookie);
            Assert.IsTrue((bool)result);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidCredentials),"A userId of null was inappropriately allowed.")]
        public void TestLoginFail()
        {
            AllDebrid all = new AllDebrid(username: this.username, password: "false");
            PrivateObject obj = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(all);
            var result = obj.Invoke("Login", "WrongUsername", "WrongPassword");
            Assert.IsFalse((bool)result);
            Assert.IsNull(obj.GetField("_cookie"));
        }

        [TestMethod()]
        public void TestGetUrlSuccess()
        {
            AllDebrid debrid = new AllDebrid(username: this.username, password: this.password);
            var result = debrid.GetUrlDebride(url: "http://uptobox.com/4uf113r6fmwg");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Link));
        }
    }
}
