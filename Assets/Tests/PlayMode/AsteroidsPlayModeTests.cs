using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class AsteroidsPlayModeTests : SceneTestFixture
{
    [UnityTest]
    public IEnumerator TestAsteroidsScene()
    {
        yield return LoadScene("Asteroids");
        yield return new WaitForSeconds(2.0f);
    }
}
