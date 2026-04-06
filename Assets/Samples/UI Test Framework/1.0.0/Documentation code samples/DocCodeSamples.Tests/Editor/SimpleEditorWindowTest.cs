using NUnit.Framework;
using UnityEditor.UIElements.TestFramework;
using UnityEngine.UIElements;

public class SimpleEditorWindowTest : EditorWindowUITestFixture<SimpleEditorWindow>
{
    [Test]
    public void ClickButtonTest()
    {
        simulate.FrameUpdate();

        Button button = rootVisualElement.Q<Button>();

        Assume.That(button, Is.Not.Null);
        Assume.That(button.text, Is.EqualTo("Button not clicked"));

        simulate.Click(button);
        simulate.FrameUpdate();

        Assert.That(button.text, Is.EqualTo("Button was clicked!"));
    }
}
