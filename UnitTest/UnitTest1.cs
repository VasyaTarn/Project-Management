using App;

namespace UnitTest
{
    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void ContainsContainsAttributesTest()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(helper, "new Helper() should not be null");
            Assert.IsTrue(helper.ContainsAttributes("<div style=\"\"></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i style=\"code\" ></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i style=\"code\"  required ></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i style='code'  required></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i required style=\"code\" ></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i required style=\"code\"></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<img onload=\"dangerCode()\" src=\"puc.png\"/>"));
            Assert.IsTrue(helper.ContainsAttributes("<img  src=\"images/img.png\"/>"));
            Assert.IsTrue(helper.ContainsAttributes("<img width=100 />"));
            Assert.IsFalse(helper.ContainsAttributes("<div></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div ></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<br/>"));
            Assert.IsFalse(helper.ContainsAttributes("<br />"));
            Assert.IsFalse(helper.ContainsAttributes("<div requred ></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div required ></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div required></div>"));
        }

        [TestMethod] 
        public void TestEscapeHtml()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(helper, "new Helper() should not be null");
            Dictionary<String, String> testCases = new()
            {
                {"<p>Hello World!</p>", "&lt;p&gt;Hello World!&lt;/p&gt;"},
                {"<img scr='images/image.jpg'>", "&lt;img scr='images/image.jpg'&gt;"},
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    helper.EscapeHtml(testCase.Key),
                    $"{testCase.Value} -- {testCase.Key[0]} + {testCase.Key[1]}"
                    );
            }
        }

        [TestMethod]
        public void TestEscapeHtmlEx()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(helper, "new Helper() should not be null");
            var ex = Assert.ThrowsException<ArgumentNullException>(
                () => helper.EscapeHtml(null!)
                );
            Assert.IsTrue(ex.Message.Contains("HTML"), "HTML should not be null");
            var ex1 = Assert.ThrowsException<ArgumentException>(
                () => helper.EscapeHtml("<<<<html>ndjngjdn</html>")
                );
            Assert.IsTrue(ex1.Message.Contains("correct"), "This should be a correct html file");
            var ex2 = Assert.ThrowsException<ArgumentException>(
                () => helper.EscapeHtml("p>dvd<////p>>>>")
                );
            Assert.IsTrue(ex2.Message.Contains("correct"), "This should be a correct html file");
            var ex3 = Assert.ThrowsException<ArgumentException>(
                () => helper.EscapeHtml("")
                );
            Assert.IsTrue(ex3.Message.Contains("empty"), "HTML must not be empty");
        }

        [TestMethod]
        public void EllipsisTest()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(helper, "new Helper() should not be null");
            Assert.AreEqual(
                "He...",
                helper.Ellipsis("Hello, world", 5));
            Assert.AreEqual(
                "Hel...",
                helper.Ellipsis("Hello, world", 6));
            Assert.AreEqual(
                "Tes...",
                helper.Ellipsis("Test string", 6));
        }

        [TestMethod]
        public void ElipsisExcetionTest()
        {
            Helper helper = new Helper();
            var ex = Assert.ThrowsException<ArgumentNullException>(
                () => helper.Ellipsis(null!, 3)
                );
            Assert.IsTrue(ex.Message.Contains("input"),"Exception should contain 'input' substring");
            var ex1 = Assert.ThrowsException<ArgumentException>(
                () => helper.Ellipsis("dsdgfsd", 2)
                );
            Assert.IsTrue(ex1.Message.Contains("less than 3"), "Exception should contain 'less than 3' substring");
            var ex2 = Assert.ThrowsException<ArgumentException>(
                () => helper.Ellipsis("abcd", 6)
                );
            Assert.IsTrue(ex2.Message.Contains("greater than input length"), "Exception should contain 'greater than input length' substring");
        }


        [TestMethod]
        public void FinalizeTest()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(helper, "new Helper() should not be null");
            Assert.AreEqual(
                "Message1.",
                helper.Finalize("Message1"));
            Assert.AreEqual(
                "Message2.",
                helper.Finalize("Message2."));
            Assert.AreEqual(
                "Message3.",
                helper.Finalize("Message3"));
            Assert.AreEqual(
                "Message4?.",
                helper.Finalize("Message4?"));
        }

        [TestMethod]
        public void CombineUrlTest()
        {
            Helper helper = new Helper();
            Dictionary<String[], String> testCases = new ()
            {
                {new[] {"/home", "index"}, "/home/index"},
                {new[] {"/shop", "cart"}, "/shop/cart"},
                {new[] {"auth/", "logout"}, "/auth/logout"},
                {new[] {"forum/", "topic/"}, "/forum/topic"},
            };
            foreach(var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    helper.CombineUrl(testCase.Key[0], testCase.Key[1]),
                    $"{testCase.Value} -- {testCase.Key[0]} + {testCase.Key[1]}"
                    );
            }
        }

        [TestMethod]
        public void CombineUrlMultiTest()
        {
            Helper helper = new Helper();
            Dictionary<String[], String> testCases = new()
            {
                {new[] {"/home", "index", "123"}, "/home/index/123"},
                {new[] {"/shop", "cart", "/123"}, "/shop/cart/123"},
                {new[] {"auth/", "logout", "123/"}, "/auth/logout/123"},
                {new[] {"forum/", "topic/", "/123/"}, "/forum/topic/123"},
                {new[] { "/home///", "index"}, "/home/index"},
                {new[] { "///home/", "/index"}, "/home/index"},
                {new[] { "home/", "////index"}, "/home/index"},
                {new[] { "shop/", "////cart", "//user/", "..", "//123"}, "/shop/cart/user/123"},
                {new[] { "/shop/", "/cart/", "/user//", "/..//", "//123"}, "/shop/cart/user/123"}
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    helper.CombineUrl(testCase.Key),
                    $"{testCase.Value} -- {testCase.Key[0]} + {testCase.Key[1]}"
                    );
            }
        }

        [TestMethod]
        public void CombineUrlExceptionTest()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(
                () => helper.CombineUrl("/home", null!)
                );
            Assert.AreEqual("/home", helper.CombineUrl("home", null!));
            Assert.AreEqual("/home", helper.CombineUrl("home", null!, null!));
            Assert.AreEqual("/home", helper.CombineUrl("home", null!, null!, null!));
            Assert.AreEqual("/section/subsection", helper.CombineUrl("section///", "///subsection//", null!));
            var ex = Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!, null!));
            Assert.AreEqual("Arguments are null!", ex.Message);
            Assert.AreEqual(
                "Arguments are null!",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!, null!, null!, null!)).Message);
            Assert.AreEqual(
                "Arguments are null!",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!, null!, null!)).Message);
            Assert.AreEqual(
                "Arguments are null!",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!)).Message);
            Assert.AreEqual(
                "Not null argument after null one",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!, "/subsection")).Message
                );
            Assert.AreEqual(
                "Not null argument after null one",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!, null!, "/subsection")).Message
                );
            Assert.AreEqual(
                "Not null argument after null one",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl(null!, "/subsection", null!)).Message
                );
            Assert.AreEqual(
                "Not null argument after null one",
                Assert.ThrowsException<ArgumentException>(() => helper.CombineUrl("section", null!, "/subsection")).Message
                );
        }
    }
}