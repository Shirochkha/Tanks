using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameLibrary.Networks
{
    /// <summary>
    /// Класс сообщений
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Конструктор для json
        /// </summary>
        private Message()
        {
        }

        /// <summary>
        /// Конструктор для создания сообщений различных типов
        /// </summary>
        public Message(MessageType type, string content)
        {
            Type = type;
            Content = content;
        }

        /// <summary>
        ///Конструкторконструктор для создания объекта Message из массива байт
        /// </summary>
        public Message(byte[] bytes)
        {
            string json = Encoding.UTF8.GetString(bytes);
            var tempMessage = JsonConvert.DeserializeObject<Message>(json);
            Type = tempMessage.Type;
            Content = tempMessage.Content;
        }

        /// <summary>
        /// Статический метод для десериализации JSON-строки в объект Message
        /// </summary>
        public static IEnumerable<Message> FromJson(string json) =>
            json.Split('*').Select(j => JsonConvert.DeserializeObject<Message>(j));

        /// <summary>
        /// Метод для сериализации объекта Message в JSON-строку
        /// </summary>
        public string AsJson()
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(this, settings);
        }

        /// <summary>
        /// Статический метод для пустого сообщения
        /// </summary>
        public static Message EmptyWithType(MessageType type)
        {
            return new Message(type, string.Empty);
        }

        /// <summary>
        /// Неявное приведение
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator byte[](Message message)
        {
            return Encoding.UTF8.GetBytes(message.ToString());
        }

        public override string ToString()
        {
            return AsJson();
        }
    }
}