using GameEngine.Objects;
using GameLibrary.AmmunitionDecorator;

namespace GameLibrary.Weapons
{
    /// <summary>
    /// Классы оружия с пулями
    /// </summary>
    public class BulletWeapon : GameEngine.Objects.Weapons
    {
        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public BulletWeapon()
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public BulletWeapon(Tank parent)
        {
            Parent = parent;
        }

        public override Ammo Shot()
        {
            if (Parent.Ammunition > 0)
            {
                Parent.Ammunition--;
                Parent.OnChanged();
                Bullet ammoAntiarcraft = new Bullet(Parent.GameField, Parent.Location, Parent, Parent.Direction);
                ammoAntiarcraft.SetTexture();
                return ammoAntiarcraft;
            }

            return null;
        }

        public override Ammo GetAmmo()
        {
            Bullet a = new Bullet(null, new System.Drawing.Point(100, 100), null, 0);
            return a;
        }
    }
}
