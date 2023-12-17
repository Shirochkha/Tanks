using GameEngine.Game;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;

namespace GameLibrary.Converters
{
    /// <summary>
    /// Класс конвертера бонуса
    /// </summary>
    public class BonusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Bonus).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            string bonusType = jObject["Type"].Value<string>();
            string locationString = jObject["Location"].Value<string>();
            var index = locationString.IndexOf(',');
            var x = Convert.ToInt32(locationString.Substring(0, index));
            var y = Convert.ToInt32(locationString.Substring(index + 1, locationString.Length - index - 1));
            var location = new Point(x, y);
            Debug.WriteLine(location);

            Bonus bonus;
            switch (bonusType)
            {
                case nameof(FuelBonus):
                    bonus = new FuelBonus(location, null, TextureRepository.Get("fuelBonus"));
                    break;
                case nameof(SpeedBonus):
                    bonus = new SpeedBonus(location, null, TextureRepository.Get("speedBonus"));
                    break;
                case nameof(DamageBonus):
                    bonus = new DamageBonus(location, null, TextureRepository.Get("damageBonus"));
                    break;
                case nameof(AmmunitionBonus):
                    bonus = new AmmunitionBonus(location, null, TextureRepository.Get("addBulletsBonus"));
                    break;
                case nameof(ArmorBonus):
                    bonus = new ArmorBonus(location, null, TextureRepository.Get("armorBonus"));
                    break;
                default:
                    throw new InvalidOperationException($"Неизвестный тип бонуса: {bonusType}");
            }

            serializer.Populate(jObject.CreateReader(), bonus);

            return bonus;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
