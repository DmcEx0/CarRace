using NUnit.Framework;
using UnityEditor.UIElements.TestFramework;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements.TestFrameworkDocCodeSamples.Editor.Tests
{
    internal class EditorWindowUITestFixtureExample
    {
        // Important: This code appears in our package documentation.
        // If you need to disable it, please do so outside the #region tag.
        #region BasicEditorWindowExample
        public class BasicEditorWindowExample : EditorWindowUITestFixture<UITestFrameworkDocSampleWindow>
        {
            [Test]
            public void EditorWindowTest()
            {
                // Ensure the window's UI is up to date.
                simulate.FrameUpdate();

                // Use the rootVisualElement property to query for elements
                // within the window created by the test fixture.
                Button button = rootVisualElement.Q<Button>("MyButton");
                Assert.That(button, Is.Not.Null);

                // Test steps.
                // ...
            }

        }
        #endregion

        // Important: This code appears in our package documentation.
        // If you need to disable it, please do so outside the #region tag.
        #region EditorWindowDebugging
        public class EditorWindowDebugging : EditorWindowUITestFixture<UITestFrameworkDocSampleWindow>
        {
            [Test]
            public void TestToDebug()
            {
                // Set debugMode to true for the test
                // you are currently writing or debugging.
                debugMode = true;

                // Test steps.
                // ...

                // The UI will remain open and visible when the test fails.
                // Assert.Fail();
            }
        }
        #endregion
    }
}
