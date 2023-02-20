using CezaryTomczak.Asteroids.View.Asteroid;
using CezaryTomczak.Asteroids.View.Asteroid.States;
using NUnit.Framework;
using Assert = ModestTree.Assert;
using Zenject;

public class AsteroidsEditModeTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        SignalBusInstaller.Install(Container);

        Container.Inject(this);

        Container.BindFactory<AsteroidStateMoving, AsteroidStateMoving.Factory>().WhenInjectedInto<AsteroidStateFactory>();
        Container.BindFactory<SignalBus, AsteroidStateDestroyed, AsteroidStateDestroyed.Factory>().WhenInjectedInto<AsteroidStateFactory>();
        Container.BindFactory<AsteroidStateHit, AsteroidStateHit.Factory>().WhenInjectedInto<AsteroidStateFactory>();
    }
    
    [Test]
    public void TestBindAsteroidStateFactory()
    {
        Container.Bind<AsteroidStateFactory>().AsSingle();

        Assert.IsNotNull(Container.Resolve<AsteroidStateFactory>());
        Assert.IsEqual(Container.Resolve<AsteroidStateFactory>(), Container.Resolve<AsteroidStateFactory>());
    }
}
