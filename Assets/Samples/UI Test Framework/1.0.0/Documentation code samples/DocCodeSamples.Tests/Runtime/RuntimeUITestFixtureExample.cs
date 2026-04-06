using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements.TestFramework;

namespace UnityEngine.UIElements.TestFrameworkDocCodeSamples.Runtime.Tests
{
    internal class RuntimeUITestFixtureExample
    {
        // Important: This code appears in our package documentation.
        // If you need to disable it, please do so outside the #region tag.
        #region BasicRuntimeExampleClass
        // This class is contained in a runtime test assembly.
        public class BasicRuntimeExampleClass : RuntimeUITestFixture
        {
            [UnityOneTimeSetUp]
            public IEnumerator UnityOneTimeSetUp()
            {
                // Load the scene and yield a player
                // frame to ensure it is up to date.
                yield return SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
                yield return null;
            }

            [Test]
            public void BasicRuntimeExampleTest()
            {
                // Fetch the UIDocument from the scene.
                UIDocument uiDocument = FetchUIDocument();

                // and pass it to SetUIContent
                // to hook it up for simulation.
                SetUIContent(uiDocument);

                simulate.FrameUpdate();

                Button button = rootVisualElement.Q<Button>("MyButton");
                Assert.That(button, Is.Not.Null);

                // Test steps.
                // ...
            }

            public UIDocument FetchUIDocument()
            {
                // Locate the gameObject that contains your UIDocument content.
                var gameObject = GameObject.Find("UIDocument");

                // Get the UIDocument component.
                UIDocument uiDocument = gameObject.GetComponent<UIDocument>();

                uiDocument.panelSettings = Resources.Load<PanelSettings>("UITestFrameworkDocSamplePanelSettings");
                uiDocument.visualTreeAsset = Resources.Load<VisualTreeAsset>("UITestFrameworkDocSample");

                return uiDocument;
            }
        }
        #endregion
    }
}
