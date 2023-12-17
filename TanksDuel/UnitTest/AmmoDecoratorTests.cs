using GameEngine.Game;
using GameEngine.Input;
using GameEngine.Objects;
using GameLibrary.AmmunitionDecorator;
using GameLibrary.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace UnitTest
{
    [TestClass]
    public class AmmoDecoratorTests
    {
        [TestMethod]
        public void TestAmmoDamageDecorator()
        {
            Ammo baseAmmo = new Bullet(new GameField(), new Point(0, 0), new NormalTank(), Direction.Up);
            int[] textures = new int[] { 1, 2, 3 };

            AmmoDamageDecorator damageDecorator = new AmmoDamageDecorator(baseAmmo, textures);

            Assert.AreEqual(baseAmmo.Damage + 20, damageDecorator.Damage);
        }
    }
}


