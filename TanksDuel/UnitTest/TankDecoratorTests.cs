using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLibrary.Decorators;

namespace UnitTest
{
    
    [TestClass]
    public class TankDecoratorTests
    {
        [TestMethod]
        public void TestSlowTankDecorator()
        {
            NormalTank normal = new NormalTank();
            var slow = new SlowTank(normal);
            Assert.IsTrue(normal.Speed == slow.Speed + 4);
        }

        [TestMethod]
        public void TestArmorTankDecorator()
        {
            NormalTank normal = new NormalTank();
            var armor = new ArmorTank(normal);
            Assert.IsTrue(normal.Armor == armor.Armor - 30);
        }

        [TestMethod]
        public void TestBulletTankDecorator()
        {
            NormalTank normal = new NormalTank();
            int normalBullet = normal.Ammunition;
            var bullet = new BulletTank(normal);
            int bulletBullet = bullet.Ammunition;
            Assert.IsTrue(normalBullet == bulletBullet - 10);
        }

        [TestMethod]
        public void TestFastTankDecorator()
        {
            NormalTank normal = new NormalTank();
            var fast = new FastTank(normal);
            Assert.IsTrue(normal.Speed == fast.Speed - 10);
        }

        [TestMethod]
        public void TestFuelTankDecorator()
        {
            NormalTank normal = new NormalTank();
            int normalFuel = normal.Fuel;
            var fuel = new FuelTank(normal);
            int fuelFuel = fuel.Fuel;
            Assert.IsTrue(normalFuel == fuelFuel - 300);
        }
    }
}
