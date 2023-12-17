using GameEngine.Objects;
using GameLibrary.AmmunitionDecorator;
using GameLibrary.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class WeaponTests
    {
        [TestMethod]
        public void TestBulletWeapon()
        {
            Weapons cannon = new BulletWeapon();
            Assert.IsTrue(cannon.GetAmmo() is Bullet);
        }
    }
}
