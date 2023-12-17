using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLibrary.AmmunitionDecorator;
using GameEngine.Objects;
using GameLibrary.Weapons;
using GameEngine.Input;
using GameEngine.Game;
using GameLibrary.Bonuses;
using GameLibrary.Factories;
using System.Drawing;
using Moq;

namespace UnitTest
{

    [TestClass]
    public class BonusFactoryTests
    {
        public class TextureRepositoryWrapper
        {
            public virtual int[] GetTexture(string name) => TextureRepository.Get(name);

        }

        [TestMethod]
        public void TestAmmunitionBonus()
        {
            GameField gameField = new GameField();
            var textureRepositoryWrapperMock = new Mock<TextureRepositoryWrapper>();

            textureRepositoryWrapperMock.Setup(tr => tr.GetTexture(It.IsAny<string>())).Returns(new int[] { 1, 2, 3 });

            int[] ammunitionTextures = textureRepositoryWrapperMock.Object.GetTexture("addBulletsBonus");

            AmmunitionBonusFactory ammunitionBonusFactory = new AmmunitionBonusFactory(gameField, ammunitionTextures);

            Bonus bonus = ammunitionBonusFactory.Spawn(new Point(100, 100));

            Assert.IsInstanceOfType(bonus, typeof(AmmunitionBonus));
        }

        [TestMethod]
        public void TestArmorBonus()
        {
            GameField gameField = new GameField();
            var textureRepositoryWrapperMock = new Mock<TextureRepositoryWrapper>();

            textureRepositoryWrapperMock.Setup(tr => tr.GetTexture(It.IsAny<string>())).Returns(new int[] { 1, 2, 3 });

            int[] armorTextures = textureRepositoryWrapperMock.Object.GetTexture("armorBonus");

            ArmorBonusFactory armorBonusFactory = new ArmorBonusFactory(gameField, armorTextures);

            Bonus bonus = armorBonusFactory.Spawn(new Point(100, 100));

            Assert.IsInstanceOfType(bonus, typeof(ArmorBonus));
        }

        [TestMethod]
        public void TestDamageBonus()
        {
            GameField gameField = new GameField();
            var textureRepositoryWrapperMock = new Mock<TextureRepositoryWrapper>();

            textureRepositoryWrapperMock.Setup(tr => tr.GetTexture(It.IsAny<string>())).Returns(new int[] { 1, 2, 3 });

            int[] armorTextures = textureRepositoryWrapperMock.Object.GetTexture("damageBonus");

            DamageBonusFactory damageBonusFactory = new DamageBonusFactory(gameField, armorTextures);

            Bonus bonus = damageBonusFactory.Spawn(new Point(100, 100));

            Assert.IsInstanceOfType(bonus, typeof(DamageBonus));
        }

        [TestMethod]
        public void TestFuelBonus()
        {
            GameField gameField = new GameField();
            var textureRepositoryWrapperMock = new Mock<TextureRepositoryWrapper>();

            textureRepositoryWrapperMock.Setup(tr => tr.GetTexture(It.IsAny<string>())).Returns(new int[] { 1, 2, 3 });

            int[] armorTextures = textureRepositoryWrapperMock.Object.GetTexture("fuelBonus");

            FuelBonusFactory fuelBonusFactory = new FuelBonusFactory(gameField, armorTextures);

            Bonus bonus = fuelBonusFactory.Spawn(new Point(100, 100));

            Assert.IsInstanceOfType(bonus, typeof(FuelBonus));
        }

        [TestMethod]
        public void TestSpeedBonus()
        {
            GameField gameField = new GameField();
            var textureRepositoryWrapperMock = new Mock<TextureRepositoryWrapper>();

            textureRepositoryWrapperMock.Setup(tr => tr.GetTexture(It.IsAny<string>())).Returns(new int[] { 1, 2, 3 });

            int[] armorTextures = textureRepositoryWrapperMock.Object.GetTexture("speedBonus");

            SpeedBonusFactory speedBonusFactory = new SpeedBonusFactory(gameField, armorTextures);

            Bonus bonus = speedBonusFactory.Spawn(new Point(100, 100));

            Assert.IsInstanceOfType(bonus, typeof(SpeedBonus));
        }
    }
}