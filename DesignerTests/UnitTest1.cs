using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ScraperDesigner;

namespace DesignerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSelectorFromString()
        {
            string selectorString = @"div id=""content"" optional=""true"" class=""panel""";
            Selector selector = Selector.FromString(selectorString);
            Assert.AreEqual(selector.Tag, "div");
            Assert.AreEqual(selector.Attributes["id"], "content");
            Assert.AreEqual(selector.Attributes["optional"], "true");
            Assert.AreEqual(selector.Attributes["class"], "panel");
        }

        [TestMethod]
        public void TestPathFromString()
        {
            string pathString = @"div id=""content""/div id=""main""/ul/li";
            ElementPath path = ElementPath.FromString(pathString);

            List<Selector> selectors = path.path;

            Assert.AreEqual(selectors[0].Tag, "div");
            Assert.AreEqual(selectors[0].Attributes["id"], "content");

            Assert.AreEqual(selectors[1].Tag, "div");
            Assert.AreEqual(selectors[1].Attributes["id"], "main");

            Assert.AreEqual(selectors[2].Tag, "ul");
            Assert.AreEqual(selectors[3].Tag, "li");
        }
    }
}
