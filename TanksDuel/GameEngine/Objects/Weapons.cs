using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Objects
{
    /// <summary>
    /// Класс абстрактного оружия
    /// </summary>
    public abstract class Weapons
    {
        /// <summary>
        /// Ссылка на родительский объект
        /// </summary>
        public Tank Parent { get; set; }
        /// <summary>
        /// Выстрел
        /// </summary>
        public abstract Ammo Shot();

        /// <summary>
        /// Получение вооружения. Для тестов
        /// </summary>
        public abstract Ammo GetAmmo();
    }
}
