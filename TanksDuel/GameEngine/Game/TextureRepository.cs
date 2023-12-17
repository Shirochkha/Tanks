using System.Collections.Generic;

namespace GameEngine.Game
{
    /// <summary>
    /// Класс репозитория текстур
    /// </summary>
    public static class TextureRepository
    {
        /// <summary>
        /// Словарь текстур
        /// </summary>
        private static readonly Dictionary<string, int[]> _textures;

        /// <summary>
        /// Конструктор класса репозитория текстур
        /// </summary>
        static TextureRepository()
        {
            _textures = new Dictionary<string, int[]>();
        }

        /// <summary>
        /// Метод добавления текстуры в репозиторий
        /// </summary>
        public static void Add(string name, int[] textures)
        {
            _textures.Add(name, textures);
        }

        /// <summary>
        /// Метод получения текстуры из репозиторя
        /// </summary>
        public static int[] Get(string name)
        {
            return _textures[name];
        }
    }
}
